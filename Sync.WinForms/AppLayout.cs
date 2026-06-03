namespace Sync.WinForms;

public static class AppLayout
{
    public const int SidebarWidth = 230;
    public const int ContentX = 285;
    public const int ContentTop = 55;
    public const int ContentWidth = 860;

    public static void SetupPage(Form form, string title)
    {
        form.Text = title;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Size = new Size(1200, 720);
        form.BackColor = ColorTranslator.FromHtml("#f7f7f4");
        form.FormBorderStyle = FormBorderStyle.FixedSingle;
        form.MaximizeBox = false;
        form.AutoScaleMode = AutoScaleMode.None;
    }

    public static void AddSidebar(Form form, string activeMenu)
    {
        Panel sidebar = new Panel
        {
            Size = new Size(SidebarWidth, form.ClientSize.Height),
            Location = new Point(0, 0),
            BackColor = Color.White
        };
        form.Controls.Add(sidebar);
        sidebar.BringToFront();

        Label lblLogo = new Label
        {
            Text = "Sync",
            Font = new Font("맑은 고딕", 22, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            AutoSize = true,
            Location = new Point(28, 42),
            BackColor = Color.White
        };
        sidebar.Controls.Add(lblLogo);

        Label lblUser = new Label
        {
            Text = $"{AppSession.UserName}님",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            AutoSize = true,
            Location = new Point(30, 88),
            BackColor = Color.White
        };
        sidebar.Controls.Add(lblUser);

        Label lblEmail = new Label
        {
            Text = AppSession.UserEmail,
            Font = new Font("맑은 고딕", 8),
            ForeColor = ColorTranslator.FromHtml("#999999"),
            AutoSize = true,
            Location = new Point(30, 112),
            BackColor = Color.White
        };
        sidebar.Controls.Add(lblEmail);

        RoundedButton btnHome = CreateNavButton(form, "홈", "home", activeMenu, 165, () => new MainForm());
        sidebar.Controls.Add(btnHome);

        RoundedButton btnBooks = CreateNavButton(form, "내 책장", "books", activeMenu, 220, () => new MyBooksForm());
        sidebar.Controls.Add(btnBooks);

        RoundedButton btnSearch = CreateNavButton(form, "책 검색", "search", activeMenu, 275, () => new BookSearchForm());
        sidebar.Controls.Add(btnSearch);

        RoundedButton btnReviews = CreateNavButton(form, "내 작성글", "reviews", activeMenu, 330, () => new MyReviewsForm());
        sidebar.Controls.Add(btnReviews);

        Label lblLogout = new Label
        {
            Text = "로그아웃",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#b45b5b"),
            AutoSize = true,
            Location = new Point(30, 610),
            BackColor = Color.White,
            Cursor = Cursors.Hand
        };

        lblLogout.Click += (s, e) =>
        {
            AppSession.Clear();

            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            form.Close();
        };

        sidebar.Controls.Add(lblLogout);
    }

    private static RoundedButton CreateNavButton(
        Form currentForm,
        string text,
        string key,
        string activeMenu,
        int y,
        Func<Form> createForm)
    {
        bool isActive = key == activeMenu;

        RoundedButton button = new RoundedButton
        {
            Text = text,
            Font = new Font("맑은 고딕", 10, FontStyle.Bold),
            Size = new Size(170, 42),
            Location = new Point(30, y),
            BackColor = isActive
                ? ColorTranslator.FromHtml("#315843")
                : ColorTranslator.FromHtml("#7A9686"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = isActive ? Cursors.Default : Cursors.Hand
        };

        if (!isActive)
        {
            button.Click += (s, e) =>
            {
                Form nextForm = createForm();
                nextForm.Show();
                currentForm.Close();
            };
        }

        return button;
    }
}