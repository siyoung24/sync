using Sync.WinForms.Models;
using Sync.WinForms.Services;

namespace Sync.WinForms;

public partial class MyReviewsForm : Form
{
    private FlowLayoutPanel reviewPanel = null!;
    private ComboBox cmbSort = null!;
    private List<MyReview> myReviews = new List<MyReview>();

    public MyReviewsForm()
    {
        InitializeComponent();
        BuildMyReviewsUI();
    }

    private async void BuildMyReviewsUI()
    {
        Controls.Clear();

        AppLayout.SetupPage(this, "Sync - 내 작성글");
        AppLayout.AddSidebar(this, "reviews");

        Label lblTitle = new Label
        {
            Text = "내가 쓴 기록",
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
            Text = "작성한 한줄평을 책별로 모아보고, 이전 독서 흐름을 확인할 수 있어요.",
            Font = new Font("맑은 고딕", 10, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#777777"),
            Size = new Size(760, 30),
            Location = new Point(288, 112),
            TextAlign = ContentAlignment.TopLeft,
            BackColor = ColorTranslator.FromHtml("#f7f7f4")
        };
        Controls.Add(lblSub);

        

        cmbSort = new ComboBox
        {
            Font = new Font("맑은 고딕", 10),
            Size = new Size(170, 34),
            Location = new Point(970, 105),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        cmbSort.Items.Add("최신순");
        cmbSort.Items.Add("오래된순");
        cmbSort.SelectedIndex = 0;

        cmbSort.SelectedIndexChanged += CmbSort_SelectedIndexChanged;

        Controls.Add(cmbSort);
        cmbSort.BringToFront();

        reviewPanel = new FlowLayoutPanel
        {
            Size = new Size(880, 485),
            Location = new Point(285, 160),
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };
        Controls.Add(reviewPanel);

        await LoadMyReviewsAsync();
    }

    private async Task LoadMyReviewsAsync()
    {
        try
        {
            ShowMessageState("내 기록을 불러오는 중입니다...");

            myReviews = await ApiClient.GetMyReviewsAsync();

            if (myReviews.Count == 0)
            {
                ShowMessageState("아직 작성한 기록이 없습니다.");
                return;
            }

            RenderReviews();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            ShowMessageState("내 기록을 불러오지 못했습니다.");
        }
    }

    private void CmbSort_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (myReviews == null || myReviews.Count == 0)
            return;

        RenderReviews();
    }

    private void RenderReviews()
    {
        reviewPanel.Controls.Clear();

        List<MyReview> sortedReviews;

        if (cmbSort.SelectedIndex == 1)
        {
            // 오래된순: 예전에 쓴 기록부터
            sortedReviews = myReviews
                .OrderBy(r => ToLocalCreatedAt(r.CreatedAt))
                .ThenBy(r => r.Id)
                .ToList();
        }
        else
        {
            // 최신순: 최근에 쓴 기록부터
            sortedReviews = myReviews
                .OrderByDescending(r => ToLocalCreatedAt(r.CreatedAt))
                .ThenByDescending(r => r.Id)
                .ToList();
        }

        string currentBookTitle = "";

        foreach (MyReview review in sortedReviews)
        {
            if (currentBookTitle != review.BookTitle)
            {
                currentBookTitle = review.BookTitle;
                reviewPanel.Controls.Add(CreateBookHeader(currentBookTitle));
            }

            reviewPanel.Controls.Add(CreateReviewCard(review));
        }

        reviewPanel.PerformLayout();
    }

    private DateTime ToLocalCreatedAt(DateTime createdAt)
    {
        if (createdAt.Kind == DateTimeKind.Utc)
            return createdAt.ToLocalTime();

        if (createdAt.Kind == DateTimeKind.Local)
            return createdAt;

        return DateTime.SpecifyKind(createdAt, DateTimeKind.Utc).ToLocalTime();
    }

    private Label CreateBookHeader(string bookTitle)
    {
        return new Label
        {
            Text = bookTitle,
            Font = new Font("맑은 고딕", 14, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#1f1f1f"),
            Size = new Size(850, 38),
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = ColorTranslator.FromHtml("#f7f7f4"),
            Margin = new Padding(0, 10, 0, 5)
        };
    }

    private RoundedPanel CreateReviewCard(MyReview review)
    {
        RoundedPanel card = new RoundedPanel
        {
            Size = new Size(850, 105),
            BackColor = Color.White,
            BorderColor = ColorTranslator.FromHtml("#e6e6e6"),
            BorderRadius = 14,
            Margin = new Padding(0, 0, 0, 15)
        };

        RoundedPanel coverBox = new RoundedPanel
        {
            Size = new Size(50, 72),
            Location = new Point(25, 16),
            BackColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderColor = ColorTranslator.FromHtml("#eeeeee"),
            BorderRadius = 4
        };
        card.Controls.Add(coverBox);

        if (!string.IsNullOrWhiteSpace(review.Cover))
        {
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(50, 72),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ColorTranslator.FromHtml("#eeeeee")
            };
            coverBox.Controls.Add(pictureBox);
            pictureBox.LoadAsync(review.Cover);
        }
        else
        {
            Label lblCover = new Label
            {
                Text = "Book",
                Font = new Font("맑은 고딕", 8),
                ForeColor = ColorTranslator.FromHtml("#777777"),
                Size = new Size(50, 72),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#eeeeee")
            };
            coverBox.Controls.Add(lblCover);
        }

        Label lblPage = new Label
        {
            Text = $"p. {review.Page}",
            Font = new Font("맑은 고딕", 12, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#436b55"),
            Size = new Size(90, 28),
            Location = new Point(100, 18),
            BackColor = Color.White
        };
        card.Controls.Add(lblPage);

        Label lblContent = new Label
        {
            Text = review.Content,
            Font = new Font("맑은 고딕", 11, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#333333"),
            Size = new Size(560, 48),
            Location = new Point(100, 48),
            BackColor = Color.White
        };
        card.Controls.Add(lblContent);

        Label lblDate = new Label
        {
            Text = ToLocalCreatedAt(review.CreatedAt).ToString("yyyy-MM-dd HH:mm"),
            Font = new Font("맑은 고딕", 8, FontStyle.Regular),
            ForeColor = ColorTranslator.FromHtml("#999999"),
            Size = new Size(150, 24),
            Location = new Point(675, 40),
            TextAlign = ContentAlignment.MiddleRight,
            BackColor = Color.White
        };
        card.Controls.Add(lblDate);

        return card;
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
            Margin = new Padding(0, 120, 0, 0)
        };

        reviewPanel.Controls.Add(label);
    }

    private void LblBack_Click(object? sender, EventArgs e)
    {
        MainForm mainForm = new MainForm();
        mainForm.Show();
        Close();
    }

    private void MyReviewsForm_Load(object sender, EventArgs e)
    {
    }
}