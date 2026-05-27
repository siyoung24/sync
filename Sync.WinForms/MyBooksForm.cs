using Sync.WinForms.Models;
using Sync.WinForms.Services;

namespace Sync.WinForms;

public partial class MyBooksForm : Form
{
    private FlowLayoutPanel booksPanel = null!;

    public MyBooksForm()
    {
        InitializeComponent();
        BuildMyBooksUI();
    }

    private async void BuildMyBooksUI()
    {
        Controls.Clear();

        AutoScaleMode = AutoScaleMode.None;

        Text = "Sync - 내 책장";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1050, 680);
        BackColor = ColorTranslator.FromHtml("#f7f7f4");
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        Label lblTitle = new Label
        {
            Text = "내 책장",
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
            Text = "읽고 있는 책을 관리하고, 페이지별 기록을 남길 수 있어요.",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(700, 30),
            Location = new Point(63, 112),
            TextAlign = ContentAlignment.TopLeft,
            BackColor = ColorTranslator.FromHtml("#f7f7f4")
        };
        Controls.Add(lblSub);

        Label lblBack = new Label
        {
            Text = "← 메인으로",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#436b55"),
            AutoSize = true,
            Location = new Point(880, 58),
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            Cursor = Cursors.Hand
        };
        lblBack.Click += LblBack_Click;
        Controls.Add(lblBack);

        RoundedPanel infoBox = new RoundedPanel
        {
            Size = new Size(900, 70),
            Location = new Point(60, 160),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 16
        };
        Controls.Add(infoBox);

        Label lblInfo = new Label
        {
            Text = "책 검색 화면에서 최대 3권까지 책을 추가할 수 있습니다.",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(820, 30),
            Location = new Point(28, 23),
            BackColor = Color.White
        };
        infoBox.Controls.Add(lblInfo);

        booksPanel = new FlowLayoutPanel
        {
            Size = new Size(920, 370),
            Location = new Point(60, 260),
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true
        };
        Controls.Add(booksPanel);

