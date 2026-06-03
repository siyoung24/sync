using Sync.WinForms.Models;
using Sync.WinForms.Services;

namespace Sync.WinForms;

public partial class MemoViewForm : Form
{
    private readonly MyBook selectedBook;

    private TextBox txtCurrentPage = null!;
    private FlowLayoutPanel reviewPanel = null!;

    public MemoViewForm(MyBook book)
    {
        InitializeComponent();
        selectedBook = book;
        BuildMemoViewUI();
    }

    private async void BuildMemoViewUI()
    {
        Controls.Clear();

        AppLayout.SetupPage(this, "Sync - 기록보기");
        AppLayout.AddSidebar(this, "view");

        Label lblTitle = new Label
        {
            Text = "기록보기",
            Font = new Font("맑은 고딕", 24, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(400, 55),
            Location = new Point(285, 45),
            TextAlign = ContentAlignment.TopLeft,
            BackColor = ColorTranslator.FromHtml("#f7f7f4")
        };
        Controls.Add(lblTitle);

        Label lblSub = new Label
        {
            Text = "현재 페이지를 기준으로, 가까운 이전 기록 5개를 확인할 수 있어요.",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(720, 30),
            Location = new Point(288, 112),
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
            Location = new Point(1035, 58),
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            Cursor = Cursors.Hand
        };
        lblBack.Click += LblBack_Click;
        Controls.Add(lblBack);

        RoundedPanel topCard = new RoundedPanel
        {
            Size = new Size(860, 150),
            Location = new Point(285, 160),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 16
        };
        Controls.Add(topCard);

        RoundedPanel coverBox = new RoundedPanel
        {
            Size = new Size(70, 100),
            Location = new Point(28, 25),
            BackColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderRadius = 4
        };
        topCard.Controls.Add(coverBox);

        if (!string.IsNullOrWhiteSpace(selectedBook.Cover))
        {
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(70, 100),
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
                Font = new Font("맑은 고딕", 8),
                ForeColor = ColorTranslator.FromHtml("#777777"),
                Size = new Size(70, 100),
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
            Size = new Size(450, 34),
            Location = new Point(120, 28),
            BackColor = Color.White
        };
        topCard.Controls.Add(lblBookTitle);

        Label lblAuthor = new Label
        {
            Text = selectedBook.Author,
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(450, 25),
            Location = new Point(120, 65),
            BackColor = Color.White
        };
        topCard.Controls.Add(lblAuthor);

        Label lblCurrent = new Label
        {
            Text = $"현재 저장된 페이지: {selectedBook.CurrentPage} / {selectedBook.TotalPages}",
            Font = new Font("맑은 고딕", 10, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#436b55"),
            Size = new Size(450, 25),
            Location = new Point(120, 96),
            BackColor = Color.White
        };
        topCard.Controls.Add(lblCurrent);

        Label lblPage = new Label
        {
            Text = "조회할 현재 페이지",
            Font = new Font("맑은 고딕", 10),
            ForeColor = ColorTranslator.FromHtml("#666666"),
            Size = new Size(160, 26),
            Location = new Point(600, 30),
            BackColor = Color.White
        };
        topCard.Controls.Add(lblPage);

        txtCurrentPage = new TextBox
        {
            Text = selectedBook.CurrentPage > 0 ? selectedBook.CurrentPage.ToString() : "",
            Font = new Font("맑은 고딕", 11),
            Size = new Size(120, 32),
            Location = new Point(600, 62)
        };
        topCard.Controls.Add(txtCurrentPage);

        Label lblTotal = new Label
        {
            Text = $"/ {selectedBook.TotalPages}",
            Font = new Font("맑은 고딕", 11, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(90, 32),
            Location = new Point(730, 62),
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.White
        };
        topCard.Controls.Add(lblTotal);

        RoundedButton btnLoad = new RoundedButton
        {
            Text = "기록 조회",
            Font = new Font("맑은 고딕", 10, FontStyle.Bold),
            Size = new Size(130, 40),
            Location = new Point(600, 100),
            BackColor = ColorTranslator.FromHtml("#436b55"),
            ForeColor = Color.White,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };
        btnLoad.Click += BtnLoad_Click;
        topCard.Controls.Add(btnLoad);

        reviewPanel = new FlowLayoutPanel
        {
            Size = new Size(880, 320),
            Location = new Point(285, 335),
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };
        Controls.Add(reviewPanel);

        if (selectedBook.CurrentPage > 0)
        {
            await LoadReviewsAsync(selectedBook.CurrentPage);
        }
        else
        {
            ShowMessageState("현재 페이지를 입력한 뒤 기록 조회 버튼을 눌러주세요.");
        }
    }

    private async void BtnLoad_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtCurrentPage.Text.Trim(), out int currentPage))
        {
            MessageBox.Show("현재 페이지는 숫자로 입력해주세요.");
            return;
        }

        if (currentPage < 1 || currentPage > selectedBook.TotalPages)
        {
            MessageBox.Show($"1부터 {selectedBook.TotalPages} 사이의 페이지를 입력해주세요.");
            return;
        }

        await LoadReviewsAsync(currentPage);
    }

