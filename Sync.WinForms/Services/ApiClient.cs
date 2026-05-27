using System.Net.Http.Headers;
using System.Net.Http.Json;
using Sync.WinForms.Models;

namespace Sync.WinForms.Services;

public static class ApiClient
{
    private static readonly HttpClient client = new HttpClient();

    public static string BaseUrl { get; set; } = "https://localhost:62776";

    public static async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var response = await client.PostAsJsonAsync($"{BaseUrl}/api/auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "로그인에 실패했습니다.");
        }

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (result == null)
            throw new Exception("서버 응답을 읽을 수 없습니다.");

        return result;
    }

    public static async Task<AuthResponse> RegisterAsync(string name, string email, string password, string passwordConfirm)
    {
        var request = new SignupRequest
        {
            Name = name,
            Email = email,
            Password = password,
            PasswordConfirm = passwordConfirm
        };

        var response = await client.PostAsJsonAsync($"{BaseUrl}/api/auth/register", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "회원가입에 실패했습니다.");
        }

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (result == null)
            throw new Exception("서버 응답을 읽을 수 없습니다.");

        return result;
    }

    public static void SetToken(string token)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public static async Task<List<BookSearchResult>> SearchBooksAsync(string keyword)
    {
        var url = $"{BaseUrl}/api/library/search?keyword={Uri.EscapeDataString(keyword)}";

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "책 검색에 실패했습니다.");
        }

        var result = await response.Content.ReadFromJsonAsync<List<BookSearchResult>>();

        return result ?? new List<BookSearchResult>();
    }

    public static async Task AddMyBookAsync(string isbn13)
    {
        var request = new AddMyBookRequest
        {
            Isbn13 = isbn13
        };

        var response = await client.PostAsJsonAsync($"{BaseUrl}/api/mybooks", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "내 책장 추가에 실패했습니다.");
        }
    }

    public static async Task<List<MyBook>> GetMyBooksAsync()
    {
        var response = await client.GetAsync($"{BaseUrl}/api/mybooks");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "내 책장 조회에 실패했습니다.");
        }

        var result = await response.Content.ReadFromJsonAsync<List<MyBook>>();

        return result ?? new List<MyBook>();
    }

    public static async Task DeleteMyBookAsync(int bookId)
    {
        var response = await client.DeleteAsync($"{BaseUrl}/api/mybooks/{bookId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "책 삭제에 실패했습니다.");
        }
    }

    public static async Task UpdateCurrentPageAsync(int bookId, int currentPage)
    {
        var request = new UpdatePageRequest
        {
            CurrentPage = currentPage
        };

        var response = await client.PatchAsJsonAsync($"{BaseUrl}/api/mybooks/{bookId}/page", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "현재 페이지 수정에 실패했습니다.");
        }
    }

    public static async Task<Review> CreateReviewAsync(int bookId, int page, string content)
    {
        var request = new CreateReviewRequest
        {
            BookId = bookId,
            Page = page,
            Content = content
        };

        var response = await client.PostAsJsonAsync($"{BaseUrl}/api/review", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "기록 저장에 실패했습니다.");
        }

        var result = await response.Content.ReadFromJsonAsync<Review>();

        return result ?? throw new Exception("기록 저장 결과를 불러오지 못했습니다.");
    }

    public static async Task<List<Review>> GetNearbyReviewsAsync(int bookId, int currentPage, int take = 5)
    {
        string url = $"{BaseUrl}/api/review/near?BookId={bookId}&CurrentPage={currentPage}&take={take}";

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "기록 조회에 실패했습니다.");
        }

        var result = await response.Content.ReadFromJsonAsync<List<Review>>();

        return result ?? new List<Review>();
    }

    public static async Task<List<MyReview>> GetMyReviewsAsync()
    {
        var response = await client.GetAsync($"{BaseUrl}/api/review/mine");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Message ?? "내 기록 조회에 실패했습니다.");
        }

        var result = await response.Content.ReadFromJsonAsync<List<MyReview>>();

        return result ?? new List<MyReview>();
    }
}