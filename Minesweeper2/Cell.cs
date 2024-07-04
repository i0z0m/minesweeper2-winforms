namespace Minesweeper2
{
    public partial class Cell : PictureBox
    {
        public bool Mine { get; set; }
        public int Degree { get; set; } // 周囲の地雷の数を表すプロパティ
        public Point Position { get; set; } // 位置情報を持つプロパティ
        private Dictionary<Point, Cell> _cellList; // セルリストへの参照
        private GameState _gameState; // ゲームの状態への参照

        public Cell(Dictionary<Point, Cell> cellList, Point position, GameState gameState)
        {
            InitializeComponent();
            this.Click += Cell_Click;
            _cellList = cellList;
            Position = position;
            _gameState = gameState;
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

        public Image GetImage() => CurrentMode switch
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

        private void Cell_Click(object sender, EventArgs e)
        {
            // ゲームの状態がRunningでない場合は処理を中断
            if (_gameState.CurrentState != GameState.State.Running)
            {
                return;
            }

            MouseEventArgs mouseEvent = e as MouseEventArgs;

            string modeName = CurrentMode.ToString();
            if (modeName.Contains("Ans"))
            {
                return;
            }

            if (modeName.Contains("Btn"))
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
                _gameState.CurrentState = GameState.State.GameOver;
                // ゲームオーバー時にすべてのセルを明らかにする
                foreach (var cellEntry in _cellList.Values)
                {
                    if (cellEntry.CurrentMode == Mode.BtnBlank || cellEntry.CurrentMode == Mode.BtnFlag || cellEntry.CurrentMode == Mode.BtnHold)
                    {
                        if (cellEntry.Mine)
                        {
                            cellEntry.CurrentMode = Mode.AnsMine; // 地雷セルはAnsMineに設定
                        }
                        else
                        {
                            cellEntry.CurrentMode = cellEntry.CurrentMode switch
                            {
                                Mode.BtnFlag => Mode.BtnFlagX,
                                Mode.BtnHold => Mode.BtnHoldX,
                                _ => cellEntry.CurrentMode
                            };
                            if (cellEntry.CurrentMode == Mode.BtnBlank)
                            {
                                cellEntry.RevealCells();
                            }
                        }
                    }
                }
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
            }
        }
    }
}
