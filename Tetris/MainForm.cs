using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainForm : Form
    {
        private const int CellSize = 27;
        private readonly Game game = new Game(CellSize);

        public MainForm()
        {
            InitializeComponent();

            gameField.Paint += Draw;

            game.Defeat += () =>
            {
                gameField.Refresh();
                MessageBox.Show("Game over");
                game.StartGame();
            };

            game.StartGame();
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            game.Draw(e.Graphics);
        }
    }
}
