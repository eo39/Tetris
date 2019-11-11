using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    internal class Game
    {
        private readonly Font font = new Font("Arial", 15);
        private readonly int cellSize;

        private const int FieldHeight = 20;
        private const int FieldWidth = 10;

        private Figure fallingFigure;
        private Figure nextFallingFigure;
        private int[,] gameField;
        private int gameScore;

        public event Action Defeat = delegate { };

        public Game(int cellSize)
        {
            this.cellSize = cellSize;
        }

        public void StartGame()
        {
            gameField = new int[FieldWidth + 1, FieldHeight + 1];
            gameScore = 0;
            fallingFigure = Figure.BuildRandomFigure();
            nextFallingFigure = Figure.BuildRandomFigure();
        }


        public void Update()
        {
            for (int i = 0; i < 4; i++)
                fallingFigure.Coordinates[0, i]++;

            if (!GetFallingBlockMove(0, 0))
            {
                for (int i = 0; i < 4; i++)
                    gameField[fallingFigure.Coordinates[1, i], fallingFigure.Coordinates[0, i] - 1] = 1;

                fallingFigure = nextFallingFigure;
                nextFallingFigure = Figure.BuildRandomFigure();
            }

            DeleteFullLines();

            if (IsDefeat())
                Defeat();
        }

        private void DeleteFullLines()
        {
            int deletedLinesCount = 0;

            for (int i = 0; i <= FieldHeight; i++)
            {
                int cellsInLineCount = Enumerable.Range(0, gameField.GetLength(0))
                    .Count(j => gameField[j, i] == 1);

                if (cellsInLineCount != FieldWidth) continue;

                deletedLinesCount++;

                for (int k = i; k > 1; k--)
                    for (int l = 1; l < FieldWidth - 1; l++)
                    {
                        gameField[l, k] = gameField[l, k - 1];
                    }
            }

            gameScore += (int) (100 * (Math.Pow(2, deletedLinesCount) - 1));
        }

        private bool IsDefeat()
        {
            for (int i = 0; i < 4; i++)
                if (gameField[fallingFigure.Coordinates[1, i], fallingFigure.Coordinates[0, i]] == 1)
                    return true;

            return false;
        }

        public void MoveFallingBlock(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.A:
                case Keys.Left:
                    FallingBlockMove(-1, 0);
                    break;
                case Keys.D:
                case Keys.Right:
                    FallingBlockMove(1, 0);
                    break;
                case Keys.S:
                case Keys.Down:
                    FallingBlockMove(0, 1);
                    break;
                case Keys.W:
                case Keys.Up:
                    fallingFigure.RotateFigure(gameField, FieldWidth, FieldHeight);
                    break;
                case Keys.Space:
                    while (GetFallingBlockMove(0, 1))
                    {
                        FallingBlockMove(0, 1);
                    }
                    break;
            }
        }

        private void FallingBlockMove(int offsetX, int offsetY)
        {
            if (GetFallingBlockMove(offsetX, offsetY))
                fallingFigure.ChangeCoordinates(offsetX, offsetY);
        }

        private bool GetFallingBlockMove(int offsetX, int offsetY)
        {
            for (int i = 0; i < 4; i++)
                if (fallingFigure.Coordinates[1, i] + offsetX >= FieldWidth ||
                    fallingFigure.Coordinates[1, i] + offsetX < 0 ||
                    fallingFigure.Coordinates[0, i] + offsetY >= FieldHeight ||
                    fallingFigure.Coordinates[0, i] + offsetY < 0 ||
                    gameField[fallingFigure.Coordinates[1, i] + offsetX, fallingFigure.Coordinates[0, i] + offsetY] == 1)
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
                graphics.DrawLine(Pens.Black, x * cellSize, 0, x * cellSize, FieldHeight * cellSize);

            for (int y = 0; y <= FieldHeight; y++)
                graphics.DrawLine(Pens.Black, 0, y * cellSize, FieldWidth * cellSize, y * cellSize);

            for (int x = 0; x <= FieldWidth; x++)
                for (int y = 0; y <= FieldHeight; y++)
                {
                    if (gameField[x, y] == 1)
                        graphics.FillRectangle(Brushes.Blue, x * cellSize, y * cellSize, cellSize, cellSize);
                }
        }

        private void DrawFallingBlock(Graphics graphics)
        {
            for (int i = 0; i < 4; i++)
            {
                graphics.FillRectangle(Brushes.Red, fallingFigure.Coordinates[1, i] * cellSize,
                    fallingFigure.Coordinates[0, i] * cellSize,
                    cellSize, cellSize);
            }
        }

        private void DrawInterface(Graphics graphics)
        {
            for (int i = 0; i < 4; i++)
            {
                graphics.DrawRectangle(Pens.Black, nextFallingFigure.Coordinates[1, i] * cellSize + 210,
                    nextFallingFigure.Coordinates[0, i] * cellSize + 57,
                    cellSize, cellSize);
                graphics.FillRectangle(Brushes.Red, nextFallingFigure.Coordinates[1, i] * cellSize + 210,
                    nextFallingFigure.Coordinates[0, i] * cellSize + 57,
                    cellSize, cellSize);
            }

            graphics.DrawRectangle(Pens.Black, 290, 30, 109, 109);
            graphics.DrawString("Next block:", font, Brushes.Black, 290, 0);
            graphics.DrawString("Score: " + gameScore, font, Brushes.Black, 290, 150);
        }
    }
}