using System;

namespace Tetris
{
    public abstract class Shape
    {
        public int Width;
        public int Height;
        public int[,] Dots;
        //private int[,] backupDots;

        public Shape(int columns, int rows)
        {
            Width = columns;
            Height = rows;
            Dots = new int[Height, Width];
        }
        public void turn(string move)
        {
            int[, ] backupDots = Dots;

            Dots = new int[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (move == "cw")
                    {
                        Dots[i, j] = backupDots[Height - 1 - j, i];
                    }

                    else if (move == "ccw")
                    {
                        Dots[i, j] = backupDots[j, Width - 1 - i];
                    }
                }
            }

            var temp = Width;
            Width = Height;
            Height = temp;
        }
        public abstract void FillShape();
    }

    public class LineShape : Shape
    {
        public LineShape(int columns) : base(columns, 5-columns)
        {
            FillShape();
        }
        public override void FillShape()
        {
            for (int i = 0; i < Height; i++)
            {
                for(int j = 0;j < Width; j++)
                {
                    Dots[i, j] = 1;
                }
            }
        }
    }

    public class RectShape : Shape
    {
        private readonly int _fill_pos, _col_fill;
        public RectShape(int columns, int fill_pos, int col_fill) : base(columns, 5-columns)
        {
            _fill_pos = fill_pos;
            _col_fill = col_fill;
            FillShape();
        }
        public override void FillShape()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if(i == 0 && j != _fill_pos)
                    {
                        Dots[i, j] = 0;
                        continue;
                    }
                    Dots[i, j] = 1;
                }
            }

            if(_col_fill == 2)
            {
                if (_fill_pos == 0)
                {
                    Dots[0, 1] = 1;
                    Dots[1, 0] = 0;
                }

                else
                {
                    Dots[0, 1] = 1;
                    Dots[0, 2] = 1;
                    Dots[1, 2] = 0;
                }
            }
        }
    }

    public class SquareShape : Shape
    {
        public SquareShape() : base(2, 2)
        {
            FillShape();
        }

        public override void FillShape()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Dots[i, j] = 1;
                }
            }
        }
    }


}