    private async Task LoadReviewsAsync(int currentPage)
    {
        try
        {
            ShowMessageState("기록을 불러오는 중입니다...");

            List<Review> reviews = await ApiClient.GetNearbyReviewsAsync(
                selectedBook.BookId,
                currentPage,
                5
            );

            reviewPanel.Controls.Clear();

            if (reviews.Count == 0)
            {
                ShowMessageState("현재 페이지까지 작성된 기록이 없습니다.");
                return;
            }

            foreach (Review review in reviews)
            {
                reviewPanel.Controls.Add(CreateReviewCard(review, currentPage));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            ShowMessageState("기록을 불러오지 못했습니다.");
        }
    }

    private void ShowMessageState(string message)
    {
        reviewPanel.Controls.Clear();

        Label label = new Label
        {
            Text = message,
            Font = new Font("맑은 고딕", 11, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#888888"),
            Size = new Size(850, 45),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            Margin = new Padding(0, 60, 0, 0)
        };

        reviewPanel.Controls.Add(label);
    }

    private RoundedPanel CreateReviewCard(Review review, int currentPage)
    {
        int pageDiff = currentPage - review.Page;

        string distanceText = pageDiff == 0
            ? "현재 페이지 기록"
            : $"{pageDiff}페이지 전 기록";

        RoundedPanel card = new RoundedPanel
        {
            Size = new Size(850, 95),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 14,
            Margin = new Padding(0, 0, 0, 15)
        };

        Label lblPage = new Label
        {
            Text = $"p. {review.Page}",
            Font = new Font("맑은 고딕", 12, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#436b55"),
            Size = new Size(90, 28),
            Location = new Point(25, 20),
            BackColor = Color.White
        };
        card.Controls.Add(lblPage);

        Label lblDistance = new Label
        {
            Text = distanceText,
            Font = new Font("맑은 고딕", 9, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#999999"),
            Size = new Size(140, 24),
            Location = new Point(25, 50),
            BackColor = Color.White
        };
        card.Controls.Add(lblDistance);

        Label lblContent = new Label
        {
            Text = review.Content,
            Font = new Font("맑은 고딕", 11, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#333333"),
            Size = new Size(560, 48),
            Location = new Point(180, 25),
            BackColor = Color.White
        };
        card.Controls.Add(lblContent);

        Label lblDate = new Label
        {
            Text = review.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
            Font = new Font("맑은 고딕", 8, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#999999"),
            Size = new Size(130, 24),
            Location = new Point(705, 35),
            BackColor = Color.White
        };
        card.Controls.Add(lblDate);

        return card;
    }

    private void LblBack_Click(object? sender, EventArgs e)
    {
        MyBooksForm myBooksForm = new MyBooksForm();
        myBooksForm.Show();
        Close();
    }

    private void MemoViewForm_Load(object sender, EventArgs e)
    {
    }
}