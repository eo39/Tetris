using System;
using System.Drawing;

namespace Tetris
{
    class Game
    {
        private readonly Random random;
        private readonly int cellSize;

        private const int FieldHeight = 20;
        private const int FieldWidth = 10;

        private int[,] fallingBlock;
        private int[,] gameField;

        public event Action Defeat = delegate { };

        public Game(int cellSize)
        {
            random = new Random();
            this.cellSize = cellSize;
            fallingBlock = new int[2, 4];
            gameField = new int[FieldWidth + 1, FieldHeight + 1];
        }

        public void StartGame()
        {
            GenerateNewBlock();
        }

        private void GenerateNewBlock()
        {
            switch (random.Next(7))
            {
                case 0:
                    fallingBlock = new[,] { { 0, 1, 2, 3 }, { 4, 4, 4, 4 } };
                    break;
                case 1:
                    fallingBlock = new[,] { { 0, 1, 0, 1 }, { 4, 4, 5, 5 } };
                    break;
                case 2:
                    fallingBlock = new[,] { { 0, 1, 2, 2 }, { 4, 4, 4, 5 } };
                    break;
                case 3:
                    fallingBlock = new[,] { { 0, 1, 2, 2 }, { 4, 4, 4, 3 } };
                    break;
                case 4:
                    fallingBlock = new[,] { { 1, 1, 2, 2 }, { 3, 4, 4, 5 } };
                    break;
                case 5:
                    fallingBlock = new[,] { { 1, 1, 2, 2 }, { 5, 4, 4, 3 } };
                    break;
                case 6:
                    fallingBlock = new[,] { { 1, 2, 2, 2 }, { 4, 3, 4, 5 } };
                    break;
            }
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
            for (int i = 0; i < 4; i++)
            {
                graphics.FillRectangle(Brushes.Red, fallingBlock[1, i] * cellSize, fallingBlock[0, i] * cellSize,
                    cellSize, cellSize);
            }
        }
    }
}
