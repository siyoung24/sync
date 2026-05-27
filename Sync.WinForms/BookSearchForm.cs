using Sync.WinForms.Models;
using Sync.WinForms.Services;

namespace Sync.WinForms;

public partial class BookSearchForm : Form
{
    private RoundedTextBox txtKeyword = null!;
    private FlowLayoutPanel resultPanel = null!;

    public BookSearchForm()
    {
        InitializeComponent();
        BuildBookSearchUI();
    }

    private void BuildBookSearchUI()
    {
        Controls.Clear();

        AutoScaleMode = AutoScaleMode.None;

        Text = "Sync - 책 검색";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1050, 680);
        BackColor = ColorTranslator.FromHtml("#f7f7f4");
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        Label lblTitle = new Label
        {
            Text = "책 검색",
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
            Text = "읽고 있는 책을 검색하고 내 책장에 추가해보세요.",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(650, 30),
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

        RoundedPanel searchBox = new RoundedPanel
        {
            Size = new Size(900, 95),
            Location = new Point(60, 160),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 16
        };
        Controls.Add(searchBox);

        txtKeyword = new RoundedTextBox
        {
            Size = new Size(680, 48),
            Location = new Point(25, 24),
            BorderRadius = 10,
            BorderColor = ColorTranslator.FromHtml("#e2e2e2"),
            BackColor = Color.White,
            Font = new Font("맑은 고딕", 11)
        };
        searchBox.Controls.Add(txtKeyword);

        RoundedButton btnSearch = new RoundedButton
        {
            Text = "검색",
            Font = new Font("맑은 고딕", 11, FontStyle.Bold),
            Size = new Size(150, 48),
            Location = new Point(725, 24),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };
        btnSearch.Click += BtnSearch_Click;
        searchBox.Controls.Add(btnSearch);

        resultPanel = new FlowLayoutPanel
        {
            Size = new Size(920, 340),
            Location = new Point(60, 285),
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true
        };
        Controls.Add(resultPanel);

        ShowEmptyState();
    }

