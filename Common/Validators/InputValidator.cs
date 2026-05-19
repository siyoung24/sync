namespace MemoApp.Common.Validators;

public static class InputValidator
{
    //검색어가 유효한지 검사하는 메서드
    public static bool IsValidKeyword(string? keyword)
    {
        // IsNullOrWhiteSpace가 null, 빈 문자열, 공백 모두 탐색
        return !string.IsNullOrWhiteSpace(keyword); 
    }
    
    //페이지 번호가 유효한지 검사하는 메서드
    public static bool IsValidPage(int page, int totalPages)
    {
        return page >= 1 && page <= totalPages;
    }
}