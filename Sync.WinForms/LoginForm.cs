using System.Drawing.Drawing2D;
using Sync.WinForms.Services;

namespace Sync.WinForms;

public partial class LoginForm : Form
{
    private RoundedTextBox txtEmail = null!;
    private RoundedTextBox txtPassword = null!;
    private RoundedButton btnLogin = null!;
    private Label lblGoRegister = null!;

    public LoginForm()
    {
        InitializeComponent();
        BuildLoginUI();
    }

    private void BuildLoginUI()
    {
        Controls.Clear();

        Text = "Sync - 로그인";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1050, 680);
        BackColor = ColorTranslator.FromHtml("#f7f7f4");
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        RoundedPanel cardPanel = new RoundedPanel
        {
            Size = new Size(450, 500),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 16
        };

        cardPanel.Location = new Point(
            (ClientSize.Width - cardPanel.Width) / 2,
            (ClientSize.Height - cardPanel.Height) / 2
        );

        Controls.Add(cardPanel);

        Label lblTitle = new Label
        {
            Text = "Sync",
            Font = new Font("맑은 고딕", 24, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            AutoSize = true,
            Location = new Point(45, 42),
            BackColor = Color.White
        };
        cardPanel.Controls.Add(lblTitle);

        Label lblSubTitle = new Label
        {
            Text = "페이지 기반 한줄평 시스템",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            AutoSize = true,
            Location = new Point(48, 92),
            BackColor = Color.White
        };
        cardPanel.Controls.Add(lblSubTitle);

        Label lblLoginTab = new Label
        {
            Text = "로그인",
            Font = new Font("맑은 고딕", 12, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#3f6752"),
            AutoSize = true,
            Location = new Point(115, 155),
            BackColor = Color.White
        };
        cardPanel.Controls.Add(lblLoginTab);

        lblGoRegister = new Label
        {
            Text = "회원가입",
            Font = new Font("맑은 고딕", 11, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            AutoSize = true,
            Location = new Point(292, 156),
            BackColor = Color.White,
            Cursor = Cursors.Hand
        };
        lblGoRegister.Click += btnGoRegister_Click;
        cardPanel.Controls.Add(lblGoRegister);

        Panel lineBase = new Panel
        {
            Size = new Size(360, 1),
            BackColor = ColorTranslator.FromHtml("#e7e7e7"),
            Location = new Point(45, 195)
        };
        cardPanel.Controls.Add(lineBase);

        Panel activeLine = new Panel
        {
            Size = new Size(120, 2),
            BackColor = ColorTranslator.FromHtml("#3f6752"),
            Location = new Point(95, 194)
        };
        cardPanel.Controls.Add(activeLine);

        Label lblEmail = new Label
        {
            Text = "이메일",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            AutoSize = true,
            Location = new Point(45, 232),
            BackColor = Color.White
        };
        cardPanel.Controls.Add(lblEmail);

        txtEmail = new RoundedTextBox
        {
            Size = new Size(360, 50),
            Location = new Point(45, 260),
            BorderRadius = 8,
            BorderColor = ColorTranslator.FromHtml("#e2e2e2"),
            BackColor = Color.White,
            Font = new Font("맑은 고딕", 11)
        };
        cardPanel.Controls.Add(txtEmail);

        Label lblPassword = new Label
        {
            Text = "비밀번호",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            AutoSize = true,
            Location = new Point(45, 325),
            BackColor = Color.White
        };
        cardPanel.Controls.Add(lblPassword);

        txtPassword = new RoundedTextBox
        {
            Size = new Size(360, 50),
            Location = new Point(45, 353),
            BorderRadius = 10,
            BorderColor = ColorTranslator.FromHtml("#e2e2e2"),
            BackColor = Color.White,
            Font = new Font("맑은 고딕", 11),
            UseSystemPasswordChar = true
        };
        cardPanel.Controls.Add(txtPassword);

        btnLogin = new RoundedButton
        {
            Text = "로그인",
            Font = new Font("맑은 고딕", 12, FontStyle.Bold),
            Size = new Size(360, 52),
            Location = new Point(45, 420),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 10,
            Cursor = Cursors.Hand
        };
        btnLogin.Click += btnLogin_Click;
        cardPanel.Controls.Add(btnLogin);
    }

    private async void btnLogin_Click(object? sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text.Trim();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("이메일과 비밀번호를 입력해주세요.");
            return;
        }

        try
        {
            btnLogin.Enabled = false;
            btnLogin.Text = "로그인 중...";

            var result = await ApiClient.LoginAsync(email, password);

            AppSession.AccessToken = result.AccessToken;
            AppSession.UserId = result.User.Id;
            AppSession.UserName = result.User.Name;
            AppSession.UserEmail = result.User.Email;

            ApiClient.SetToken(result.AccessToken);

            MessageBox.Show($"{AppSession.UserName}님 로그인 성공!");

            MainForm mainForm = new MainForm();
            mainForm.Show();
            Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            btnLogin.Enabled = true;
            btnLogin.Text = "로그인";
        }
    }

    private void btnGoRegister_Click(object? sender, EventArgs e)
    {
        RegisterForm registerForm = new RegisterForm();
        registerForm.Show();
        Hide();
    }

    private void LoginForm_Load(object sender, EventArgs e)
    {
    }
}

public class RoundedPanel : Panel
{
    public int BorderRadius { get; set; } = 16;
    public Color BorderColor { get; set; } = ColorTranslator.FromHtml("#e6e6e6");

