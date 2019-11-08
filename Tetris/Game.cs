using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
            gameField = new int[FieldWidth + 1, FieldHeight + 1];
            GenerateNewBlock();
        }

        private void GenerateNewBlock()
        {
            switch (random.Next(8))
            {
                case 0:
                    fallingBlock = new[,] {{0, 0, 0, 0}, {3, 4, 5, 2}};
                    break;
                case 1:
                    fallingBlock = new[,] {{0, 1, 1, 0}, {4, 4, 5, 5}};
                    break;
                case 2:
                    fallingBlock = new[,] {{0, 1, 2, 2}, {4, 4, 4, 5}};
                    break;
                case 3:
                    fallingBlock = new[,] {{0, 1, 2, 2}, {5, 5, 5, 4}};
                    break;
                case 4:
                    fallingBlock = new[,] {{0, 1, 0, 1}, {3, 4, 4, 5}};
                    break;
                case 5:
                    fallingBlock = new[,] {{0, 1, 0, 1}, {5, 4, 4, 3}};
                    break;
                case 6:
                    fallingBlock = new[,] {{0, 1, 1, 1}, {4, 4, 3, 5}};
                    break;
                case 7:
                    fallingBlock = new[,] {{0, 0, 0, 0}, {4, 4, 4, 4}};
                    break;
            }
        }

        public void Update()
        {
            for (int i = 0; i < 4; i++)
                fallingBlock[0, i]++;

            if (!GetCanFallingBlockMove(0,0))
            {
                for (int i = 0; i < 4; i++)
                    gameField[fallingBlock[1, i], --fallingBlock[0, i]] = 1;

                GenerateNewBlock();

                if (IsDefeat())
                    Defeat();
            }

            DeleteFullLines();
        }

        private bool IsDefeat()
        {
            for (int i = 0; i < 4; i++)
                if(gameField[fallingBlock[1, i], fallingBlock[0, i]] == 1)
                    return true;
            return false;
        }

        private void DeleteFullLines()
        {
            for (int i = FieldHeight - 1; i > 2; i--)
            {
                int lineCount = Enumerable
                    .Range(0, gameField.GetLength(0))
                    .Select(j => gameField[j, i])
                    .ToArray()
                    .Count(t => t == 1);

                if (lineCount == FieldWidth)
                    for (int k = i; k > 1; k--)
                        for (int l = 1; l < FieldWidth - 1; l++)
                            gameField[l, k] = gameField[l, k - 1];
            }
        }

        public void OffsettingFallingBlock(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.A:
                    FallingBlockMove(-1, 0);
                    break;
                case Keys.D:
                    FallingBlockMove(1, 0);
                    break;
                case Keys.S:
                    FallingBlockMove(0, 1);
                    break;
                case Keys.W:
                    FallingBlockRotate();
                    break;
            }
        }

        private void FallingBlockMove(int offsetX, int offsetY)
        {
            if (GetCanFallingBlockMove(offsetX, offsetY))
                for (int i = 0; i < 4; i++)
                {
                    fallingBlock[0, i] += offsetY;
                    fallingBlock[1, i] += offsetX;
                }
        }

        private void FallingBlockRotate()
        {
            var fallingBlockTemp = new int[2, 4];
            Array.Copy(fallingBlock, fallingBlockTemp, fallingBlock.Length);

            if (!IsFallingBlockSquare())
                for (int i = 0; i < 4; i++)
                {
                    (fallingBlock[1, i], fallingBlock[0, i]) = (
                        fallingBlock[1, 1] + fallingBlock[0, 1] - fallingBlock[0, i],
                        fallingBlock[0, 1] - fallingBlock[1, 1] + fallingBlock[1, i]);
                }

            if(!GetCanFallingBlockMove(0,0))
                Array.Copy(fallingBlockTemp, fallingBlock, fallingBlock.Length);
        }

        private bool IsFallingBlockSquare()
        {
            return fallingBlock[0, 0] == fallingBlock[0, 3] &&
                   fallingBlock[0, 1] == fallingBlock[0, 2] &&
                   fallingBlock[1, 0] == fallingBlock[1, 1] &&
                   fallingBlock[1, 2] == fallingBlock[1, 3];
        }

        private bool GetCanFallingBlockMove(int offsetX, int offsetY)
        {
            int cellsMoveCount = 0;
            for (int i = 0; i < 4; i++)
                if (fallingBlock[1, i] + offsetX < FieldWidth && fallingBlock[1, i] + offsetX >= 0 &&
                    fallingBlock[0, i] + offsetY < FieldHeight && fallingBlock[0, i] + offsetY >= 0 &&
                    gameField[fallingBlock[1, i] + offsetX, fallingBlock[0, i] + offsetY] != 1)
                {
                    cellsMoveCount++;
                }

            return cellsMoveCount >= 4;
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