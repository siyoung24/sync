namespace Sync.WinForms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        BuildMainUI();
    }

    private void BuildMainUI()
    {
        Controls.Clear();

        AppLayout.SetupPage(this, "Sync - 메인");
        AppLayout.AddSidebar(this, "home");
        
        // 왼쪽 사이드바
        Panel sidebar = new Panel
        {
            Size = new Size(230, ClientSize.Height),
            Location = new Point(0, 0),
            BackColor = Color.White
        };
        Controls.Add(sidebar);

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
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            AutoSize = true,
            Location = new Point(30, 88),
            BackColor = Color.White
        };
        sidebar.Controls.Add(lblUser);

        Label lblEmail = new Label
        {
            Text = AppSession.UserEmail,
            Font = new Font("맑은 고딕", 8, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#999999"),
            AutoSize = true,
            Location = new Point(30, 112),
            BackColor = Color.White
        };
        sidebar.Controls.Add(lblEmail);

        RoundedButton btnMyBooks = CreateSideButton("내 책장", 165);
        btnMyBooks.Click += BtnMyBooks_Click;
        sidebar.Controls.Add(btnMyBooks);

        RoundedButton btnBookSearch = CreateSideButton("책 검색", 220);
        btnBookSearch.Click += BtnBookSearch_Click;
        sidebar.Controls.Add(btnBookSearch);

        RoundedButton btnMyReviews = CreateSideButton("내 작성글", 275);
        btnMyReviews.Click += BtnMyReviews_Click;
        sidebar.Controls.Add(btnMyReviews);

        Label lblLogout = new Label
        {
            Text = "로그아웃",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#b45b5b"),
            AutoSize = true,
            Location = new Point(30, 560),
            BackColor = Color.White,
            Cursor = Cursors.Hand
        };
        lblLogout.Click += LblLogout_Click;
        sidebar.Controls.Add(lblLogout);

        // 오른쪽 메인 영역
        Label lblTitle = new Label
        {
            Text = "오늘의 독서 기록을 시작해볼까요?",
            Font = new Font("맑은 고딕", 21, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(720, 55),
            Location = new Point(285, 45),
            BackColor = ColorTranslator.FromHtml("#f7f7f4")
        };
        Controls.Add(lblTitle);

        Label lblSub = new Label
        {
            Text = "읽고 있는 책을 등록하고, 페이지별 한줄평을 남겨보세요.",
            Font = new Font("맑은 고딕", 11, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(760, 30),
            Location = new Point(288, 112),
            BackColor = ColorTranslator.FromHtml("#f7f7f4")
        };
        Controls.Add(lblSub);

        RoundedPanel card1 = CreateMenuCard(
            "읽고 있는 책 설정",
            "현재 읽는 책을 최대 3권까지 등록하고 관리할 수 있어요.",
            "내 책장 열기",
            new Point(285, 190)
        );
        Controls.Add(card1);

        RoundedPanel card2 = CreateMenuCard(
            "책 검색",
            "알라딘 검색을 통해 읽을 책을 찾고 내 책장에 추가할 수 있어요.",
            "책 검색하기",
            new Point(630, 190)
        );
        Controls.Add(card2);

        RoundedPanel card3 = CreateWideCard(
            "내가 쓴 기록 모아보기",
            "작성한 한줄평을 책별로 모아보고, 이전 독서 흐름을 다시 확인할 수 있어요.",
            "내 기록 보기",
            new Point(285, 435)
        );
        Controls.Add(card3);
    }

    private RoundedButton CreateSideButton(string text, int y)
    {
        RoundedButton button = new RoundedButton
        {
            Text = text,
            Font = new Font("맑은 고딕", 10, FontStyle.Bold),
            Size = new Size(170, 42),
            Location = new Point(30, y),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };

        return button;
    }

    private RoundedPanel CreateMenuCard(string title, string description, string buttonText, Point location)
    {
        RoundedPanel card = new RoundedPanel
        {
            Size = new Size(310, 195),
            Location = location,
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 16
        };

        Label lblTitle = new Label
        {
            Text = title,
            Font = new Font("맑은 고딕", 14, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(260, 32),
            Location = new Point(25, 30),
            BackColor = Color.White
        };
        card.Controls.Add(lblTitle);

        Label lblDesc = new Label
        {
            Text = description,
            Font = new Font("맑은 고딕", 9, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(255, 52),
            Location = new Point(25, 78),
            BackColor = Color.White
        };
        card.Controls.Add(lblDesc);

        RoundedButton btn = new RoundedButton
        {
            Text = buttonText,
            Font = new Font("맑은 고딕", 10, FontStyle.Bold),
            Size = new Size(255, 42),
            Location = new Point(25, 132),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };

        if (buttonText.Contains("책장"))
            btn.Click += BtnMyBooks_Click;
        else if (buttonText.Contains("검색"))
            btn.Click += BtnBookSearch_Click;

        card.Controls.Add(btn);

        return card;
    }

    private RoundedPanel CreateWideCard(string title, string description, string buttonText, Point location)
    {
        RoundedPanel card = new RoundedPanel
        {
            Size = new Size(655, 155),
            Location = location,
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 16
        };

        Label lblTitle = new Label
        {
            Text = title,
            Font = new Font("맑은 고딕", 14, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(360, 32),
            Location = new Point(28, 30),
            BackColor = Color.White
        };
        card.Controls.Add(lblTitle);

        Label lblDesc = new Label
        {
            Text = description,
            Font = new Font("맑은 고딕", 9, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(420, 50),
            Location = new Point(28, 75),
            BackColor = Color.White
        };
        card.Controls.Add(lblDesc);

        RoundedButton btn = new RoundedButton
        {
            Text = buttonText,
            Font = new Font("맑은 고딕", 10, FontStyle.Bold),
            Size = new Size(150, 42),
            Location = new Point(470, 58),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };
        btn.Click += BtnMyReviews_Click;
        card.Controls.Add(btn);

        return card;
    }

    private void BtnMyBooks_Click(object? sender, EventArgs e)
{
    MyBooksForm myBooksForm = new MyBooksForm();
    myBooksForm.Show();
    Close();
}

    private void BtnBookSearch_Click(object? sender, EventArgs e)
    {
        BookSearchForm bookSearchForm = new BookSearchForm();
        bookSearchForm.Show();
        Close();
    }

    private void BtnMyReviews_Click(object? sender, EventArgs e)
    {
        MyReviewsForm myReviewsForm = new MyReviewsForm();
        myReviewsForm.Show();
        Close();
    }

    private void LblLogout_Click(object? sender, EventArgs e)
    {
        AppSession.Clear();

        LoginForm loginForm = new LoginForm();
        loginForm.Show();
        Close();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }
}