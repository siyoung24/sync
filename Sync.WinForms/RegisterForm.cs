using Sync.WinForms.Services;

namespace Sync.WinForms;

public partial class RegisterForm : Form
{
    private RoundedTextBox txtName = null!;
    private RoundedTextBox txtEmail = null!;
    private RoundedTextBox txtPassword = null!;
    private RoundedTextBox txtPasswordConfirm = null!;
    private RoundedButton btnRegister = null!;

    public RegisterForm()
    {
        InitializeComponent();
        BuildRegisterUI();
    }

    private void BuildRegisterUI()
    {
        Controls.Clear();

        AutoScaleMode = AutoScaleMode.None;

        Text = "Sync - 회원가입";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1050, 680);
        BackColor = ColorTranslator.FromHtml("#f7f7f4");
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        RoundedPanel cardPanel = new RoundedPanel
        {
            Size = new Size(450, 600),
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
            Location = new Point(45, 35),
            BackColor = Color.White
        };
        cardPanel.Controls.Add(lblTitle);

        Label lblSubTitle = new Label
        {
            Text = "페이지 기반 한줄평 시스템",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            AutoSize = true,
            Location = new Point(48, 85),
            BackColor = Color.White
        };
        cardPanel.Controls.Add(lblSubTitle);

        Label lblLoginTab = new Label
        {
            Text = "로그인",
            Font = new Font("맑은 고딕", 11),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            AutoSize = true,
            Location = new Point(115, 140),
            BackColor = Color.White,
            Cursor = Cursors.Hand
        };
        lblLoginTab.Click += LblLoginTab_Click;
        cardPanel.Controls.Add(lblLoginTab);

        Label lblRegisterTab = new Label
        {
            Text = "회원가입",
            Font = new Font("맑은 고딕", 12, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#3f6752"),
            AutoSize = true,
            Location = new Point(285, 140),
            BackColor = Color.White
        };
        cardPanel.Controls.Add(lblRegisterTab);

        Panel lineBase = new Panel
        {
            Size = new Size(360, 1),
            BackColor = ColorTranslator.FromHtml("#e7e7e7"),
            Location = new Point(45, 180)
        };
        cardPanel.Controls.Add(lineBase);

        Panel activeLine = new Panel
        {
            Size = new Size(120, 2),
            BackColor = ColorTranslator.FromHtml("#3f6752"),
            Location = new Point(265, 179)
        };
        cardPanel.Controls.Add(activeLine);

        Label lblName = CreateInputLabel("이름", 45, 195);
        cardPanel.Controls.Add(lblName);

        txtName = CreateInputBox(45, 222);
        cardPanel.Controls.Add(txtName);

        Label lblEmail = CreateInputLabel("이메일", 45, 277);
        cardPanel.Controls.Add(lblEmail);

        txtEmail = CreateInputBox(45, 304);
        cardPanel.Controls.Add(txtEmail);

        Label lblPassword = CreateInputLabel("비밀번호", 45, 359);
        cardPanel.Controls.Add(lblPassword);

        txtPassword = CreateInputBox(45, 386);
        txtPassword.UseSystemPasswordChar = true;
        cardPanel.Controls.Add(txtPassword);

        Label lblPasswordConfirm = CreateInputLabel("비밀번호 확인", 45, 441);
        cardPanel.Controls.Add(lblPasswordConfirm);

        txtPasswordConfirm = CreateInputBox(45, 486);
        txtPasswordConfirm.UseSystemPasswordChar = true;
        cardPanel.Controls.Add(txtPasswordConfirm);

        btnRegister = new RoundedButton
        {
            Text = "가입하기",
            Font = new Font("맑은 고딕", 11, FontStyle.Bold),
            Size = new Size(360, 46),
            Location = new Point(45, 535),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };

        cardPanel.Controls.Add(btnRegister);
        btnRegister.Click += BtnRegister_Click;
    }

    private Label CreateInputLabel(string text, int x, int y)
    {
        return new Label
        {
            Text = text,
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(160, 24),
            Location = new Point(x, y),
            BackColor = Color.White
        };
    }

    private RoundedTextBox CreateInputBox(int x, int y)
    {
        return new RoundedTextBox
        {
            Size = new Size(360, 44),
            Location = new Point(x, y),
            BorderRadius = 10,
            BorderColor = ColorTranslator.FromHtml("#e2e2e2"),
            BackColor = Color.White,
            Font = new Font("맑은 고딕", 11)
        };
    }

    private async void BtnRegister_Click(object? sender, EventArgs e)
    {
        string name = txtName.Text.Trim();
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text.Trim();
        string passwordConfirm = txtPasswordConfirm.Text.Trim();

        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(passwordConfirm))
        {
            MessageBox.Show("모든 항목을 입력해주세요.");
            return;
        }

        if (password != passwordConfirm)
        {
            MessageBox.Show("비밀번호와 비밀번호 확인이 일치하지 않습니다.");
            return;
        }

        try
        {
            btnRegister.Enabled = false;
            btnRegister.Text = "가입 중...";

            await ApiClient.RegisterAsync(name, email, password, passwordConfirm);

            MessageBox.Show("회원가입이 완료되었습니다. 로그인 화면으로 이동합니다.");

            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            btnRegister.Enabled = true;
            btnRegister.Text = "가입하기";
        }
    }

    private void LblLoginTab_Click(object? sender, EventArgs e)
    {
        LoginForm loginForm = new LoginForm();
        loginForm.Show();
        Close();
    }

    private void RegisterForm_Load(object sender, EventArgs e)
    {
    }
}