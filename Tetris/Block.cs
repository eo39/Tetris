using System;

namespace Tetris
{
    class Block
    {
        public int[,] Coordinates { get; }

        private readonly string blockType;
        private int rotateMode;

        public Block(string blockType)
        {
            this.blockType = blockType;

            switch (this.blockType)
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

        public void ChangeCoordinates(int offsetX, int offsetY)
        {
            for (int i = 0; i < 4; i++)
            {
                Coordinates[0, i] += offsetY;
                Coordinates[1, i] += offsetX;
            }
        }

        public void RotateBlock(int gameFieldWidth, int gameFieldHeight)
        {
            int[,] fallingBlockTemp = new int[2, 4];
            Array.Copy(Coordinates, fallingBlockTemp, Coordinates.Length);

            switch (blockType)
            {
                case "T":
                    RotateCoordinates("Right");
                    break;
                case "L":
                    RotateCoordinates("Right");
                    break;
                case "J":
                    RotateCoordinates("Right");
                    break;
                case "Z":
                    RotateCoordinates();
                    break;
                case "S":
                    RotateCoordinates();
                    break;
                case "I":
                    RotateCoordinates();
                    break;
            }

            if (!GetCanBlockRotate(gameFieldWidth, gameFieldHeight))
                Array.Copy(fallingBlockTemp, Coordinates, Coordinates.Length);
        }

        private bool GetCanBlockRotate(int gameFieldWidth, int gameFieldHeight)
        {
            int cellsMoveCount = 0;
            for (int i = 0; i < 4; i++)
                if (Coordinates[1, i] < gameFieldWidth && Coordinates[1, i] >= 0 &&
                    Coordinates[0, i] < gameFieldHeight && Coordinates[0, i] >= 0)
                {
                    cellsMoveCount++;
                }

            return cellsMoveCount >= 4;
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