    public RoundedPanel()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);

        if (Width > 0 && Height > 0)
        {
            using GraphicsPath path = GetRoundedRect(new Rectangle(0, 0, Width, Height), BorderRadius);
            Region = new Region(path);
        }

        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

        using GraphicsPath path = GetRoundedRect(rect, BorderRadius);
        using SolidBrush brush = new SolidBrush(BackColor);
        using Pen pen = new Pen(BorderColor, 1);

        e.Graphics.FillPath(brush, path);
        e.Graphics.DrawPath(pen, path);
    }

    private static GraphicsPath GetRoundedRect(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;

        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();

        return path;
    }
}

public class RoundedTextBox : UserControl
{
    private readonly TextBox innerTextBox = new TextBox();

    public int BorderRadius { get; set; } = 10;
    public Color BorderColor { get; set; } = ColorTranslator.FromHtml("#e2e2e2");

    public override string Text
    {
        get => innerTextBox.Text;
        set => innerTextBox.Text = value;
    }

    public override Font Font
    {
        get => base.Font;
        set
        {
            base.Font = value;
            innerTextBox.Font = value;
        }
    }

    public bool UseSystemPasswordChar
    {
        get => innerTextBox.UseSystemPasswordChar;
        set => innerTextBox.UseSystemPasswordChar = value;
    }

    public RoundedTextBox()
    {
        DoubleBuffered = true;
        BackColor = Color.White;

        innerTextBox.BorderStyle = BorderStyle.None;
        innerTextBox.BackColor = Color.White;
        innerTextBox.ForeColor = Color.Black;
        innerTextBox.Location = new Point(14, 15);
        innerTextBox.Width = Width - 28;

        Controls.Add(innerTextBox);

        Click += (s, e) => innerTextBox.Focus();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        innerTextBox.Width = Width - 28;
        innerTextBox.Location = new Point(14, (Height - innerTextBox.Height) / 2);

        if (Width > 0 && Height > 0)
        {
            using GraphicsPath path = GetRoundedRect(new Rectangle(0, 0, Width, Height), BorderRadius);
            Region = new Region(path);
        }

        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

        using GraphicsPath path = GetRoundedRect(rect, BorderRadius);
        using SolidBrush brush = new SolidBrush(BackColor);
        using Pen pen = new Pen(BorderColor, 1);

        e.Graphics.FillPath(brush, path);
        e.Graphics.DrawPath(pen, path);
    }

    private static GraphicsPath GetRoundedRect(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;

        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();

        return path;
    }
}


public class RoundedButton : UserControl
{
    private string buttonText = "버튼";

    public int BorderRadius { get; set; } = 10;

    public override string Text
    {
        get => buttonText;
        set
        {
            buttonText = value;
            Invalidate();
        }
    }

    public RoundedButton()
    {
        DoubleBuffered = true;

        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true
        );

        BackColor = ColorTranslator.FromHtml("#436b55");
        ForeColor = Color.White;
        Cursor = Cursors.Hand;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // 기본 사각형 배경 방지
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

        Color parentColor = Parent?.BackColor ?? Color.White;
        using (SolidBrush parentBrush = new SolidBrush(parentColor))
        {
            e.Graphics.FillRectangle(parentBrush, ClientRectangle);
        }

        Rectangle rect = new Rectangle(1, 1, Width - 3, Height - 3);

        using System.Drawing.Drawing2D.GraphicsPath path = GetRoundedRect(rect, BorderRadius);
        using SolidBrush buttonBrush = new SolidBrush(
            Enabled ? BackColor : ColorTranslator.FromHtml("#9aa89f")
        );

        e.Graphics.FillPath(buttonBrush, path);

        using StringFormat sf = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        using SolidBrush textBrush = new SolidBrush(ForeColor);

        RectangleF textRect = new RectangleF(0, -1, Width, Height);
        e.Graphics.DrawString(Text, Font, textBrush, textRect, sf);
    }

    private static System.Drawing.Drawing2D.GraphicsPath GetRoundedRect(Rectangle rect, int radius)
    {
        System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
        int d = radius * 2;

        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();

        return path;
    }
}