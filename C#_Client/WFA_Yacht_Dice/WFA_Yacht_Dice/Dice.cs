using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace WFA_Yacht_Dice
{
    class Dice
    {
        private PictureBox mypicbox;
        private int dicenum;
        private Random random;

        public PictureBox getPicbox {
            get { return mypicbox; }
        } 

        private static Image[] dice_images = { Properties.Resources.img_dice_1,
                                   Properties.Resources.img_dice_2,
                                   Properties.Resources.img_dice_3,
                                   Properties.Resources.img_dice_4,
                                   Properties.Resources.img_dice_5,
                                   Properties.Resources.img_dice_6 };

        public Dice(PictureBox picbox, Random random) {
            this.mypicbox = picbox;
            this.random = random;
        }

        public void diceroll() {
            mypicbox.Image = dice_images[random.Next(0, 5)];
        }

        public void setDice(int num) {
            dicenum = num;
            Debug.WriteLine("dice : " + num + "\n");
            mypicbox.Image = dice_images[num - 1];
        }
    }
}
