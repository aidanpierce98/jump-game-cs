using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//glitch comes from timer when gravity changes, BringToFront reduces glitch
namespace Jump_Game_C
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, jumping, isGameOver;

        int jumpSpeed;
        int force;
        int score = 0;
        bool gotAllCoins = false;
        int playerSpeed = 7;

        int horizontalSpeed = 5;
        int verticalSpeed = 3;

        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtScore_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score + Environment.NewLine + "Collect all coins";

            player.Top += jumpSpeed;

            if (goLeft == true)
            {
                player.Left -= playerSpeed;
            }

            if (goRight == true)
            {
                player.Left += playerSpeed;
            }

            //if force is less than 0 set jump to false so player cant jump higher, whenever jumping is false the jumpSpeed goes back to 10 and when true negative jumpSpeed moves player up, taking force down by one moves player down 

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -9;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            if (score == 17)
            {
                gotAllCoins = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "Enter the door";
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {

                    //if a PictureBox has tag of platform
                    if ((string)x.Tag == "Platform")
                    {
                        //check for collisions aka if player is intersecting with any platforms, force set to 8 so player can jump again, top -height makes player move right above platform when jumping on platform 
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            player.Top = x.Top - player.Height;

                            if (((string)x.Name == "horizontalPlatform" && goLeft == false) || ((string)x.Name == "horizontalPlatform" && goRight == false))
                            {
                                player.Left = player.Left - horizontalSpeed;
                            }
                        }

                        x.BringToFront();
                    }

                    //if PictureBox has tag of coin, and intersects with coin then make coin disappear and increase score by one, x.Visible = true stops the score from exponentially increasing
                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                        }
                    }

                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gameTimer.Stop();
                            isGameOver = true;
                            txtScore.Text = "Score: " + score + Environment.NewLine + "You have been defeated :(" + Environment.NewLine + "Press enter to retry";
                        }
                    }
                }
            }

            //move platform left
            horizontalPlatform.Left -= horizontalSpeed;

            //Client.Size is size of the form, reverse direction if platform touches left wall or if it touches the other wall at the end of the form size
            if (horizontalPlatform.Left < 0 || horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            //  makes platform move
            verticalPlatform.Top += verticalSpeed;

            if (verticalPlatform.Top < 195 || verticalPlatform.Top > 587)
            {
                verticalSpeed = -verticalSpeed;
            }

            enemyOne.Left -= enemyOneSpeed;

            if (enemyOne.Left < pictureBox5.Left || enemyOne.Right > pictureBox5.Right)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }

            enemyTwo.Left -= enemyTwoSpeed;

            if (enemyTwo.Left < pictureBox2.Left || enemyTwo.Right > pictureBox2.Right)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }

            if (player.Top > this.ClientSize.Height)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You Fell!";
            }

            if (player.Bounds.IntersectsWith(victory.Bounds) && gotAllCoins == false)
            {
                player.BringToFront();
            }

            if (player.Bounds.IntersectsWith(victory.Bounds) && gotAllCoins)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You Win!";
            }
      
        }

        private void Form1_ContextMenuStripChanged(object sender, EventArgs e)
        {

        }

        private void victory_Click(object sender, EventArgs e)
        {

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode  == Keys.Left)
            {
                goLeft = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }

            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }

            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {

            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            gotAllCoins = false;
            score = 0;

            txtScore.Text = "Score: " + score + Environment.NewLine + "Collect all coins";

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }

            }

            // reset position of player, coin, and enemies
            
            player.Left = 59;
            player.Top = 695;

            enemyOne.Left = 432;
            enemyTwo.Left = 420;

            horizontalPlatform.Left = 261;
            verticalPlatform.Top = 567;

            gameTimer.Start();
        }
    }
}
