using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Block
    {
        private Random _rand = new Random();
        protected Shape _blockShape;

        public Shape BlockShape
        {
            get { return _blockShape; }
            set { _blockShape = value; }
        }

        public Block(int val)
        {
            _blockShape = RandomShape();
            FillBlock(val);
        }

        public Shape RandomShape()
        {
            Shape shape;
            int col = (_rand.Next()%4)+1;

            if (col == 1 || col == 4)
            {
                shape = new LineShape(col);

                return shape;
            }

            else if (col == 2)
            {
                shape = new SquareShape();

                return shape;
            }

            else
            {
                int fill_pos = _rand.Next() % 3;
                int col_fill = (_rand.Next() % 2) + 1;

                shape = new RectShape(col, fill_pos, col_fill);

                return shape;
            }            
        }

        public void FillBlock(int val)
        {
            for (int i = 0; i < _blockShape.Height; i++)
            {
                for (int j = 0; j < _blockShape.Width; j++)
                {
                    _blockShape.Dots[i, j] = _blockShape.Dots[i, j] == 1 ? val : 0;
                }
            }
        }    
    }

    public class GenerateBlocks
    {
        public static bool isSame = false;

        public static Block Generate()
        {
            Block block;
            Random _rand = new Random();
            int blockType;

            if (isSame)
            {
                blockType = 2;
            }

            else
            {
                blockType = (_rand.Next() % 2) + 1;
            }

            block = new Block(blockType);
            return block;
        }

        public static void UpdateScore(int val)
        {
            switch (val)
            {
                case 1:
                    Score.balanceScore.Score += Score.balanceScore.BonusScore;
                    break;

                case 2:
                    Score.creditScore.Score += 1;
                    break;
            }
        }

        public static Brush FillColor(int val)
        {
            switch (val)
            {
                case 1:
                    return Brushes.Red;
                case 2:
                    return Brushes.Blue;
                default:
                    return Brushes.LightGray;
            }
        }
    }
}
