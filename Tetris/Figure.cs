namespace Tetris
{
    class Figure
    {
        public readonly int[,] Coordinates;

        private readonly string figureName;
        private int rotateMode;

        public Figure(string figureName)
        {
            this.figureName = figureName;

            switch (this.figureName)
            {
                case "T":
                    Coordinates = new[,] {{0, 1, 1, 1}, {4, 4, 3, 5}};
                    break;
                case "L":
                    Coordinates = new[,] {{0, 1, 2, 2}, {4, 4, 4, 5}};
                    break;
                case "J":
                    Coordinates = new[,] {{0, 1, 2, 2}, {5, 5, 5, 4}};
                    break;
                case "Z":
                    Coordinates = new[,] {{0, 1, 0, 1}, {3, 4, 4, 5}};
                    rotateMode = 1;
                    break;
                case "S":
                    Coordinates = new[,] {{0, 1, 0, 1}, {5, 4, 4, 3}};
                    rotateMode = 1;
                    break;
                case "I":
                    Coordinates = new[,] {{0, 0, 0, 0}, {4, 3, 5, 2}};
                    rotateMode = 1;
                    break;
                case "O":
                    Coordinates = new[,] {{0, 1, 1, 0}, {4, 4, 5, 5}};
                    break;
                case "Point":
                    Coordinates = new[,] {{0, 0, 0, 0}, {4, 4, 4, 4}};
                    break;
            }
        }

        public void RotateFigure()
        {
            switch (figureName)
            {
                case "T":
                    ChangeCoordinatesTLJ("Right");
                    break;
                case "L":
                    ChangeCoordinatesTLJ("Right");
                    break;
                case "J":
                    ChangeCoordinatesTLJ("Right");
                    break;
                case "Z":
                    ChangeCoordinatesSZI();
                    break;
                case "S":
                    ChangeCoordinatesSZI();
                    break;
                case "I":
                    ChangeCoordinatesSZI();
                    break;
            }
        }

        private void ChangeCoordinatesTLJ(string direction)
        {
            switch (direction)
            {
                case "Right":
                    for (int i = 0; i < 4; i++)
                    {
                        (Coordinates[1, i], Coordinates[0, i]) = (
                            Coordinates[1, 1] + Coordinates[0, 1] - Coordinates[0, i],
                            Coordinates[0, 1] - Coordinates[1, 1] + Coordinates[1, i]);
                    }

                    break;
                case "Left":
                    for (int i = 0; i < 4; i++)
                    {
                        (Coordinates[1, i], Coordinates[0, i]) = (
                            Coordinates[1, 1] - Coordinates[0, 1] + Coordinates[0, i],
                            Coordinates[1, 1] + Coordinates[0, 1] - Coordinates[1, i]);
                    }

                    break;
            }
        }

        private void ChangeCoordinatesSZI()
        {
            if (rotateMode == 1)
            {
                ChangeCoordinatesTLJ("Right");
                rotateMode = 2;
            }
            else
            {
                ChangeCoordinatesTLJ("Left");
                rotateMode = 1;
            }
        }
    }
}
