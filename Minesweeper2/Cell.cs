namespace Minesweeper2
{
    public partial class Cell : PictureBox
{
    public bool Mine { get; set; }

    public Cell()
    {
        InitializeComponent();
        this.Click += Cell_Click;
    }

    public enum Mode
    {
        AnsBlank0,
        AnsBlank1,
        AnsBlank2,
        AnsBlank3,
        AnsBlank4,
        AnsBlank5,
        AnsBlank6,
        AnsBlank7,
        AnsBlank8,
        AnsMine,
        AnsMineX,
        BtnBlank,
        BtnFlag,
        BtnFlagX,
        BtnHold,
        BtnHoldX
    }

    public Mode CurrentMode
    {
        get => _currentMode;
        set
        {
            _currentMode = value;
            this.Image = GetImage();
        }
    }
    private Mode _currentMode;

    public Image GetImage()
    {
        return CurrentMode switch
        {
            Mode.AnsBlank0 => Properties.Resources.ans_blank0,
            Mode.AnsBlank1 => Properties.Resources.ans_blank1,
            Mode.AnsBlank2 => Properties.Resources.ans_blank2,
            Mode.AnsBlank3 => Properties.Resources.ans_blank3,
            Mode.AnsBlank4 => Properties.Resources.ans_blank4,
            Mode.AnsBlank5 => Properties.Resources.ans_blank5,
            Mode.AnsBlank6 => Properties.Resources.ans_blank6,
            Mode.AnsBlank7 => Properties.Resources.ans_blank7,
            Mode.AnsBlank8 => Properties.Resources.ans_blank8,
            Mode.AnsMine => Properties.Resources.ans_mine,
            Mode.AnsMineX => Properties.Resources.ans_mineX,
            Mode.BtnBlank => Properties.Resources.btn_blank,
            Mode.BtnFlag => Properties.Resources.btn_flag,
            Mode.BtnFlagX => Properties.Resources.btn_flagX,
            Mode.BtnHold => Properties.Resources.btn_hold,
            Mode.BtnHoldX => Properties.Resources.btn_holdX,
            _ => throw new ArgumentOutOfRangeException(nameof(CurrentMode), $"Unsupported cell mode: {CurrentMode}")
        };
    }

    private void Cell_Click(object sender, EventArgs e)
    {
        // デリゲートを通じてクリックイベントを処理
        CellClickHandler?.Invoke(this, e);
    }

    // クリックイベントを処理するためのデリゲート
    public Action<object, EventArgs> CellClickHandler { get; set; }
}
}
