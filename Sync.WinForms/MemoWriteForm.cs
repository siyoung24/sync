using Sync.WinForms.Models;
using Sync.WinForms.Services;

namespace Sync.WinForms;

public partial class MemoWriteForm : Form
{
    private readonly MyBook selectedBook;

    private TextBox txtPage = null!;
    private TextBox txtTotalPage = null!;
    private TextBox txtContent = null!;
    private RadioButton rbPaper = null!;
    private RadioButton rbEbook = null!;
    private RoundedButton btnSave = null!;

    public MemoWriteForm(MyBook book)
    {
        InitializeComponent();
        selectedBook = book;
        BuildMemoWriteUI();
    }

    private void BuildMemoWriteUI()
    {
        Controls.Clear();

        AutoScaleMode = AutoScaleMode.None;

        Text = "Sync - 기록하기";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1050, 680);
        BackColor = ColorTranslator.FromHtml("#f7f7f4");
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        Label lblTitle = new Label
        {
            Text = "기록하기",
            Font = new Font("맑은 고딕", 24, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(400, 80),
            Location = new Point(60, 25),
            TextAlign = ContentAlignment.TopLeft,
            BackColor = ColorTranslator.FromHtml("#f7f7f4")
        };
        Controls.Add(lblTitle);

        Label lblSub = new Label
        {
            Text = "읽고 있는 페이지를 입력하고, 해당 페이지에 대한 한줄평을 남겨보세요.",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(720, 30),
            Location = new Point(63, 112),
            TextAlign = ContentAlignment.TopLeft,
            BackColor = ColorTranslator.FromHtml("#f7f7f4")
        };
        Controls.Add(lblSub);

        Label lblBack = new Label
        {
            Text = "← 내 책장으로",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#436b55"),
            AutoSize = true,
            Location = new Point(850, 58),
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            Cursor = Cursors.Hand
        };
        lblBack.Click += LblBack_Click;
        Controls.Add(lblBack);

        RoundedPanel mainCard = new RoundedPanel
        {
            Size = new Size(900, 440),
            Location = new Point(60, 170),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 16
        };
        Controls.Add(mainCard);

        RoundedPanel coverBox = new RoundedPanel
        {
            Size = new Size(80, 115),
            Location = new Point(35, 35),
            BackColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderRadius = 4
        };
        mainCard.Controls.Add(coverBox);

        if (!string.IsNullOrWhiteSpace(selectedBook.Cover))
        {
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(80, 115),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ColorTranslator.FromHtml("#eeeeee")
            };
            coverBox.Controls.Add(pictureBox);
            pictureBox.LoadAsync(selectedBook.Cover);
        }
        else
        {
            Label lblCover = new Label
            {
                Text = "Book",
                Font = new Font("맑은 고딕", 9),
                ForeColor = ColorTranslator.FromHtml("#777777"),
                Size = new Size(80, 115),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#eeeeee")
            };
            coverBox.Controls.Add(lblCover);
        }

