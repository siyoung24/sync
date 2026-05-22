using System.Text.Json.Serialization;

namespace MemoApp.Books.Dtos.Aladin;

// 알라딘 ItemSearch API 응답 매핑 (output=js)
public class AladinSearchResponse
{
    [JsonPropertyName("totalResults")]
    public int TotalResults { get; set; }

    [JsonPropertyName("item")]
    public List<AladinItem>? Item { get; set; }
}

public class AladinItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("publisher")]
    public string Publisher { get; set; } = string.Empty;

    [JsonPropertyName("isbn13")]
    public string Isbn13 { get; set; } = string.Empty;

    [JsonPropertyName("cover")]
    public string Cover { get; set; } = string.Empty;

    [JsonPropertyName("subInfo")]
    public AladinSubInfo? SubInfo { get; set; }
}

public class AladinSubInfo
{
    [JsonPropertyName("itemPage")]
    public int ItemPage { get; set; }
}
