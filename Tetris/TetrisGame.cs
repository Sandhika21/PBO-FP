using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public class ShoppingPanel : Form
    {
        private Label BalanceScoreLabel;
        private Label CreditScoreLabel;

        // Buttons for purchasing items
        private Button buyExtraLifeButton;
        private Button buySpeedBoostButton;
        private Button buyBombButton;

        // Button to close the shopping panel
        private Button closeButton;

        // Constructor that accepts the player's current score
        public ShoppingPanel()
        {
            InitializeForm();
            InitializeControls();
        }

        // Initialize the form properties
        private void InitializeForm()
        {
            this.Text = "Shopping Panel";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        // Initialize all controls on the form
        private void InitializeControls()
        {
            // Label for Current Score
            BalanceScoreLabel = new Label
            {
                Text = $"Balance Score: {Menus.BScore}",
                Location = new Point(20, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true
            };
            this.Controls.Add(BalanceScoreLabel);

            // Label for Remaining Score after purchases
            CreditScoreLabel = new Label
            {
                Text = $"Credit Score: {Menus.CScore}",
                Location = new Point(20, 50),
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true
            };
            this.Controls.Add(CreditScoreLabel);

            // Separator Line
            Label separator = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Location = new Point(20, 80),
                Size = new Size(340, 2)
            };
            this.Controls.Add(separator);

            Item extraLife = new Item(40, 40, "Extra Life", new Point(50, 100), new Size(250, 40));
            Item speedBoost = new Item(20, 30, "Speed Boost", new Point(50, 160), new Size(250, 40));
            Item bomb = new Item(50, 60, "Bomb", new Point(50, 220), new Size(250, 40));

            extraLife.button.Click += (s, e) =>
            {
                if (AttemptPurchase(extraLife))
                {
                    Menus.ExtraLife++;
                }
            };
            this.Controls.Add(extraLife.button);


            speedBoost.button.Click += (s, e) =>
            {
                if (AttemptPurchase(speedBoost))
                {
                    Menus.Speed++;
                }
            };
            this.Controls.Add(speedBoost.button);

            bomb.button.Click += (s, e) =>
            {
                if (AttemptPurchase(bomb))
                {
                    Menus.Bomb++;
                }
            };
            this.Controls.Add(bomb.button);


            // Close Button
            closeButton = new Button
            {
                Text = "Close",
                Location = new Point(150, 280),
                Size = new Size(100, 30)
            };
            closeButton.Click += CloseButton_Click;
            this.Controls.Add(closeButton);
        }

        // Common method to handle purchases
        private bool AttemptPurchase(Item item)
        {
            if (Menus.BScore >= item.BPrice && Menus.CScore >= item.CPrice)
            {
                // Deduct the price from current score
                Menus.BScore -= item.BPrice;
                Menus.CScore -= item.CPrice;
                BalanceScoreLabel.Text = $"Balance Score: {Menus.BScore}";
                CreditScoreLabel.Text = $"Credit Score: {Menus.CScore}";

                // Notify the player of successful purchase
                MessageBox.Show($"{item.Name} purchased successfully!", "Purchase Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;
            }
            else
            {
                // Notify the player of insufficient funds
                MessageBox.Show($"Insufficient score to purchase {item.Name}.", "Purchase Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        // Event handler for closing the shopping panel
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Method to retrieve the remaining score after shopping
        /*public int GetRemainingScore()
        {
            return currentScore;
        }*/
    }

    public partial class TetrisGame : Form
    {
        private Button bombButton;
        private Label BombShow, LifeShow, LShow, SShow;
        private int _life, _interval;
        Block currentShape;
        Block nextShape;
        Timer timer = new Timer();
        Nilai nilai = new Nilai();
        public TetrisGame()
        {
            InitializeComponent();

            BombShow = new Label
            {
                Text = $"         Bomb: " + Menus.Bomb + "\nPress E to Activate",
                Location = new Point(335, 360),
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true
            };
            this.Controls.Add(BombShow);

            LifeShow = new Label
            {
                Text = $"         Extra Life: " + Menus.ExtraLife + "\nPress D to Activate",
                Location = new Point(335, 280),
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true
            };
            this.Controls.Add(LifeShow);

            _life = 0;

            LShow = new Label
            {
                Text = "Life: " + _life,
                Location = new Point(335, 330),
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true
            };
            this.Controls.Add(LShow);

            SShow = new Label
            {
                Text = $"         Speed Boost: " + Menus.Speed + "\nPress S to Activate",
                Location = new Point(335, 400),
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true
            };
            this.Controls.Add(SShow);
            _interval = 5;

            loadCanvas();
            Score.Reset();

            currentShape = getRandomShapeWithCenterAligned();
            nextShape = getNextShape();

            timer.Tick += Timer_Tick;
            timer.Interval = 500;
            timer.Start();

            this.KeyDown += Tetris_KeyDown;

            /*
            bombButton = new Button
            {
                Text = "Bomb" + Menus.Bomb,
                Location = new Point(150, 100),
                Size = new Size(100, 130)
            };
            bombButton.Click += (s, e) =>
            {
                this.Focus();
                BombEffect(s, e);
            };
            this.Controls.Add(bombButton);

            this.KeyPreview = true;
            */
        }

        Bitmap canvasBitmap;
        Graphics canvasGraphics;
        int canvasWidth = 15;
        int canvasHeight = 20;
        int[,] canvasDotArray;
        int dotSize = 20;
        private void loadCanvas()
        {
            // Resize the picture box based on the dotsize and canvas size
            pictureBox1.Width = canvasWidth * dotSize;
            pictureBox1.Height = canvasHeight * dotSize;

            // Create Bitmap with picture box's size
            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            canvasGraphics = Graphics.FromImage(canvasBitmap);

            canvasGraphics.FillRectangle(Brushes.LightGray, 0, 0, canvasBitmap.Width, canvasBitmap.Height);

            // Load bitmap into picture box
            pictureBox1.Image = canvasBitmap;

            // Initialize canvas dot array. by default all elements are zero
            canvasDotArray = new int[canvasWidth, canvasHeight];
        }

        int currentX;
        int currentY;
        private Block getRandomShapeWithCenterAligned()
        {
            var block = GenerateBlocks.Generate();

            // Calculate the x and y values as if the shape lies in the center
            currentX = 7;
            currentY = -block.BlockShape.Height;

            return block;
        }

        Bitmap workingBitmap;
        Graphics workingGraphics;
        private void Timer_Tick(object sender, EventArgs e)
        {
            var isMoveSuccess = moveShapeIfPossible(moveDown: 1);

            // if shape reached bottom or touched any other shapes
            if (!isMoveSuccess)
            {
                // copy working image into canvas image
                canvasBitmap = new Bitmap(workingBitmap);

                updateCanvasDotArrayWithCurrentShape();

                // get next shape
                currentShape = nextShape;
                nextShape = getNextShape();

                clearFilledRowsAndUpdateScore();
            }
        }

        private void updateCanvasDotArrayWithCurrentShape()
        {
            for (int i = 0; i < currentShape.BlockShape.Width; i++)
            {
                for (int j = 0; j < currentShape.BlockShape.Height; j++)
                {
                    if (currentShape.BlockShape.Dots[j, i] != 0)
                    {
                        // Calculate the positions to avoid IndexOutOfRangeException
                        int targetX = currentX + i;
                        int targetY = currentY + j;

                        // Check if the target position is within bounds
                        if (targetX >= 0 && targetX < canvasDotArray.GetLength(0) &&
                            targetY >= 0 && targetY < canvasDotArray.GetLength(1))
                        {
                            canvasDotArray[targetX, targetY] = currentShape.BlockShape.Dots[j, i];
                        }
                        checkIfGameOver();

                        // (Previous ver of Code) 
                        //DON'T  Remove => Just in case there is an ERROR
                        //checkIfGameOver();
                        //canvasDotArray[currentX + i, currentY + j] = 1;
                    }
                }
            }
        }

        private void checkIfGameOver()
        {
            if (currentY < 0) 
            {
                if (_life == 0)
                {
                    gameOver();
                }
                else
                {                    
                    for (int i = 0; i < canvasWidth; i++)
                    {
                        for (int j = 0; j < canvasHeight / 2; j++)
                        {
                            canvasDotArray[i, j] = 0;
                        }
                    }
                    canvasGraphics.FillRectangle(Brushes.LightGray, 0, 0, canvasBitmap.Width, canvasBitmap.Height / 2);
                    pictureBox1.Image = canvasBitmap;
                    _life--;
                    LShow.Text = "Life: " + _life;
                }
            }
        }

        private void gameOver()
        {
            timer.Stop();
            this.Close();
        }

        // returns if it reaches the bottom or touches any other blocks
        private bool moveShapeIfPossible(int moveDown = 0, int moveSide = 0)
        {
            var newX = currentX + moveSide;
            var newY = currentY + moveDown;

            // check if it reaches the bottom or side bar
            if (newX < 0 || newX + currentShape.BlockShape.Width > canvasWidth
                || newY + currentShape.BlockShape.Height > canvasHeight)
                return false;

            // check if it touches any other blocks 
            for (int i = 0; i < currentShape.BlockShape.Width; i++)
            {
                for (int j = 0; j < currentShape.BlockShape.Height; j++)
                {
                    if (newY + j > 0 && canvasDotArray[newX + i, newY + j] != 0 && currentShape.BlockShape.Dots[j, i] != 0)
                        return false;
                }
            }

            currentX = newX;
            currentY = newY;

            drawShape();

            return true;
        }

        private void drawShape()
        {
            workingBitmap = new Bitmap(canvasBitmap);
            workingGraphics = Graphics.FromImage(workingBitmap);

            for (int i = 0; i < currentShape.BlockShape.Width; i++)
            {
                for (int j = 0; j < currentShape.BlockShape.Height; j++)
                {
                    if (currentShape.BlockShape.Dots[j, i] != 0)
                        workingGraphics.FillRectangle(GenerateBlocks.FillColor(currentShape.BlockShape.Dots[j, i]), (currentX + i) * dotSize, (currentY + j) * dotSize, dotSize, dotSize);
                }
            }

            pictureBox1.Image = workingBitmap;
        }

        private void Tetris_KeyDown(object sender, KeyEventArgs e)
        {
            var verticalMove = 0;
            var horizontalMove = 0;

            // calculate the vertical and horizontal move values
            // based on the key pressed
            switch (e.KeyCode)
            {
                case Keys.E:
                    if(Menus.Bomb > 0)
                    {
                        Array.Clear(canvasDotArray, 0, canvasDotArray.Length);
                        canvasGraphics.FillRectangle(Brushes.LightGray, 0, 0, canvasWidth * dotSize, canvasHeight * dotSize);

                        pictureBox1.Image = canvasBitmap;
                        Menus.Bomb--;
                        Score.balanceScore.Score += 5;

                        BombShow.Text = $"         Bomb: " + Menus.Bomb + "\nPress E to Activate";
                        label1.Text = "Balance Score: " + Score.balanceScore.Score;                        
                    }
                    break;
                case Keys.D:
                    if(Menus.ExtraLife > 0)
                    {
                        Menus.ExtraLife--;
                        _life++;
                        LifeShow.Text = $"         Extra Life: " + Menus.ExtraLife + "\nPress D to Activate";
                        LShow.Text = "Life: " + _life;
                    }
                    break;
                case Keys.S:
                    if(Menus.Speed > 0)
                    {
                        _interval += 10;
                        Menus.Speed--;
                        SShow.Text = $"         Speed Boost: " + Menus.Speed + "\nPress S to Activate";
                    }
                    break;
                // move shape left
                case Keys.Left:
                    verticalMove--;
                    break;

                // move shape right
                case Keys.Right:
                    verticalMove++;
                    break;

                // move shape down faster
                case Keys.Down:
                    horizontalMove++;
                    break;

                // rotate the shape clockwise
                case Keys.Up:
                    currentShape.BlockShape.turn("cw");
                    break;
                default:
                    return;
            }

            var isMoveSuccess = moveShapeIfPossible(horizontalMove, verticalMove);

            // if the player was trying to rotate the shape, but
            // that move was not possible - rollback the shape
            if (!isMoveSuccess && e.KeyCode == Keys.Up)
                currentShape.BlockShape.turn("ccw");
        }


        public void clearFilledRowsAndUpdateScore()
        {
            // check through each rows
            for (int i = 0; i < canvasHeight; i++)
            {
                int j;
                for (j = canvasWidth - 1; j >= 0; j--)
                {
                    if (canvasDotArray[j, i] == 0)
                        break;
                }

                if (j == -1)
                {
                    // increase the speed 
                    timer.Interval -= _interval;

                    // update the dot array based on the check
                    for (j = 0; j < canvasWidth; j++)
                    {
                        GenerateBlocks.UpdateScore(canvasDotArray[j, i]);
                        for (int k = i; k > 0; k--)
                        {
                            canvasDotArray[j, k] = canvasDotArray[j, k - 1];
                        }

                        canvasDotArray[j, 0] = 0;
                    }
                    label1.Text = "Balance Score: " + Score.balanceScore.Score;
                    label2.Text = "Credit Score: " + Score.creditScore.Score;
                    Score.bonusPotential.UpdateScore(Score.balanceScore, Score.creditScore);
                    label3.Text = "Bonus Potential: " + Score.bonusPotential.Score;

                    Score.balanceScore.ApplyBuff(Score.creditScore);
                    Score.creditScore.ApplyBuff(Score.balanceScore);
                }
            }

            // Draw panel based on the updated array values
            for (int i = 0; i < canvasWidth; i++)
            {
                for (int j = 0; j < canvasHeight; j++)
                {
                    canvasGraphics = Graphics.FromImage(canvasBitmap);
                    canvasGraphics.FillRectangle(
                        GenerateBlocks.FillColor(canvasDotArray[i, j]),
                        i * dotSize, j * dotSize, dotSize, dotSize
                        );
                }
            }

            pictureBox1.Image = canvasBitmap;
        }

        Bitmap nextShapeBitmap;
        Graphics nextShapeGraphics;
        private Block getNextShape()
        {
            var shape = getRandomShapeWithCenterAligned();

            // Codes to show the next shape in the side panel
            nextShapeBitmap = new Bitmap(6 * dotSize, 6 * dotSize);
            nextShapeGraphics = Graphics.FromImage(nextShapeBitmap);

            nextShapeGraphics.FillRectangle(Brushes.LightGray, 0, 0, nextShapeBitmap.Width, nextShapeBitmap.Height);

            // Find the ideal position for the shape in the side panel
            var startX = (6 - shape.BlockShape.Width) / 2;
            var startY = (6 - shape.BlockShape.Height) / 2;

            for (int i = 0; i < shape.BlockShape.Height; i++)
            {
                for (int j = 0; j < shape.BlockShape.Width; j++)
                {
                    nextShapeGraphics.FillRectangle(
                        GenerateBlocks.FillColor(shape.BlockShape.Dots[i, j]),
                        (startX + j) * dotSize, (startY + i) * dotSize, dotSize, dotSize);
                }
            }

            pictureBox2.Size = nextShapeBitmap.Size;
            pictureBox2.Image = nextShapeBitmap;

            return shape;
        }
    }
}
