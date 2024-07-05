namespace Minesweeper2
{
    public partial class Cell : PictureBox
    {
        public bool Mine { get; set; }
        public int Degree { get; set; } // 周囲の地雷の数を表すプロパティ
        private Minesweeper2 _mainForm; // MainFormへの参照

        public Cell(Minesweeper2 mainForm)
        {
            InitializeComponent();
            this.Click += Cell_Click;
            _mainForm = mainForm;
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

        private Bitmap GetImage() => CurrentMode switch
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

        private void Cell_Click(object? sender, EventArgs e)
        {
            // ゲームの状態がRunningでない場合は処理を中断
            if (_mainForm.CurrentState != Minesweeper2.State.Run)
            {
                return;
            }

            MouseEventArgs? mouseEvent = e as MouseEventArgs;

            string modeName = CurrentMode.ToString();
            if (modeName.Contains("Ans", StringComparison.Ordinal))
            {
                return;
            }

            if (modeName.Contains("Btn", StringComparison.Ordinal))
            {
                if (mouseEvent?.Button == MouseButtons.Left)
                {
                    // BtnFlagまたはBtnHoldのときは反応しない
                    if (CurrentMode == Mode.BtnFlag || CurrentMode == Mode.BtnHold)
                    {
                        return;
                    }

                    // 左クリック時の処理
                    RevealCells();
                }
                else if (mouseEvent?.Button == MouseButtons.Right)
                {
                    // 右クリック時の処理
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

        public void RevealCells()
        {
            if (Mine)
            {
                CurrentMode = Mode.AnsMineX; // 地雷を踏んだセルの位置をAnsMineXに設定
                _mainForm.EndGame(true);
            }
            else
            {
                CurrentMode = Degree switch
                {
                    0 => Mode.AnsBlank0,
                    1 => Mode.AnsBlank1,
                    2 => Mode.AnsBlank2,
                    3 => Mode.AnsBlank3,
                    4 => Mode.AnsBlank4,
                    5 => Mode.AnsBlank5,
                    6 => Mode.AnsBlank6,
                    7 => Mode.AnsBlank7,
                    8 => Mode.AnsBlank8,
                    _ => Mode.AnsBlank0
                };

                if (Degree == 0)
                {
                    _mainForm.RevealAdjacentCells(this);
                }

                _mainForm.CheckGameState();
            }
        }
    }
}