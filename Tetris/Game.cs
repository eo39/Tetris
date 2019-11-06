using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    class Game
    {
        private readonly Random random;
        private readonly int cellSize;

        private const int FieldHeight = 20;
        private const int FieldWidth = 10;

        private List<Point> fallingBlock;
        private int[,] gameField;

        public event Action Defeat = delegate { };

        public Game(int cellSize)
        {
            random = new Random();
            this.cellSize = cellSize;
            fallingBlock = new List<Point>();
            gameField = new int[FieldWidth + 1, FieldHeight + 1];
        }

        public void StartGame()
        {
            GenerateNewBlock();
        }

        private void GenerateNewBlock()
        {
            switch (random.Next(8))
            {
                case 0:
                    for (int i = 0; i < 4; i++)
                        fallingBlock.Add(new Point(i + 3, 0));
                    break;
                case 1:
                    for (int i = 0; i < 2; i++)
                    {
                        fallingBlock.Add(new Point(i + 4, i));
                        fallingBlock.Add(new Point(i + 5, i));
                    }
                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                        fallingBlock.Add(new Point(4, i));
                    fallingBlock.Add(new Point(5, 2));
                    break;
                case 3:
                    for (int i = 0; i < 3; i++)
                        fallingBlock.Add(new Point(5, i));
                    fallingBlock.Add(new Point(4, 2));
                    break;
                case 4:
                    for (int i = 0; i < 3; i++)
                        fallingBlock.Add(new Point(i + 4, 1));
                    fallingBlock.Add(new Point(5, 0));
                    break;
                case 5:
                    for (int i = 0; i < 2; i++)
                    {
                        fallingBlock.Add(new Point(i + 5, 0));
                        fallingBlock.Add(new Point(i + 4, 1));
                    }
                    break;
                case 6:
                    for (int i = 0; i < 2; i++)
                    {
                        fallingBlock.Add(new Point(4, i));
                        fallingBlock.Add(new Point(5, i));
                    }
                    break;
                case 7:
                    fallingBlock.Add(new Point(5, 0));
                    break;
            }
        }

        public void OffsettingFallingBlock(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.A:
                    if (GetCanFallingBlockMove())
                        for (int i = 0; i < fallingBlock.Count; i++)
                            fallingBlock[i] = new Point(fallingBlock[i].X - 1, fallingBlock[i].Y);
                    break;
                case Keys.D:
                    if (GetCanFallingBlockMove())
                        for (int i = 0; i < fallingBlock.Count; i++)
                            fallingBlock[i] = new Point(fallingBlock[i].X + 1, fallingBlock[i].Y);
                    break;
            }
        }

        private bool GetCanFallingBlockMove()
        {
            for (int i = 0; i < fallingBlock.Count; i++)
                if (fallingBlock[i].X + 1 <= FieldHeight && fallingBlock[i].X - 1 >= 0)
                {
                    return true;
                }

            return false;
        }

        public void Draw(Graphics graphics)
        {
            DrawField(graphics);
            DrawFallingBlock(graphics);
        }

        private void DrawField(Graphics graphics)
        {
            for (int x = 0; x <= FieldWidth; x++)
                for (int y = 0; y <= FieldHeight; y++)
                {
                    graphics.DrawLine(Pens.Black, x * cellSize, y * cellSize, FieldWidth - x * cellSize, y * cellSize);
                    graphics.DrawLine(Pens.Black, x * cellSize, y * cellSize, x * cellSize, FieldHeight - y * cellSize);

                    if (gameField[x, y] == 1)
                        graphics.FillRectangle(Brushes.Blue, x * cellSize, y * cellSize, cellSize, cellSize);
                }
        }

        private void DrawFallingBlock(Graphics graphics)
        {
            for (int i = 0; i < fallingBlock.Count; i++)
            {
                graphics.FillRectangle(Brushes.Red, fallingBlock[i].X * cellSize, fallingBlock[i].Y * cellSize,
                    cellSize, cellSize);
            }
        }
    }
}
