namespace Sync.WinForms;

public static class AppSession
{
    public static string AccessToken { get; set; } = "";
    public static int UserId { get; set; }
    public static string UserName { get; set; } = "";
    public static string UserEmail { get; set; } = "";

    public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(AccessToken);

    public static void Clear()
    {
        AccessToken = "";
        UserId = 0;
        UserName = "";
        UserEmail = "";
    }
}