        Label lblBookTitle = new Label
        {
            Text = selectedBook.Title,
            Font = new Font("맑은 고딕", 15, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(680, 34),
            Location = new Point(140, 35),
            BackColor = Color.White
        };
        mainCard.Controls.Add(lblBookTitle);

        Label lblAuthor = new Label
        {
            Text = selectedBook.Author,
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(680, 25),
            Location = new Point(140, 72),
            BackColor = Color.White
        };
        mainCard.Controls.Add(lblAuthor);

        Label lblGuide = new Label
        {
            Text = $"현재 저장된 페이지: {selectedBook.CurrentPage} / {selectedBook.TotalPages}",
            Font = new Font("맑은 고딕", 10, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#436b55"),
            Size = new Size(680, 25),
            Location = new Point(140, 102),
            BackColor = Color.White
        };
        mainCard.Controls.Add(lblGuide);

        rbPaper = new RadioButton
        {
            Text = "종이책",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#333333"),
            Location = new Point(140, 135),
            Size = new Size(90, 25),
            BackColor = Color.White,
            Checked = true
        };
        mainCard.Controls.Add(rbPaper);

        rbEbook = new RadioButton
        {
            Text = "전자책",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#333333"),
            Location = new Point(240, 135),
            Size = new Size(90, 25),
            BackColor = Color.White
        };
        mainCard.Controls.Add(rbEbook);

        Label lblPage = new Label
        {
            Text = "현재 페이지",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(150, 28),
            Location = new Point(35, 185),
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.White
        };
        mainCard.Controls.Add(lblPage);

        txtPage = new TextBox
        {
            Text = selectedBook.CurrentPage > 0 ? selectedBook.CurrentPage.ToString() : "",
            Font = new Font("맑은 고딕", 11),
            Size = new Size(150, 32),
            Location = new Point(35, 215)
        };
        mainCard.Controls.Add(txtPage);

        Label lblSlash = new Label
        {
            Text = "/",
            Font = new Font("맑은 고딕", 14, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#888888"),
            Size = new Size(30, 32),
            Location = new Point(205, 214),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.White
        };
        mainCard.Controls.Add(lblSlash);

        Label lblTotal = new Label
        {
            Text = "전체 페이지",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(150, 28),
            Location = new Point(275, 185),
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.White
        };
        mainCard.Controls.Add(lblTotal);

        txtTotalPage = new TextBox
        {
            Text = selectedBook.TotalPages.ToString(),
            Font = new Font("맑은 고딕", 11),
            Size = new Size(150, 32),
            Location = new Point(275, 215),
            ReadOnly = true,
            BackColor = ColorTranslator.FromHtml("#f4f4f4")
        };
        mainCard.Controls.Add(txtTotalPage);

        rbPaper.CheckedChanged += ReadingTypeChanged;
        rbEbook.CheckedChanged += ReadingTypeChanged;

        ReadingTypeChanged(null, EventArgs.Empty);
        Label lblContent = new Label
        {
            Text = "한줄평",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(100, 28),
            Location = new Point(35, 270),
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.White
        };
        mainCard.Controls.Add(lblContent);

        txtContent = new TextBox
        {
            Font = new Font("맑은 고딕", 11),
            Size = new Size(620, 80),
            Location = new Point(35, 300),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
        mainCard.Controls.Add(txtContent);

        btnSave = new RoundedButton
        {
            Text = "기록 저장",
            Font = new Font("맑은 고딕", 11, FontStyle.Bold),
            Size = new Size(160, 46),
            Location = new Point(690, 335),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };
        btnSave.Click += BtnSave_Click;
        mainCard.Controls.Add(btnSave);
    }

    private void ReadingTypeChanged(object? sender, EventArgs e)
    {
        if (rbEbook.Checked)
        {
            txtTotalPage.ReadOnly = false;
            txtTotalPage.BackColor = Color.White;
            txtTotalPage.Text = "";
        }
        else
        {
            txtTotalPage.ReadOnly = true;
            txtTotalPage.BackColor = ColorTranslator.FromHtml("#f4f4f4");
            txtTotalPage.Text = selectedBook.TotalPages.ToString();
        }
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtPage.Text.Trim(), out int inputCurrent))
        {
            MessageBox.Show("현재 페이지는 숫자로 입력해주세요.");
            return;
        }

        if (!int.TryParse(txtTotalPage.Text.Trim(), out int inputTotal))
        {
            MessageBox.Show("전체 페이지는 숫자로 입력해주세요.");
            return;
        }

        int page;

        if (rbEbook.Checked)
        {
            if (inputCurrent < 1 || inputTotal < 1 || inputCurrent > inputTotal)
            {
                MessageBox.Show("전자책 기준 현재 위치와 전체 위치를 올바르게 입력해주세요.");
                return;
            }

            // 전자책 위치를 종이책 기준 페이지로 환산
            page = (int)Math.Ceiling((double)inputCurrent / inputTotal * selectedBook.TotalPages);

            if (page < 1)
                page = 1;

            if (page > selectedBook.TotalPages)
                page = selectedBook.TotalPages;
        }
        else
        {
            page = inputCurrent;

            if (page < 1 || page > selectedBook.TotalPages)
            {
                MessageBox.Show($"1부터 {selectedBook.TotalPages} 사이의 페이지를 입력해주세요.");
                return;
            }
        }

        string content = txtContent.Text.Trim();

        if (string.IsNullOrWhiteSpace(content))
        {
            MessageBox.Show("한줄평을 입력해주세요.");
            return;
        }

        try
        {
            btnSave.Enabled = false;
            btnSave.Text = "저장 중...";

            await ApiClient.UpdateCurrentPageAsync(selectedBook.BookId, page);
            await ApiClient.CreateReviewAsync(selectedBook.BookId, page, content);

            if (rbEbook.Checked)
            {
                MessageBox.Show($"기록이 저장되었습니다.\n전자책 위치를 종이책 기준 p.{page}로 환산해 저장했습니다.");
            }
            else
            {
                MessageBox.Show("기록이 저장되었습니다.");
            }

            MyBooksForm myBooksForm = new MyBooksForm();
            myBooksForm.Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            btnSave.Enabled = true;
            btnSave.Text = "기록 저장";
        }
    }

    private void LblBack_Click(object? sender, EventArgs e)
    {
        MyBooksForm myBooksForm = new MyBooksForm();
        myBooksForm.Show();
        Close();
    }

    private void MemoWriteForm_Load(object sender, EventArgs e)
    {
    }
}