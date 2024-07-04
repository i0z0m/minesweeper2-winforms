namespace Minesweeper2
{
    public partial class Minesweeper2 : Form
    {
        public enum State
        {
            NotStarted,
            Running,
            Paused,
            GameOver
        }

        // ゲームの状態を管理するプロパティ
        private State _state;
        public State CurrentState
        {
            get => _state;
            set
            {
                _state = value;
                // 状態が変わったときに必要な処理をここに追加
            }
        }

        private Dictionary<Point, Cell> _cellList = new Dictionary<Point, Cell>();

        public Minesweeper2()
        {
            InitializeComponent();
            InitializeCellList();
        }

        private void Minesweeper2_Load(object sender, EventArgs e)
        {
            FillMainPanel();
        }

        private void InitializeCellList()
        {
            int Rows = Properties.Settings.Default.NumberOfRows;
            int Colmns = Properties.Settings.Default.NumberOfColumns;

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Colmns; col++)
                {
                    int cellSize = 50;
                    Point cellPosition = new Point(col, row); // 修正: 座標をセルのインデックスとして管理

                    Cell cell = new Cell
                    {
                        Location = new Point(col * cellSize, row * cellSize), // 表示位置
                        Size = new Size(cellSize, cellSize),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        CurrentMode = Cell.Mode.BtnBlank,
                        CellClickHandler = Cell_Click // デリゲートを設定
                    };

                    _cellList.Add(cellPosition, cell);
                }
            }
        }

        private void FillMainPanel()
        {
            MainPanel.Controls.Clear();

            foreach (var cellEntry in _cellList)
            {
                Cell cell = cellEntry.Value;
                MainPanel.Controls.Add(cell);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            foreach (var cellEntry in _cellList)
            {
                Cell cell = cellEntry.Value;
                cell.CurrentMode = Cell.Mode.BtnBlank;
                cell.Degree = 0; // Degreeをリセット
            }
            PlaceMines();
            CurrentState = State.Running; // ゲームの状態をRunningに設定
        }

        private void PlaceMines()
        {
            foreach (var cell in _cellList.Values)
            {
                cell.Mine = false;
            }

            int mineCount = (int)(_cellList.Count * Properties.Settings.Default.PercentageOfMines / 100.0);
            Random random = new Random();

            var cellKeys = _cellList.Keys.OrderBy(_ => random.Next()).Take(mineCount);

            foreach (var minePosition in cellKeys)
            {
                _cellList[minePosition].Mine = true;
                UpdateCellDegree(minePosition);
            }
        }

        private void UpdateCellDegree(Point minePosition)
        {
            int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                Point neighborPosition = new Point(minePosition.X + dx[i], minePosition.Y + dy[i]);
                if (_cellList.TryGetValue(neighborPosition, out Cell? value))
                {
                    value.Degree++;
                }
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            // ゲームの状態がRunningでない場合は処理を中断
            if (CurrentState != State.Running)
            {
                return;
            }

            Cell cell = sender as Cell;
            if (cell == null) return;

            MouseEventArgs mouseEvent = e as MouseEventArgs;

            string modeName = cell.CurrentMode.ToString();
            if (modeName.Contains("Ans"))
            {
                return;
            }

            if (modeName.Contains("Btn"))
            {
                if (mouseEvent?.Button == MouseButtons.Left)
                {
                    // BtnFlagまたはBtnHoldのときは反応しない
                    if (cell.CurrentMode == Cell.Mode.BtnFlag || cell.CurrentMode == Cell.Mode.BtnHold)
                    {
                        return;
                    }

                    // 左クリック時の処理
                    RevealCells(cell);
                }
                else if (mouseEvent?.Button == MouseButtons.Right)
                {
                    // 右クリック時の処理
                    cell.CurrentMode = cell.CurrentMode switch
                    {
                        Cell.Mode.BtnBlank => Cell.Mode.BtnFlag,
                        Cell.Mode.BtnFlag => Cell.Mode.BtnHold,
                        Cell.Mode.BtnHold => Cell.Mode.BtnBlank,
                        _ => cell.CurrentMode // それ以外の場合は変更しない
                    };
                }
            }
        }

        private void RevealCells(Cell cell)
        {
            if (cell.Mine)
            {
                cell.CurrentMode = Cell.Mode.AnsMineX; // 地雷を踏んだセルの位置をAnsMineXに設定
                CurrentState = State.GameOver;
                // ゲームオーバー時にすべてのセルを明らかにする
                foreach (var cellEntry in _cellList.Values)
                {
                    if (cellEntry.CurrentMode == Cell.Mode.BtnBlank || cellEntry.CurrentMode == Cell.Mode.BtnFlag || cellEntry.CurrentMode == Cell.Mode.BtnHold)
                    {
                        if (cellEntry.Mine)
                        {
                            cellEntry.CurrentMode = Cell.Mode.AnsMine; // 地雷セルはAnsMineに設定
                        }
                        else
                        {
                            cellEntry.CurrentMode = cellEntry.CurrentMode switch
                            {
                                Cell.Mode.BtnFlag => Cell.Mode.BtnFlagX,
                                Cell.Mode.BtnHold => Cell.Mode.BtnHoldX,
                                _ => cellEntry.CurrentMode
                            };
                            if (cellEntry.CurrentMode == Cell.Mode.BtnBlank)
                            {
                                RevealCells(cellEntry);
                            }
                        }
                    }
                }
            }
            else
            {
                cell.CurrentMode = cell.Degree switch
                {
                    0 => Cell.Mode.AnsBlank0,
                    1 => Cell.Mode.AnsBlank1,
                    2 => Cell.Mode.AnsBlank2,
                    3 => Cell.Mode.AnsBlank3,
                    4 => Cell.Mode.AnsBlank4,
                    5 => Cell.Mode.AnsBlank5,
                    6 => Cell.Mode.AnsBlank6,
                    7 => Cell.Mode.AnsBlank7,
                    8 => Cell.Mode.AnsBlank8,
                    _ => Cell.Mode.AnsBlank0
                };
            }
        }
    }
}