        await LoadMyBooksAsync();
    }

    private async Task LoadMyBooksAsync()
    {
        try
        {
            booksPanel.Controls.Clear();

            Label loading = new Label
            {
                Text = "내 책장을 불러오는 중입니다...",
                Font = new Font("맑은 고딕", 11, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#888888"),
                Size = new Size(850, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#f7f7f4"),
                Margin = new Padding(0, 80, 0, 0)
            };
            booksPanel.Controls.Add(loading);

            List<MyBook> books = await ApiClient.GetMyBooksAsync();

            booksPanel.Controls.Clear();

            if (books.Count == 0)
            {
                ShowEmptyState();
                return;
            }

            foreach (MyBook book in books)
            {
                booksPanel.Controls.Add(CreateMyBookCard(book));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            ShowEmptyState();
        }
    }

    private void ShowEmptyState()
    {
        booksPanel.Controls.Clear();

        Label empty = new Label
        {
            Text = "아직 등록된 책이 없습니다. 책 검색 화면에서 책을 추가해보세요.",
            Font = new Font("맑은 고딕", 11, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#888888"),
            Size = new Size(850, 40),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            Margin = new Padding(0, 80, 0, 0)
        };

        booksPanel.Controls.Add(empty);
    }

    private RoundedPanel CreateMyBookCard(MyBook book)
    {
        RoundedPanel card = new RoundedPanel
        {
            Size = new Size(430, 195),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 14,
            Margin = new Padding(0, 0, 25, 25)
        };

        RoundedPanel coverBox = new RoundedPanel
        {
            Size = new Size(70, 100),
            Location = new Point(22, 25),
            BackColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderRadius = 4
        };
        card.Controls.Add(coverBox);

        if (!string.IsNullOrWhiteSpace(book.Cover))
        {
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(70, 100),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ColorTranslator.FromHtml("#eeeeee")
            };
            coverBox.Controls.Add(pictureBox);
            pictureBox.LoadAsync(book.Cover);
        }
        else
        {
            Label lblCover = new Label
            {
                Text = "Book",
                Font = new Font("맑은 고딕", 8),
                ForeColor = ColorTranslator.FromHtml("#777777"),
                Size = new Size(70, 100),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#eeeeee")
            };
            coverBox.Controls.Add(lblCover);
        }

        Label lblTitle = new Label
        {
            Text = book.Title,
            Font = new Font("맑은 고딕", 12, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(280, 32),
            Location = new Point(112, 24),
            BackColor = Color.White
        };
        card.Controls.Add(lblTitle);

        Label lblAuthor = new Label
        {
            Text = book.Author,
            Font = new Font("맑은 고딕", 9),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(280, 24),
            Location = new Point(112, 58),
            BackColor = Color.White
        };
        card.Controls.Add(lblAuthor);

        Label lblPage = new Label
        {
            Text = $"현재 페이지: {book.CurrentPage} / {book.TotalPages}",
            Font = new Font("맑은 고딕", 9, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#436b55"),
            Size = new Size(280, 24),
            Location = new Point(112, 88),
            BackColor = Color.White
        };
        card.Controls.Add(lblPage);

        RoundedButton btnWrite = new RoundedButton
        {
            Text = "기록하기",
            Font = new Font("맑은 고딕", 9, FontStyle.Bold),
            Size = new Size(90, 34),
            Location = new Point(22, 145),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };
        btnWrite.Click += (s, e) =>
        {
            MemoWriteForm memoWriteForm = new MemoWriteForm(book);
            memoWriteForm.Show();
            Close();
        };
        card.Controls.Add(btnWrite);

        RoundedButton btnView = new RoundedButton
        {
            Text = "기록보기",
            Font = new Font("맑은 고딕", 9, FontStyle.Bold),
            Size = new Size(90, 34),
            Location = new Point(120, 145),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };

        btnView.Click += (s, e) =>
        {
            MemoViewForm memoViewForm = new MemoViewForm(book);
            memoViewForm.Show();
            Close();
        };

        card.Controls.Add(btnView);

        RoundedButton btnPage = new RoundedButton
        {
            Text = "페이지 수정",
            Font = new Font("맑은 고딕", 9, FontStyle.Bold),
            Size = new Size(100, 34),
            Location = new Point(218, 145),
            BackColor = ColorTranslator.FromHtml("#6f8877"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };
        btnPage.Click += async (s, e) =>
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "현재 읽고 있는 페이지를 입력하세요.",
                "페이지 수정",
                book.CurrentPage.ToString()
            );

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input, out int currentPage))
            {
                MessageBox.Show("숫자만 입력해주세요.");
                return;
            }

            if (currentPage < 0 || currentPage > book.TotalPages)
            {
                MessageBox.Show($"0부터 {book.TotalPages} 사이의 페이지를 입력해주세요.");
                return;
            }

            try
            {
                await ApiClient.UpdateCurrentPageAsync(book.BookId, currentPage);
                MessageBox.Show("현재 페이지가 수정되었습니다.");
                await LoadMyBooksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        };
        card.Controls.Add(btnPage);

        RoundedButton btnDelete = new RoundedButton
        {
            Text = "삭제",
            Font = new Font("맑은 고딕", 9, FontStyle.Bold),
            Size = new Size(70, 34),
            Location = new Point(328, 145),
            BackColor = ColorTranslator.FromHtml("#b45b5b"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };
        btnDelete.Click += async (s, e) =>
        {
            DialogResult result = MessageBox.Show(
                $"'{book.Title}'을 내 책장에서 삭제할까요?",
                "삭제 확인",
                MessageBoxButtons.YesNo
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                await ApiClient.DeleteMyBookAsync(book.BookId);
                MessageBox.Show("삭제되었습니다.");
                await LoadMyBooksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        };
        card.Controls.Add(btnDelete);

        return card;
    }

    private void LblBack_Click(object? sender, EventArgs e)
    {
        MainForm mainForm = new MainForm();
        mainForm.Show();
        Close();
    }

    private void MyBooksForm_Load(object sender, EventArgs e)
    {
    }

    private void MyBooksForm_Load_1(object sender, EventArgs e)
    {

    }
}