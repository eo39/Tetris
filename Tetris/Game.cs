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

        private Figure fallingBlock;
        private int[,] gameField;

        public event Action Defeat = delegate { };

        public Game(int cellSize)
        {
            random = new Random();
            this.cellSize = cellSize;
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
                    fallingBlock = new Figure("T");
                    break;
                case 1:
                    fallingBlock = new Figure("J");
                    break;
                case 2:
                    fallingBlock = new Figure("L");
                    break;
                case 3:
                    fallingBlock = new Figure("Z");
                    break;
                case 4:
                    fallingBlock = new Figure("S");
                    break;
                case 5:
                    fallingBlock = new Figure("I");
                    break;
                case 6:
                    fallingBlock = new Figure("O");
                    break;
                case 7:
                    fallingBlock = new Figure("Point");
                    break;
            }
        }

        public void Update()
        {
            for (int i = 0; i < 4; i++)
                fallingBlock.Coordinates[0, i]++;

            if (!GetCanFallingBlockMove(0, 0))
            {
                for (int i = 0; i < 4; i++)
                    gameField[fallingBlock.Coordinates[1, i], --fallingBlock.Coordinates[0, i]] = 1;

                GenerateNewBlock();
            }

            DeleteFullLines();

            if (IsDefeat())
                Defeat();
        }

        private bool IsDefeat()
        {
            for (int i = 0; i < 4; i++)
                if (gameField[fallingBlock.Coordinates[1, i], fallingBlock.Coordinates[0, i]] == 1)
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
                    fallingBlock.Coordinates[0, i] += offsetY;
                    fallingBlock.Coordinates[1, i] += offsetX;
                }
        }

        private void FallingBlockRotate()
        {
            var fallingBlockTemp = new int[2, 4];
            Array.Copy(fallingBlock.Coordinates, fallingBlockTemp, fallingBlock.Coordinates.Length);

            fallingBlock.RotateFigure();

            if (!GetCanFallingBlockMove(0, 0))
                Array.Copy(fallingBlockTemp, fallingBlock.Coordinates, fallingBlock.Coordinates.Length);
        }


        private bool GetCanFallingBlockMove(int offsetX, int offsetY)
        {
            int cellsMoveCount = 0;
            for (int i = 0; i < 4; i++)
                if (fallingBlock.Coordinates[1, i] + offsetX < FieldWidth &&
                    fallingBlock.Coordinates[1, i] + offsetX >= 0 &&
                    fallingBlock.Coordinates[0, i] + offsetY < FieldHeight &&
                    fallingBlock.Coordinates[0, i] + offsetY >= 0 &&
                    gameField[fallingBlock.Coordinates[1, i] + offsetX, fallingBlock.Coordinates[0, i] + offsetY] != 1)
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
                graphics.FillRectangle(Brushes.Red, fallingBlock.Coordinates[1, i] * cellSize,
                    fallingBlock.Coordinates[0, i] * cellSize,
                    cellSize, cellSize);
            }
        }
    }
}