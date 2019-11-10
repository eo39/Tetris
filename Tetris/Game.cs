using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    internal class Game
    {
        private readonly Random random;
        private readonly Font font = new Font("Arial", 15);
        private readonly int cellSize;

        private const int FieldHeight = 20;
        private const int FieldWidth = 10;

        private Block fallingBlock;
        private Block nextFallingBlock;
        private int[,] gameField;
        private int gameScore;

        public event Action Defeat = delegate { };

        public Game(int cellSize)
        {
            random = new Random();
            this.cellSize = cellSize;
        }

        public void StartGame()
        {
            gameField = new int[FieldWidth + 1, FieldHeight + 1];
            gameScore = 0;
            fallingBlock = GetNewBlock();
            nextFallingBlock = GetNewBlock();
        }

        private Block GetNewBlock()
        {
            switch (random.Next(8))
            {
                case 0:
                    return new Block("T");
                case 1:
                    return new Block("J");
                case 2:
                    return new Block("L");
                case 3:
                    return new Block("Z");
                case 4:
                    return new Block("S");
                case 5:
                    return new Block("I");
                case 6:
                    return new Block("O");
                case 7:
                    return new Block("Point");
                default:
                    return null;
            }
        }

        public void Update()
        {
            for (int i = 0; i < 4; i++)
                fallingBlock.Coordinates[0, i]++;

            if (!GetCanFallingBlockMove(0, 0))
            {
                for (int i = 0; i < 4; i++)
                    gameField[fallingBlock.Coordinates[1, i], fallingBlock.Coordinates[0, i] - 1] = 1;

                fallingBlock = nextFallingBlock;
                nextFallingBlock = GetNewBlock();
            }

            DeleteFullLines();

            if (IsDefeat())
                Defeat();
        }

        private void DeleteFullLines()
        {
            int deletedLinesCount = 0;

            for (int i = FieldHeight; i > 2; i--)
            {
                int cellsInLineCount = Enumerable.Range(0, gameField.GetLength(0))
                    .Select(j => gameField[j, i])
                    .Count(t => t == 1);

                if (cellsInLineCount != FieldWidth) continue;

                deletedLinesCount++;

                for (int k = i; k > 1; k--)
                    for (int l = 1; l < FieldWidth - 1; l++)
                    {
                        gameField[l, k] = gameField[l, k - 1];
                    }
            }

            AddScore(deletedLinesCount);
        }

        private bool IsDefeat()
        {
            for (int i = 0; i < 4; i++)
                if (gameField[fallingBlock.Coordinates[1, i], fallingBlock.Coordinates[0, i]] == 1)
                    return true;

            return false;
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
                    fallingBlock.RotateBlock(FieldWidth, FieldHeight);
                    break;

                case Keys.Left:
                    FallingBlockMove(-1, 0);
                    break;
                case Keys.Right:
                    FallingBlockMove(1, 0);
                    break;
                case Keys.Down:
                    FallingBlockMove(0, 1);
                    break;
                case Keys.Up:
                    fallingBlock.RotateBlock(FieldWidth, FieldHeight);
                    break;
            }
        }

        private void FallingBlockMove(int offsetX, int offsetY)
        {
            if (GetCanFallingBlockMove(offsetX, offsetY))
                fallingBlock.ChangeCoordinates(offsetX, offsetY);
        }

        private bool GetCanFallingBlockMove(int offsetX, int offsetY)
        {
            for (int i = 0; i < 4; i++)
                if (fallingBlock.Coordinates[1, i] + offsetX >= FieldWidth ||
                    fallingBlock.Coordinates[1, i] + offsetX < 0 ||
                    fallingBlock.Coordinates[0, i] + offsetY >= FieldHeight ||
                    fallingBlock.Coordinates[0, i] + offsetY < 0 ||
                    gameField[fallingBlock.Coordinates[1, i] + offsetX, fallingBlock.Coordinates[0, i] + offsetY] == 1)
                {
                    return false;
                }

            return true;
        }

        public void Draw(Graphics graphics)
        {
            DrawField(graphics);
            DrawFallingBlock(graphics);
            DrawInterface(graphics);
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

        private void DrawInterface(Graphics graphics)
        {
            for (int i = 0; i < 4; i++)
            {
                graphics.DrawRectangle(Pens.Black, nextFallingBlock.Coordinates[1, i] * cellSize + 210,
                    nextFallingBlock.Coordinates[0, i] * cellSize + 57,
                    cellSize, cellSize);
                graphics.FillRectangle(Brushes.Red, nextFallingBlock.Coordinates[1, i] * cellSize + 210,
                    nextFallingBlock.Coordinates[0, i] * cellSize + 57,
                    cellSize, cellSize);
            }

            graphics.DrawRectangle(Pens.Black, 290, 30, 109, 109);

            graphics.DrawString("Next block:", font, Brushes.Black, 290, 0);
            graphics.DrawString("Score: " + gameScore, font, Brushes.Black, 290, 150);
        }

        private void AddScore(int deletedLinesCount)
        {
            switch (deletedLinesCount)
            {
                case 1:
                    gameScore += 100;
                    break;
                case 2:
                    gameScore += 300;
                    break;
                case 3:
                    gameScore += 700;
                    break;
                case 4:
                    gameScore += 1500;
                    break;
            }
        }
    }
}