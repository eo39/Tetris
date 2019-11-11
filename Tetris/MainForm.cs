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
                Timer.Enabled = false;
                MessageBox.Show("Game over");
                game.StartGame();
                Timer.Enabled = true;
            };

            game.StartGame();
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            game.Draw(e.Graphics);
        }

        private void TickTimer(object sender, System.EventArgs e)
        {
            game.Update();
            gameField.Refresh();
        }

        private void DownKey(object sender, KeyEventArgs e)
        {
            game.OffsetCurrentFigure(e.KeyCode);
            gameField.Refresh();
        }
    }
}
