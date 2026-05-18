using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MemoApp.Books.Dtos;
using MemoApp.Books.Dtos.Aladin;
using MemoApp.Common.Validators;
using MemoApp.Data;
using MemoApp.Data.Entities;

namespace MemoApp.Books.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IConfiguration _config;

    public BookService(AppDbContext db, IHttpClientFactory httpFactory, IConfiguration config)
    {
        _db = db;
        _httpFactory = httpFactory;
        _config = config;
    }

    // 책 목록 조회 (검색어 있으면 제목으로 필터링)
    public async Task<List<BookDto>> GetBookList(BookQueryDto query)
    {
        IQueryable<Book> books = _db.Books;

        // 검색어 유효한 경우만 필터 적용
        if (InputValidator.IsValidKeyword(query.SearchKeyword))
        {
            var keyword = query.SearchKeyword!.Trim();
            books = books.Where(b => b.Title.Contains(keyword));
        }

        // UI 바인딩용 DTO로 변환해서 반환
        return await books
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                TotalPages = b.TotalPages,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    // 알라딘 ItemSearch API로 책 검색 (제목 키워드)
    // 주의: ItemSearch는 페이지 수 안 줌 → TotalPages=0으로 옴. 상세는 LookupAladin으로
    public async Task<List<BookSearchResultDto>> SearchAladin(string keyword)
    {
        var ttbKey = GetTtbKey();

        var url = "https://www.aladin.co.kr/ttb/api/ItemSearch.aspx" +
                  $"?ttbkey={ttbKey}" +
                  $"&Query={Uri.EscapeDataString(keyword)}" +
                  "&QueryType=Title" +
                  "&SearchTarget=Book" +
                  "&MaxResults=20" +
                  "&output=js" +
                  "&Version=20131101";

        var client = _httpFactory.CreateClient();
        var response = await client.GetStringAsync(url);

        var data = JsonSerializer.Deserialize<AladinSearchResponse>(response);
        if (data?.Item == null) return new List<BookSearchResultDto>();

        // 알라딘 응답 → 우리 DTO로 변환
        return data.Item.Select(i => new BookSearchResultDto
        {
            Title = i.Title,
            Author = i.Author,
            Publisher = i.Publisher,
            TotalPages = 0,
            Isbn13 = i.Isbn13,
            Cover = i.Cover
        }).ToList();
    }

    // 알라딘 ItemLookUp API로 ISBN13 단건 조회 (페이지 수까지 포함)
    public async Task<BookSearchResultDto?> LookupAladin(string isbn13)
    {
        var ttbKey = GetTtbKey();

        var url = "https://www.aladin.co.kr/ttb/api/ItemLookUp.aspx" +
                  $"?ttbkey={ttbKey}" +
                  $"&itemIdType=ISBN13" +
                  $"&ItemId={Uri.EscapeDataString(isbn13)}" +
                  "&output=js" +
                  "&Version=20131101" +
                  "&OptResult=itemPage";

        var client = _httpFactory.CreateClient();
        var response = await client.GetStringAsync(url);

        var data = JsonSerializer.Deserialize<AladinSearchResponse>(response);
        var item = data?.Item?.FirstOrDefault();
        if (item == null) return null;

        return new BookSearchResultDto
        {
            Title = item.Title,
            Author = item.Author,
            Publisher = item.Publisher,
            TotalPages = item.SubInfo?.ItemPage ?? 0,
            Isbn13 = item.Isbn13,
            Cover = item.Cover
        };
    }

    private string GetTtbKey()
    {
        var ttbKey = _config["Aladin:TtbKey"];
        if (string.IsNullOrWhiteSpace(ttbKey))
            throw new InvalidOperationException("알라딘 TTBKey가 설정되지 않았습니다. (appsettings.json 확인)");
        return ttbKey;
    }
}
