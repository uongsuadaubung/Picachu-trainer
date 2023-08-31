namespace Picachu

{
    public partial class Form1 : Form
    {
        private readonly MyMemory _memory;
        private int _baseAddr;

        private const int Rows = 11;
        private const int Cols = 18;
        private readonly Item[,] _map = new Item[Rows, Cols];
        private const int WmLbuttondown = 0x0201;
        private const int WmRbuttondown = 0x0204;

        private readonly MyPoint[] _directions =
        {
            new() { Row = -1, Col = 0 }, // di chuyển lên
            new() { Row = 1, Col = 0 }, // di chuyển xuống
            new() { Row = 0, Col = -1 }, // di chuyển trái
            new() { Row = 0, Col = 1 }, // di chuyển phải
        };

        public Form1()
        {
            InitializeComponent();
            _memory = new MyMemory("pikachu");
            if (!_memory.IsOk())
            {
                MessageBox.Show("Bật game lên trước");
                this.Close();
            }
            else Init();
        }

        private void Init()
        {
            _baseAddr = _memory.GetBaseAddress();
        }

        private void ResetTime()
        {
            int addr = _baseAddr + 0xB6084;

            _memory.WriteNumber(addr, 0);
        }

        private void FreezeTime()
        {
            int addr = _baseAddr + 0xB6084;

            float value = _memory.ReadFloat(addr);

            while (checkBoxFreezeTime.Checked)
            {
                _memory.WriteNumber(addr, value);
                Thread.Sleep(500);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetTime();
        }

        private void checkBoxFreezeTime_CheckedChanged(object sender, EventArgs e)
        {
            if (!_memory.IsOk())
            {
                checkBoxFreezeTime.Checked = false;
                return;
            }

            if (checkBoxFreezeTime.Checked)
            {
                Thread threadFreezeTime = new Thread(FreezeTime);
                threadFreezeTime.Start();
                btnResetTime.Enabled = false;
            }
            else
            {
                btnResetTime.Enabled = true;
            }
        }


        private void UpdateMap()
        {
            int[] idFirstValueOffset = { 0x000B6044 + _baseAddr, 0x76 };
            int addrFirstId = _memory.GetAddressFromPointer(idFirstValueOffset);

            int addrFirstDisplay = addrFirstId - 0x4;

            int currentDisplay = addrFirstDisplay;
            int currentId = addrFirstId;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (i == 0 || j == 0 || i == Rows - 1 || j == Cols - 1)
                    {
                        _map[i, j] = new Item { Id = -1, Show = 0 };
                    }
                    else
                    {
                        int valueId = _memory.ReadUShort(currentId);
                        int valueDisplay = _memory.ReadUShort(currentDisplay);
                        _map[i, j] = new Item { Id = valueId, Show = valueDisplay };
                        //memory.WriteNumber(currentId, 25, 2);
                        currentDisplay += 0x6;
                        currentId += 0x6;
                    }
                }

                if (i is 0 or Rows - 1)
                {
                    continue;
                }

                currentDisplay = addrFirstDisplay + (0x6c * i);
                currentId = addrFirstId + (0x6c * i);
            }
        }

        private bool IsValidMove(int newRow, int newCol, MyPoint end)
        {
            if (newRow < 0 ||
                newRow >= Rows ||
                newCol < 0 ||
                newCol >= Cols)
            {
                return false;
            }

            if (_map[newRow, newCol].Id == -1 || _map[newRow, newCol].Show == 0)
            {
                return true;
            }
            else if (_map[newRow, newCol].Id != -1 && _map[newRow, newCol].Show > 0 && newCol == end.Col &&
                     newRow == end.Row)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Bfs thì lại dính vài case đường sai đánh dấu visited chặn đường đúng đến đích
        /// thành ra vứt
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private bool IsValidConnectBfs(MyPoint start, MyPoint end)
        {
            bool[,] visited = new bool[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    visited[i, j] = false;
                }
            }

            visited[start.Row, start.Col] = true;

            Queue<MyQueue> queues = new();
            queues.Enqueue(new MyQueue(start, 0, null));


            while (queues.Count > 0)
            {
                MyQueue queue = queues.Dequeue();
                if (queue.Redirect > 3)
                {
                    //visited[queue.Position.Row, queue.Position.Col] = false;
                    continue;
                }

                if (queue.Position.Col == end.Col && queue.Position.Row == end.Row)
                {
                    return true;
                }

                foreach (MyPoint direction in _directions)
                {
                    int newRow = queue.Position.Row + direction.Row;
                    int newCol = queue.Position.Col + direction.Col;
                    if (!IsValidMove(newRow, newCol, end) || visited[newRow, newCol]) continue;
                    visited[newRow, newCol] = true;

                    int newRedirect = queue.Redirect;
                    if (
                        queue.LastDirection == null ||
                        queue.LastDirection.Row != direction.Row ||
                        queue.LastDirection.Col != direction.Col
                    )
                    {
                        newRedirect++; // Nếu có chuyển hướng thì tăng biến redirect
                    }

                    queues.Enqueue(new MyQueue(new MyPoint { Row = newRow, Col = newCol }, newRedirect, direction));
                }
            }

            return false;
        }

        /// <summary>
        /// Dùng dfs tốt hơn bfs mặc dù có có thể chậm hơn, nhưng mà đúng :))
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private bool IsValidConnectDfs(MyPoint start, MyPoint end)
        {
            bool[,] visited = new bool[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    visited[i, j] = false;
                }
            }

            visited[start.Row, start.Col] = true;

            return Dfs(start, 0, null);

            bool Dfs(MyPoint position, int redirect, MyPoint? lastDirection)
            {
                if (redirect > 3)
                {
                    return false;
                }

                if (position.Col == end.Col && position.Row == end.Row)
                {
                    return true;
                }

                foreach (MyPoint direction in _directions)
                {
                    int newRow = position.Row + direction.Row;
                    int newCol = position.Col + direction.Col;
                    if (!IsValidMove(newRow, newCol, end) || visited[newRow, newCol]) continue;
                    visited[newRow, newCol] = true;

                    int newRedirect = redirect;
                    if (
                        lastDirection == null ||
                        lastDirection.Row != direction.Row ||
                        lastDirection.Col != direction.Col
                    )
                    {
                        newRedirect++; // Nếu có chuyển hướng thì tăng biến redirect
                    }

                    if (Dfs(new MyPoint { Row = newRow, Col = newCol }, newRedirect, direction))
                    {
                        return true;
                    }

                    visited[newRow, newCol] = false;
                }

                return false;
            }
        }

        private MyPoint[]? FindAPair()
        {
            UpdateMap();
            for (int i = 1; i <= Rows - 2; i++)
            {
                for (int j = 1; j <= Cols - 2; j++)
                {
                    if (_map[i, j].Show == 0)
                    {
                        continue;
                    }

                    MyPoint poin1 = new MyPoint { Row = i, Col = j };
                    for (int k = 1; k <= Rows - 2; k++)
                    {
                        for (int l = 1; l <= Cols - 2; l++)
                        {
                            if (i == k && j == l)
                            {
                                continue;
                            }

                            if (_map[k, l].Show == 0)
                            {
                                continue;
                            }

                            if (_map[i, j].Id != _map[k, l].Id)
                            {
                                continue;
                            }

                            MyPoint poin2 = new MyPoint { Row = k, Col = l };
                            //if (i == 2 && j == 11 && k == 4)
                            //{
                            //    Console.WriteLine("for debug");
                            //}
                            if (!IsValidConnectDfs(poin1, poin2)) continue;
                            MyPoint[] myPoints = { poin1, poin2 };
                            return myPoints;
                        }
                    }
                }
            }

            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyPoint[]? mypoints = FindAPair();

            if (mypoints != null)
            {
                MessageBox.Show("[" + mypoints[0].Row + "," + mypoints[0].Col + "] - [" + mypoints[1].Row + "," +
                                mypoints[1].Col + "]");
            }
        }

        private void AutoMatch(bool auto)
        {
            MyPoint[]? mypoints;
            do
            {
                mypoints = FindAPair();
                if (mypoints is not { Length: > 0 }) continue;
                _memory.ClickToCell(mypoints[0].Row, mypoints[0].Col, WmRbuttondown);
                _memory.ClickToCell(mypoints[0].Row, mypoints[0].Col, WmLbuttondown);
                Thread.Sleep(300);
                _memory.ClickToCell(mypoints[1].Row, mypoints[1].Col, WmLbuttondown);
                _map[mypoints[0].Row, mypoints[0].Col].Show = 0;
                _map[mypoints[1].Row, mypoints[1].Col].Show = 0;
                Thread.Sleep(300);
            } while (mypoints is { Length: > 0 } && auto);
        }

        private void FreezeLife(bool enable)
        {
            _memory.WriteNumber(_baseAddr + 0xB0FB8, enable ? 0x9090 : 0x4866, 2);
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            Thread thread = new(() => AutoMatch(true));
            thread.Start();

            btnAuto.Enabled = false;
            btnMatchOnePair.Enabled = false;
            btnSuggestAPair.Enabled = false;
            FreezeLife(true);
            thread.Join();
            btnAuto.Enabled = true;
            btnMatchOnePair.Enabled = true;
            btnSuggestAPair.Enabled = true;
            FreezeLife(false);
        }

        private void btnMatchOnePair_Click(object sender, EventArgs e)
        {
            AutoMatch(false);
        }
    }
}