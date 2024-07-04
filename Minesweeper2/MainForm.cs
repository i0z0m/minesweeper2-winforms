namespace Minesweeper2
{
    public partial class Minesweeper2 : Form
    {
        public enum State
        {
            initial,
            Running,
            Paused,
            GameOver,
            GameClear
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
                    Point cellPosition = new Point(col * cellSize, row * cellSize);

                    Cell cell = new Cell
                    {
                        Location = cellPosition,
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
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
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
                    // 左クリック時の処理
                    cell.CurrentMode = cell.Mine switch
                    {
                        true => Cell.Mode.AnsMine,
                        false => Cell.Mode.AnsBlank1
                    };
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
    }
}
