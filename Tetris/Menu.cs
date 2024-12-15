using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using Tetris; 

namespace Tetris
{
    public partial class Menu : Form
    {
        private System.Windows.Forms.Label BScoreLabel, CScoreLabel;
        //private int FinScore;
        private Button startGameButton;
        private Button exitButton;
        private int bScore, cScore;
        private Button shoppingButton;
        Nilai CurrNilai = new Nilai();

        public Menu()//
        {
            InitializeForm();//
            InitializeControls();//

        }

        private void InitializeForm()//
        {            
            this.Text = "Main Menu";//
            this.Size = new Size(400, 300);//
            this.StartPosition = FormStartPosition.CenterScreen;//
            bScore = 0;
            cScore = 0;
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
                Text = "Balance Score: " + bScore,
                Location = new Point(10, 10),
                Font = new Font("Arial", 10),
                ForeColor = Color.Black
            };
            this.Controls.Add(BScoreLabel);

            CScoreLabel = new System.Windows.Forms.Label
            {
                Text = "Credit Score: " + cScore,
                Location = new Point(10, 20),
                Font = new Font("Arial", 10),
                ForeColor = Color.Black
            };
            this.Controls.Add(CScoreLabel);

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
            using (ShoppingPanel shoppingPanel = new ShoppingPanel(bScore, cScore))
            {
                shoppingPanel.ShowDialog();
                bScore = shoppingPanel.getBScore();
                cScore = shoppingPanel.getCScore();

                // Update the final score after shopping
                BScoreLabel.Text = "Balance Score: " + bScore;
                CScoreLabel.Text = "Credit Score: " + cScore;
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
                BScoreLabel.Text = "Balance Score: " + Score.balanceScore.Score;
                CScoreLabel.Text = "Credit Score: " + Score.creditScore.Score;
            }
            this.Show();
        }

       
    }
}