    private void ShowEmptyState()
    {
        resultPanel.Controls.Clear();

        Label empty = new Label
        {
            Text = "검색어를 입력한 뒤 검색 버튼을 눌러주세요.",
            Font = new Font("맑은 고딕", 11, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#888888"),
            Size = new Size(850, 40),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            Margin = new Padding(0, 80, 0, 0)
        };

        resultPanel.Controls.Add(empty);
    }

    private void ShowMessageState(string message)
    {
        resultPanel.Controls.Clear();

        Label label = new Label
        {
            Text = message,
            Font = new Font("맑은 고딕", 11, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#888888"),
            Size = new Size(850, 40),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            Margin = new Padding(0, 80, 0, 0)
        };

        resultPanel.Controls.Add(label);
    }

    private async void BtnSearch_Click(object? sender, EventArgs e)
    {
        string keyword = txtKeyword.Text.Trim();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            MessageBox.Show("검색어를 입력해주세요.");
            return;
        }

        RoundedButton? searchButton = sender as RoundedButton;

        try
        {
            if (searchButton != null)
            {
                searchButton.Enabled = false;
                searchButton.Text = "검색 중...";
            }

            ShowMessageState("책을 검색하는 중입니다...");

            List<BookSearchResult> books = await ApiClient.SearchBooksAsync(keyword);

            resultPanel.Controls.Clear();

            if (books.Count == 0)
            {
                ShowMessageState("검색 결과가 없습니다.");
                return;
            }

            foreach (BookSearchResult book in books)
            {
                resultPanel.Controls.Add(CreateBookCard(book));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            ShowEmptyState();
        }
        finally
        {
            if (searchButton != null)
            {
                searchButton.Enabled = true;
                searchButton.Text = "검색";
            }
        }
    }

    private RoundedPanel CreateBookCard(BookSearchResult book)
    {
        string title = string.IsNullOrWhiteSpace(book.Title) ? "제목 없음" : book.Title;
        string author = string.IsNullOrWhiteSpace(book.Author) ? "저자 정보 없음" : book.Author;
        string publisher = string.IsNullOrWhiteSpace(book.Publisher) ? "출판사 정보 없음" : book.Publisher;
        string isbn13 = book.Isbn13 ?? "";
        string coverUrl = book.Cover ?? "";

        RoundedPanel card = new RoundedPanel
        {
            Size = new Size(280, 185),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 14,
            Margin = new Padding(0, 0, 25, 25)
        };

        // 표지 영역
        RoundedPanel coverBox = new RoundedPanel
        {
            Size = new Size(58, 82),
            Location = new Point(20, 24),
            BackColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderRadius = 4
        };
        card.Controls.Add(coverBox);

        if (!string.IsNullOrWhiteSpace(coverUrl))
        {
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(58, 82),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ColorTranslator.FromHtml("#eeeeee")
            };

            coverBox.Controls.Add(pictureBox);

            try
            {
                pictureBox.LoadAsync(coverUrl);
            }
            catch
            {
                pictureBox.Dispose();

                Label lblCover = new Label
                {
                    Text = "Book",
                    Font = new Font("맑은 고딕", 8, FontStyle.Regular),
                    ForeColor = ColorTranslator.FromHtml("#777777"),
                    Size = new Size(58, 82),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = ColorTranslator.FromHtml("#eeeeee")
                };

                coverBox.Controls.Add(lblCover);
            }
        }
        else
        {
            Label lblCover = new Label
            {
                Text = "Book",
                Font = new Font("맑은 고딕", 8, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#777777"),
                Size = new Size(58, 82),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#eeeeee")
            };

            coverBox.Controls.Add(lblCover);
        }

        Label lblTitle = new Label
        {
            Text = title,
            Font = new Font("맑은 고딕", 10, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(170, 42),
            Location = new Point(92, 22),
            BackColor = Color.White
        };
        card.Controls.Add(lblTitle);

        Label lblAuthor = new Label
        {
            Text = author,
            Font = new Font("맑은 고딕", 9, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(170, 24),
            Location = new Point(92, 68),
            BackColor = Color.White
        };
        card.Controls.Add(lblAuthor);

        Label lblPublisher = new Label
        {
            Text = publisher,
            Font = new Font("맑은 고딕", 8, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#999999"),
            Size = new Size(170, 22),
            Location = new Point(92, 94),
            BackColor = Color.White
        };
        card.Controls.Add(lblPublisher);

        RoundedButton btnAdd = new RoundedButton
        {
            Text = "내 책장에 추가",
            Font = new Font("맑은 고딕", 9, FontStyle.Bold),
            Size = new Size(220, 36),
            Location = new Point(30, 130),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };

        btnAdd.Click += async (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(isbn13))
            {
                MessageBox.Show("ISBN 정보가 없어 책장에 추가할 수 없습니다.");
                return;
            }

            try
            {
                btnAdd.Enabled = false;
                btnAdd.Text = "확인 중...";

                List<MyBook> myBooks = await ApiClient.GetMyBooksAsync();

                if (myBooks.Count >= 3)
                {
                    MessageBox.Show("내 책장은 최대 3권까지만 등록할 수 있습니다.\n기존 책을 삭제한 뒤 다시 추가해주세요.");
                    return;
                }

                btnAdd.Text = "추가 중...";

                await ApiClient.AddMyBookAsync(isbn13);

                MessageBox.Show($"'{title}' 책을 내 책장에 추가했습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnAdd.Enabled = true;
                btnAdd.Text = "내 책장에 추가";
            }
        };

        card.Controls.Add(btnAdd);

        return card;
    }

    private void LblBack_Click(object? sender, EventArgs e)
    {
        MainForm mainForm = new MainForm();
        mainForm.Show();
        Close();
    }

    private void BookSearchForm_Load(object sender, EventArgs e)
    {
    }
}