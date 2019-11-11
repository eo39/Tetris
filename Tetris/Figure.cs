using System;

namespace Tetris
{
    class Figure
    {
        public int[,] Coordinates { get; }

        private static readonly Random random = new Random();
        private readonly FigureType figureType;
        private int rotateMode;

        private Figure(FigureType figureType, int rotateMode, int[,] coordinates)
        {
            this.figureType = figureType;
            this.rotateMode = rotateMode;
            Coordinates = coordinates;
        }

        public static Figure BuildRandomFigure()
        {
            switch (random.Next(8))
            {
                case 0: return new Figure(FigureType.T, 0, new[,] {{0, 1, 1, 1}, {4, 4, 3, 5}});
                case 1: return new Figure(FigureType.J, 0, new[,] {{0, 1, 2, 2}, {5, 5, 5, 4}});
                case 2: return new Figure(FigureType.L, 0, new[,] {{0, 1, 2, 2}, {4, 4, 4, 5}});
                case 3: return new Figure(FigureType.Z, 1, new[,] {{0, 1, 0, 1}, {3, 4, 4, 5}});
                case 4: return new Figure(FigureType.S, 1, new[,] {{0, 1, 0, 1}, {5, 4, 4, 3}});
                case 5: return new Figure(FigureType.I, 1, new[,] {{0, 0, 0, 0}, {5, 4, 6, 3}});
                case 6: return new Figure(FigureType.O, 0, new[,] {{0, 1, 1, 0}, {4, 4, 5, 5}});
                case 7: return new Figure(FigureType.Point, 0, new[,] {{0, 0, 0, 0}, {4, 4, 4, 4}});
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void ChangeCoordinates(int offsetX, int offsetY)
        {
            for (int i = 0; i < 4; i++)
            {
                Coordinates[0, i] += offsetY;
                Coordinates[1, i] += offsetX;
            }
        }

        public void RotateFigure(int[,] gameField, int gameFieldWidth, int gameFieldHeight)
        {
            int[,] fallingFigureTemp = new int[2, 4];
            Array.Copy(Coordinates, fallingFigureTemp, Coordinates.Length);

            switch (figureType)
            {
                case FigureType.T:
                    RotateCoordinates("Right");
                    break;
                case FigureType.J:
                    RotateCoordinates("Right");
                    break;
                case FigureType.L:
                    RotateCoordinates("Right");
                    break;
                case FigureType.Z:
                    RotateCoordinates();
                    break;
                case FigureType.S:
                    RotateCoordinates();
                    break;
                case FigureType.I:
                    RotateCoordinates();
                    break;
            }

            if (!GetFigureRotate(gameField, gameFieldWidth, gameFieldHeight))
                Array.Copy(fallingFigureTemp, Coordinates, Coordinates.Length);
        }

        private bool GetFigureRotate(int[,] gameField, int gameFieldWidth, int gameFieldHeight)
        {
            for (int i = 0; i < 4; i++)
                if (Coordinates[1, i] >= gameFieldWidth || Coordinates[1, i] < 0 ||
                    Coordinates[0, i] >= gameFieldHeight || Coordinates[0, i] < 0 ||
                    gameField[Coordinates[1, i], Coordinates[0, i]] == 1)
                {
                    return false;
                }

            return true;
        }

        private void RotateCoordinates(string direction)
        {
            switch (direction)
            {
                case "Right":
                {
                    for (int i = 0; i < 4; i++)
                    {
                        (Coordinates[1, i], Coordinates[0, i]) = (
                            Coordinates[1, 1] + Coordinates[0, 1] - Coordinates[0, i],
                            Coordinates[0, 1] - Coordinates[1, 1] + Coordinates[1, i]);
                    }

                    break;
                }

                case "Left":
                {
                    for (int i = 0; i < 4; i++)
                    {
                        (Coordinates[1, i], Coordinates[0, i]) = (
                            Coordinates[1, 1] - Coordinates[0, 1] + Coordinates[0, i],
                            Coordinates[1, 1] + Coordinates[0, 1] - Coordinates[1, i]);
                    }

                    break;
                }
            }
        }

        private void RotateCoordinates()
        {
            if (rotateMode == 1)
            {
                RotateCoordinates("Right");
                rotateMode = 2;
            }
            else
            {
                RotateCoordinates("Left");
                rotateMode = 1;
            }

        }
    }
}
