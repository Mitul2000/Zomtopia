using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Revenge_of_the_Non_Living
{
    class HiveMind:Enemy
    {
        private List<Enemy> hive = new List<Enemy>();
        private int spawntimer;
        private int hivecounter;
        private bool[] hivemoving;
        private bool intitialize = true;
        
        //contructor
        public HiveMind(Rectangle bossRec, Texture2D bossPic):base(0,25,bossRec,bossPic)
        {

        }

        //methods
        public void spawn(HiveMind boss,Texture2D hivePic,Character currentPlayer, Character[] player, double[] player1distance, double[] player2distance, double[] player3distance, double[] player4distance, int damage,GamePadState[] pad)
        {
            spawntimer++;//adds one to the spawn timer every tick
            if(intitialize)
            {
                for (int i = 0; i < 15; i++)
                {
                    hive.Add(new Enemy(25, 300, new Rectangle(rec.X, rec.Y, 75, 75), hivePic));
                }//adds new hive zombos to the list 
                for (int j = 0; j < hive.Count; j++)
                {
                    hivemoving = new bool[hive.Count];
                    hivemoving[j] = false;

                }//creates a new bool to determine if the zombos are moving for each zombo, assumes they are not moving to start
                
                intitialize = false;
            }
            if (spawntimer % 60 == 0 && hivecounter < 15)
            {
                hivemoving[hivecounter] = true;
                hivecounter++;
            }//every 2 seconds a new zombo starts to move
            for (int k = 0; k < hive.Count; k++)
                {
                    if (hivemoving[k])
                    {
                        hive[k].trace(player, hive, player1distance, player2distance, player3distance, player4distance);

                    }//if the hive is moving it follows the closest player
                }
            currentPlayer.Damage(hive, 25);//if the player intersects with hive the player loses health
            if (boss.getHealth() <= 3750)
            {
                boss.bossTrace(player,pad, boss);
            }//once the boss reaches half health more hive spawns
        }
        public void drawHive(SpriteBatch sb)
        {
            for(int i = 0; i<hive.Count; i++)
            hive[i].Draw(sb);//draws all the hive
        }
    }
}
