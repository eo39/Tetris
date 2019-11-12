using System;
using System.Drawing;
using System.Linq;

namespace Tetris
{
    internal class Figure
    {
        public Point[] Cells { get; private set; }

        private static readonly Random Random = new Random();
        private readonly FigureType figureType;
        private RotateDirection rotateDirection;

        private Figure(FigureType figureType, params Point[] cells)
        {
            this.figureType = figureType;
            Cells = cells;
            rotateDirection = RotateDirection.Right;
        }

        public static Figure BuildRandomFigure()
        {
            switch (Random.Next(8))
            {
                case 0:
                    return new Figure(FigureType.T, new Point(5, 0), new Point(5, 1), new Point(5, 2), new Point(4, 2));
                case 1:
                    return new Figure(FigureType.J, new Point(4, 0), new Point(4, 1), new Point(3, 1), new Point(5, 1));
                case 2:
                    return new Figure(FigureType.L, new Point(4, 0), new Point(4, 1), new Point(4, 2), new Point(5, 2));
                case 3:
                    return new Figure(FigureType.Z, new Point(3, 0), new Point(4, 1), new Point(4, 0), new Point(5, 1));
                case 4:
                    return new Figure(FigureType.S, new Point(5, 0), new Point(4, 1), new Point(4, 0), new Point(3, 1));
                case 5:
                    return new Figure(FigureType.I, new Point(5, 0), new Point(4, 0), new Point(6, 0), new Point(3, 0));
                case 6:
                    return new Figure(FigureType.O, new Point(4, 0), new Point(4, 1), new Point(5, 1), new Point(5, 0));
                case 7:
                    return new Figure(FigureType.Point, new Point(4, 0), new Point(4, 0), new Point(4, 0), new Point(4, 0));
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void ChangeCoordinates(int offsetX, int offsetY)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].X += offsetX;
                Cells[i].Y += offsetY;
            }
        }

        public void RotateFigure(bool[,] gameField, int gameFieldWidth, int gameFieldHeight)
        {
            Point[] rotatedCells = GetRotatedCoordinates(Cells);

            if (!IsAcceptableFigurePosition(rotatedCells, gameField, gameFieldWidth, gameFieldHeight)) return;

            switch (figureType)
            {
                case FigureType.T:
                case FigureType.J:
                case FigureType.L:
                    Cells = Cells = rotatedCells;
                    break;

                case FigureType.Z:
                case FigureType.S:
                case FigureType.I:
                    Cells = Cells = rotatedCells;
                    rotateDirection = rotateDirection == RotateDirection.Right ? RotateDirection.Left : RotateDirection.Right;
                    break;
            }
        }

        private bool IsAcceptableFigurePosition(Point[] cells, bool[,] gameField, int gameFieldWidth, int gameFieldHeight)
        {
            return cells.All(point => point.X < gameFieldWidth &&
                                      point.X >= 0 && 
                                      point.Y < gameFieldHeight && 
                                      point.Y >= 0 && 
                                      !gameField[point.X, point.Y]);
        }

        private Point[] GetRotatedCoordinates(Point[] cells)
        {
            return cells.Select(point => new Point(cells[1].X + (cells[1].Y - point.Y) * (int)rotateDirection,
                cells[1].Y + (point.X - cells[1].X) * (int)rotateDirection)).ToArray();

        }
    }
}
