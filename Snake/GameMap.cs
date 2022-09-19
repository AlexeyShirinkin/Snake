using System.Drawing;
using System.Collections.Generic;

namespace Snake
{
    class GameMap
    {
        private readonly Cell[,] map;
        public int SizeCell { get; }
        public int Width => (int)map.GetLongLength(0); 
        public int Height => (int)map.GetLongLength(1);
        public int WidthPixels => Width * SizeCell;
        public int HeightPixels => Height * SizeCell;

        public GameMap(int width, int height)
        {
            SizeCell = 30;
            map = new Cell[width, height];
        }

        public Cell this[int i, int j]
        {
            get => map[i, j];
            set => map[i, j] = value;
        }

        public Cell this[Point p]
        {
            get => map[p.X, p.Y];
            set => map[p.X, p.Y] = value;
        }

        public List<Point> GetEmptyPoints()
        {
            var emptyPoints = new List<Point>();
            for (var i = 0; i < Width; ++i)
                for (var j = 0; j < Height; ++j)
                    if (map[i, j] == Cell.Empty)
                        emptyPoints.Add(new Point(i, j));
            return emptyPoints;
        }

        public void ClearCells(Cell cell)
        {
            for (var i = 0; i < Width; ++i)
                for (var j = 0; j < Height; ++j)
                    if (map[i, j] == cell)
                        map[i, j] = Cell.Empty;
        }
    }
}