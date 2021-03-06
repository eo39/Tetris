﻿using System;
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

        private Figure currentFigure;
        private Figure nextFigure;
        private bool[,] gameField;
        private int gameScore;

        public event Action Defeat = delegate { };

        public Game(int cellSize)
        {
            this.cellSize = cellSize;
        }

        public void StartGame()
        {
            gameField = new bool[FieldWidth, FieldHeight];
            gameScore = 0;
            currentFigure = Figure.BuildRandomFigure();
            nextFigure = Figure.BuildRandomFigure();
        }


        public void Update()
        {
            MoveCurrentFigure(0, 1);
            DeleteFullLines();

            if (IsDefeat())
                Defeat();
        }

        private void DeleteFullLines()
        {
            int deletedLinesCount = 0;

            for (int i = 0; i < FieldHeight; i++)
            {
                int cellsInLineCount = Enumerable.Range(0, gameField.GetLength(0))
                    .Count(j => gameField[j, i]);

                if (cellsInLineCount != FieldWidth)
                    continue;

                deletedLinesCount++;

                for (int k = i; k > 1; k--)
                    for (int l = 0; l < FieldWidth; l++)
                    {
                        gameField[l, k] = gameField[l, k - 1];
                    }
            }

            gameScore += (int) (100 * (Math.Pow(2, deletedLinesCount) - 1));
        }

        private bool IsDefeat()
        {
            return currentFigure.Cells.Any(point => gameField[point.X, point.Y]);
        }

        public void OffsetCurrentFigure(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.A:
                case Keys.Left:
                    MoveCurrentFigure(-1, 0);
                    break;
                case Keys.D:
                case Keys.Right:
                    MoveCurrentFigure(1, 0);
                    break;
                case Keys.S:
                case Keys.Down:
                    MoveCurrentFigure(0, 1);
                    break;
                case Keys.W:
                case Keys.Up:
                    bool canCurrentFigureRotate = IsAcceptableCellsMove(currentFigure.GetRotatedCoordinates());

                    if (canCurrentFigureRotate)
                        currentFigure.RotateFigure();
                    break;
                case Keys.Space:
                    while (IsAcceptableCellsMove(currentFigure.GetOffsetCoordinates(0, 1)))
                    {
                        MoveCurrentFigure(0, 1);
                    }

                    Update();
                    break;
            }
        }

        private void MoveCurrentFigure(int offsetX, int offsetY)
        {
            if (IsAcceptableCellsMove(currentFigure.GetOffsetCoordinates(offsetX, offsetY)))
                currentFigure.ChangeCoordinates(offsetX, offsetY);
            else
            {
                if (offsetY != 0)
                {
                    foreach (var point in currentFigure.Cells)
                        gameField[point.X, point.Y] = true;

                    currentFigure = nextFigure;
                    nextFigure = Figure.BuildRandomFigure();
                }
            }
        }
        private bool IsAcceptableCellsMove(Point[] cells)
        {
            return cells.All(point => point.X < FieldWidth &&
                                      point.X >= 0 && 
                                      point.Y < FieldHeight && 
                                      point.Y >= 0 && 
                                      !gameField[point.X, point.Y]);
        }

        public void Draw(Graphics graphics)
        {
            DrawField(graphics);
            DrawCurrentFigure(graphics);
            DrawInterface(graphics);
        }

        private void DrawField(Graphics graphics)
        {
            for (int x = 0; x <= FieldWidth; x++)
                graphics.DrawLine(Pens.Black, x * cellSize, 0, x * cellSize, FieldHeight * cellSize);

            for (int y = 0; y <= FieldHeight; y++)
                graphics.DrawLine(Pens.Black, 0, y * cellSize, FieldWidth * cellSize, y * cellSize);

            for (int x = 0; x < FieldWidth; x++)
                for (int y = 0; y < FieldHeight; y++)
                {
                    if (gameField[x, y])
                        graphics.FillRectangle(Brushes.Blue, x * cellSize, y * cellSize, cellSize, cellSize);
                }
        }

        private void DrawCurrentFigure(Graphics graphics)
        {
            foreach (Point point in currentFigure.Cells)
                graphics.FillRectangle(Brushes.Red, point.X * cellSize, point.Y * cellSize, cellSize, cellSize);
        }

        private void DrawInterface(Graphics graphics)
        {
            foreach (Point point in nextFigure.Cells)
            {
                graphics.DrawRectangle(Pens.Black, point.X * cellSize + 210, point.Y * cellSize + 57, cellSize,
                    cellSize);
                graphics.FillRectangle(Brushes.Red, point.X * cellSize + 210, point.Y * cellSize + 57, cellSize,
                    cellSize);
            }

            graphics.DrawRectangle(Pens.Black, 290, 30, 109, 109);
            graphics.DrawString("Next figure:", font, Brushes.Black, 290, 0);
            graphics.DrawString("Score: " + gameScore, font, Brushes.Black, 290, 150);
        }
    }
}