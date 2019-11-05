using System;
using System.Drawing;

namespace Tetris
{
    class Game
    {
        private readonly Random random;
        private readonly int cellSize;

        private int fieldHeight;
        private int fieldWidth;

        public event Action Defeat = delegate { };

        public Game(int cellSize)
        {
            random = new Random();
            this.cellSize = cellSize;
        }

        public void StartGame()
        {
            fieldHeight = 20;
            fieldWidth = 10;
        }

        public void Draw(Graphics graphics)
        {
            DrawField(graphics);
        }

        private void DrawField(Graphics graphics)
        {
            for (int x = 0; x <= fieldWidth; x++)
                for (int y = 0; y <= fieldHeight; y++)
                {
                    graphics.DrawLine(Pens.Black, x * cellSize, y * cellSize, fieldWidth - x * cellSize, y * cellSize);
                    graphics.DrawLine(Pens.Black, x * cellSize, y * cellSize, x * cellSize, fieldHeight - y * cellSize);
                }
        }
    }
}
