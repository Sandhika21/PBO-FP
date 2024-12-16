using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using Tetris; 

namespace Tetris
{
    public partial class Menus : Form
    {
        private System.Windows.Forms.Label BScoreLabel, CScoreLabel;
        private System.Windows.Forms.Label amountBombLabel, LifeLabel;
        private Button startGameButton;
        private Button exitButton;
        private static int _bScore, _cScore;
        private static int _bomb, _extraLife;
        private Button shoppingButton;
        Nilai CurrNilai = new Nilai();

        public Menus()//
        {
            InitializeForm();//
            InitializeControls();//

        }

        public static int BScore
        {
            get { return _bScore; }
            set { _bScore = value; }
        }

        public static int CScore 
        { 
            get { return _cScore; } 
            set { _cScore = value; }
        }

        public static int Bomb
        {
            get { return _bomb; }
            set { _bomb = value; }
        }

        public static int ExtraLife
        {
            get { return _extraLife; }
            set { _extraLife = value; }
        }

        private void InitializeForm()//
        {            
            this.Text = "Main Menu";//
            this.Size = new Size(400, 300);//
            this.StartPosition = FormStartPosition.CenterScreen;//
            _bScore = 0;
            _cScore = 0;
            _bomb = 0;
        }
        private void InitializeControls()
        {
            // Tombol Start Game
            startGameButton = new Button
            {
                Text = "Start Game",
                Location = new Point(150, 50),
                Size = new Size(100, 30)
            };
            startGameButton.Click += StartGameButton_Click;
            this.Controls.Add(startGameButton);

            // Label Skor
            BScoreLabel = new System.Windows.Forms.Label
            {
                Text = "Balance Score: " + _bScore,
                Location = new Point(10, 10),
                Font = new Font("Arial", 10),
                ForeColor = Color.Black,
                AutoSize = true
            };
            this.Controls.Add(BScoreLabel);

            CScoreLabel = new System.Windows.Forms.Label
            {
                Text = "Credit Score: " + _cScore,
                Location = new Point(10, 40),
                Font = new Font("Arial", 10),
                ForeColor = Color.Black,
                AutoSize = true
            };
            this.Controls.Add(CScoreLabel);

            amountBombLabel = new System.Windows.Forms.Label
            {
                Text = $"Bomb: {_bomb}",
                Location = new Point(280, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true
            };
            this.Controls.Add(amountBombLabel);

            LifeLabel = new System.Windows.Forms.Label
            {
                Text = "Life: " + _extraLife,
                Location = new Point(10, 70),
                Font = new Font("Arial", 10),
                ForeColor = Color.Blue
            };
            this.Controls.Add(LifeLabel);

            // Tombol Shopping
            shoppingButton = new Button
            {
                Text = "Shopping",
                Location = new Point(150, 100),
                Size = new Size(100, 30)
            };
            shoppingButton.Click += ShoppingButton_Click;
            this.Controls.Add(shoppingButton);

            // Tombol Exit
            exitButton = new Button
            {
                Text = "Exit",
                Location = new Point(150, 150),
                Size = new Size(100, 30)
            };
            exitButton.Click += ExitButton_Click;
            this.Controls.Add(exitButton);
        }
        private void ShoppingButton_Click(object sender, EventArgs e)
        {
            using (ShoppingPanel shoppingPanel = new ShoppingPanel())
            {
                shoppingPanel.ShowDialog();

                // Update the final score after shopping
                BScoreLabel.Text = "Balance Score: " + _bScore;
                CScoreLabel.Text = "Credit Score: " + _cScore;
                amountBombLabel.Text = "Bomb: " + _bomb;
                LifeLabel.Text = "Life: " + _extraLife;
            }
        }
        private void StartGameButton_Click(object sender, EventArgs e)
        {
            TetrisGame gameForm = new TetrisGame();
            gameForm.FormClosed += GameForm_FormClosed;
            gameForm.Show();
            this.Hide();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TetrisGame gameForm = sender as TetrisGame;
            if (gameForm != null)
            {
                _bScore += Score.balanceScore.Score;
                _cScore += Score.creditScore.Score;
                BScoreLabel.Text = "Balance Score: " + _bScore;
                CScoreLabel.Text = "Credit Score: " + _cScore;
                amountBombLabel.Text = "Bomb: " + _bomb;
                LifeLabel.Text = "Life: " + _extraLife;
            }
            this.Show();
        }

       
    }
}