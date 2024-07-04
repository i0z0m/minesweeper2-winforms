namespace Minesweeper2
{
    public partial class Cell : PictureBox
    {
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
            MouseEventArgs? mouseEvent = e as MouseEventArgs;

            string modeName = CurrentMode.ToString();
            if (modeName.Contains("Ans"))
            {
                return;
            }

            if (modeName.Contains("Btn"))
            {
                if (mouseEvent?.Button == MouseButtons.Left)
                {
                    // TODO ここに左クリック時の処理を記述
                    // 例: BtnBlank なら AnsBlank1 に変更（実際のロジックはアプリケーションのルールに依存）
                    CurrentMode = Mode.AnsBlank1; // 仮の処理
                }
                else if (mouseEvent?.Button == MouseButtons.Right)
                {
                    // ここに右クリック時の処理を記述
                    CurrentMode = CurrentMode switch
                    {
                        Mode.BtnBlank => Mode.BtnFlag,
                        Mode.BtnFlag => Mode.BtnHold,
                        Mode.BtnHold => Mode.BtnBlank,
                        _ => CurrentMode // それ以外の場合は変更しない
                    };
                }
            }
        }
    }
}
