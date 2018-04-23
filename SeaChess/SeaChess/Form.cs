using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaChess
{
    public partial class Form : System.Windows.Forms.Form
    {
        Bitmap imgH, imgAI;
        Chess game;
        int gamesCount, human, AI;

        public Form()
        {
            InitializeComponent();
            AI = 0;
            human = 0;
            gamesCount = 1;
            NewGame();
        }
        
        public void Box_Click(object sender, EventArgs e)
        {
            //change Image and Tag
            ((PictureBox)sender).Enabled = false;
            ((PictureBox)sender).Image = imgH;
            ((PictureBox)sender).Tag = "x";

            //send move to game
            int.TryParse(((PictureBox)sender).Name[10].ToString(), out int index);
            game.Human(index);
            if (game.humanWon)
            {
                MessageBox.Show("You won! ;(");
                human++;
                gamesCount++;
                NewGame();
                return;
            }

            //request and process a move from AI
            index = game.AImove;

            if (index != 0)
            {
                PictureBox picBox = getPB(index);
                picBox.Image = imgAI;
                picBox.Enabled = false;
            }

            //check if Ai won
            if (game.AIWon)
            {
                MessageBox.Show("I won! nqnqnq");
                AI++;
                gamesCount++;
                NewGame();
            }
            else if(game.filled == 9 )
            {
                MessageBox.Show("I'll get you !");
                gamesCount++;
                NewGame();
            }
        }

        private void NewGame()
        {
            //writes the score
            text.Text = "Me: "+ AI.ToString() +"\nYou: " + human.ToString();

            //determines the beginner
            bool humanBegins = false;
            if (Math.IEEERemainder(gamesCount, 2) != 0)
            {
                humanBegins = true;
            }

            //set Human and AI marks
            if (radioX.Checked == true)
            {
                imgH = Properties.Resources.x;
                imgAI = Properties.Resources.o;
            }
            else
            {
                imgH = Properties.Resources.o;
                imgAI = Properties.Resources.x;
            }

            //reset Picture Boxes
            foreach (Control control in Controls)
            {
                if (control is PictureBox)
                {
                    ((PictureBox)control).Image = null;
                    control.Enabled = true;
                }
            }
            
            //create new game
            game = new Chess(humanBegins);

            //AI makes First Move if it's his turn
            if (!humanBegins)
            {
                PictureBox picBox = getPB(game.AI());
                picBox.Image = imgAI;
                picBox.Enabled = false;
            }
        }

        private PictureBox getPB(int index)
        {
            Object[] array;
            array = Controls.Find("PictureBox" + index.ToString(), true);
            return ((PictureBox)array[0]);
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            human = 0;
            AI = 0;
            gamesCount = 1;
            NewGame();
        }

        private void test_Click(object sender, EventArgs e)
        {
            MessageBox.Show(game.ArrayToString() + "\n" + game.move.ToString());
        }
    }
}
