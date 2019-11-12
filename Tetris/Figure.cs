using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tetris
{
    internal class Figure
    {
        public Point[] Cells { get; private set; }

        private static readonly Random Random = new Random();
        private readonly FigureType figureType;
        private int rotateMode;

        private Figure(FigureType figureType, int rotateMode, Point[] cells)
        {
            this.figureType = figureType;
            this.rotateMode = rotateMode;
            Cells = cells;
        }

        public static Figure BuildRandomFigure()
        {
            switch (Random.Next(8))
            {
                case 0:
                    return new Figure(FigureType.T, 1,
                        new[] {new Point(5, 0), new Point(5, 1),new Point(5, 2), new Point(4, 2)});
                case 1:
                    return new Figure(FigureType.J, 1,
                        new[] { new Point(4, 0), new Point(4, 1), new Point(3, 1), new Point(5, 1) });
                case 2:
                    return new Figure(FigureType.L, 1,
                        new[] { new Point(4, 0), new Point(4, 1), new Point(4, 2), new Point(5, 2) });
                case 3:
                    return new Figure(FigureType.Z, 1,
                        new[] { new Point(3, 0), new Point(4, 1), new Point(4, 0), new Point(5, 1) });
                case 4:
                    return new Figure(FigureType.S, 1,
                        new[] { new Point(5, 0), new Point(4, 1), new Point(4, 0), new Point(3, 1) });
                case 5:
                    return new Figure(FigureType.I, 1,
                        new[] { new Point(5, 0), new Point(4, 0), new Point(6, 0), new Point(3, 0) });
                case 6:
                    return new Figure(FigureType.O, 1,
                        new[] { new Point(4, 0), new Point(4, 1), new Point(5, 1), new Point(5, 0) });
                case 7:
                    return new Figure(FigureType.Point, 1,
                        new[] { new Point(4, 0), new Point(4, 0), new Point(4, 0), new Point(4, 0) });
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
            Point[] currentFigureTemp = new Point[4];
            Array.Copy(Cells, currentFigureTemp, Cells.Length);

            switch (figureType)
            {
                case FigureType.T:
                case FigureType.J:
                case FigureType.L:
                    Cells = GetRotatedCoordinates();
                    break;

                case FigureType.Z:
                case FigureType.S:
                case FigureType.I:
                    Cells = GetRotatedCoordinates();
                    rotateMode = rotateMode == 1 ? -1 : 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!CanFigureRotate(Cells, gameField, gameFieldWidth, gameFieldHeight))
                Array.Copy(currentFigureTemp, Cells, Cells.Length);
        }

        private static bool CanFigureRotate(IEnumerable<Point> cells, bool[,] gameField, int gameFieldWidth, int gameFieldHeight)
        {
            return cells.All(point => point.X < gameFieldWidth &&
                                      point.X >= 0 && 
                                      point.Y < gameFieldHeight && 
                                      point.Y >= 0 && 
                                      !gameField[point.X, point.Y]);
        }

        private Point[] GetRotatedCoordinates()
        {
            Point[] coordinatesTemp = Cells;

            for (int i = 0; i < Cells.Length; i++)
                coordinatesTemp[i] = new Point(Cells[1].X + (Cells[1].Y - Cells[i].Y) * rotateMode,
                    coordinatesTemp[1].Y + (Cells[i].X - Cells[1].X) * rotateMode);

            return coordinatesTemp;
        }
    }
}
