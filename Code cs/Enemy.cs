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
    class Enemy:Character
    {
        private int damage;
        private int bossDashTimer = 60;
        private int tick;
        private bool[] followP1 = new bool[2];
        private bool[] followP2 = new bool[2];
        private bool[] followP3 = new bool[2];
        private bool[] followP4 = new bool[2];
        private bool followlastPlayer;
        private double[] playerBossDistance = new double[4];
        private bool countdown;
        private bool bossDash;
        private bool facingLeft;
        private bool facingRight;
        private bool facingUp;
        private bool facingDown;

        //constructor
        public Enemy (int Damage, int Health,Rectangle enemyRec, Texture2D enemyPic):base(0,Health, enemyRec, enemyPic)
        {
            setDamage(Damage);
        }

        //getters(accessors)
        public int getDamage()
        {
            return damage;
        }//returns the damage value
        public bool getfacingLeft()
        {
            return facingLeft;
        }
        public bool getfacingRight()
        {
            return facingRight;
        }
        public bool getfacingUp()
        {
            return facingUp;
        }
        public bool getfacingDown()
        {
            return facingDown;
        }
        //setters(mutators)
        public void setDamage(int newDamage)
        {
            damage = newDamage;
        }//sets the damage value

        //methods
        public void trace(Character[] player, List<Enemy> enemy, double[] player1Distance, double[] player2Distance, double[] player3Distance, double[] player4Distance)
        {
            
            if (player[1] == null && player[2] == null && player[3] == null)
            {
                if (rec.X < player[0].getRec().X)
                {
                    rec.X++;
                    facingRight = true;
                }
                else
                {
                    facingRight = false;
                }
                if (rec.X > player[0].getRec().X)
                {
                    rec.X--;
                    facingLeft = true;
                }
                else
                {
                    facingLeft = false;
                }
                if (rec.Y > player[0].getRec().Y)
                {
                    rec.Y--;
                    facingUp = true;
                }
                else
                {
                    facingUp = false;
                }
                if (rec.Y < player[0].getRec().Y)
                {
                    rec.Y++;
                    facingDown = true;
                }
                else
                {
                    facingDown = false;
                }
            }//enemies will follow player 1
            else if (player[2] == null && player[3] == null)
            {
                for (int i = 0; i < enemy.Count; i++)
                {

                    if (player1Distance[i] < player2Distance[i])
                    {
                        followP1[0] = true;
                     
                    }
                    else
                    {
                        followP1[0] = false;
                    }
                    if (player1Distance[i] > player2Distance[i])
                    {
                        followP2[0] = true;
                    }
                    else
                    {
                        followP2[0] = false;
                    }
                }//follows the closest player for two players
            }
            else if(player[3] == null)
            {
                for(int j = 0; j<enemy.Count; j++)
                {
                    if (player1Distance[j] < player2Distance[j] && player1Distance[j] < player3Distance[j])
                    {
                        followP1[0] = true;
                    }
                    else
                    {
                        followP1[0] = false;
                    }
                    if(player2Distance[j] < player1Distance[j] && player2Distance[j] < player3Distance[j])
                    {
                        followP2[0] = true;
                    }
                    else
                    {
                        followP2[0] = false;
                    }
                    if(player3Distance[j] < player1Distance[j] && player3Distance[j] < player2Distance[2])
                    {
                        followP3[0] = true;
                    }
                    else
                    {
                        followP3[0] = false;
                    }
                    if(!followP1[0] && !followP2[0] && !followP3[0])
                    {
                        followlastPlayer = true;
                    }//if the zombos are not following anyone they will follow player 1 (3 player)
                    else
                    {
                        followlastPlayer = false;
                    }
                }//sets following true for the player with the least distance from the enemy
                
            }
            else
            {
                for (int k = 0; k < enemy.Count; k++)
                {
                    if (player1Distance[k] < player2Distance[k] && player1Distance[k] < player3Distance[k] && player1Distance[k] < player4Distance[k])
                    {
                        followP1[0] = true;
                    }
                    else
                    {
                        followP1[0] = false;
                    }
                    if (player2Distance[k] < player1Distance[k] && player2Distance[k] < player3Distance[k] && player2Distance[k] < player4Distance[k])
                    {
                        followP2[0] = true;
                    }
                    else
                    {
                        followP2[0] = false;
                    }
                    if (player3Distance[k] < player1Distance[k] && player3Distance[k] < player2Distance[k] && player3Distance[k] < player4Distance[k] )
                    {
                        followP3[0] = true;
                    }
                    else
                    {
                        followP3[0] = false;
                    }
                    if(player4Distance[k] < player1Distance[k] && player4Distance[k] < player2Distance[k] && player4Distance[k] < player3Distance[k])
                    {
                        followP4[0] = true;
                    }
                    else
                    {
                        followP4[0] = false;
                    }
                    if (!followP1[0] && !followP2[0] && !followP3[0] && !followP4[0])
                    {
                        followlastPlayer = true;
                    }//if the zombos are not following anyone they will follow player 1 (4 player)
                    else
                    {
                        followlastPlayer = false;
                    }
                }//sets following true for the player with the least distance from the enemy
            }
            if (!followP1[0] && !followP2[0] && !followP3[0])
            {
                followP1[0] = true;
            }//if the zombos are not following anyone they will follow player 1 (3 player)
            if (!followP1[0] && !followP2[0] && !followP3[0] && followP4[0])
            {
                followP1[0] = true;
            }//if the zombos are not following anyone they wil follow player 1 (4 player)
            if (followP1[0])
            {
                if (rec.X < player[0].getRec().X)
                {
                    rec.X++;
                    facingRight = true;
                }
                else
                {
                    facingRight = false;
                }
                if (rec.X > player[0].getRec().X)
                {
                    rec.X--;
                    facingLeft = true;

                }

                if (rec.Y > player[0].getRec().Y)
                {
                    rec.Y--;
                    facingUp = true;
                }
                if (rec.Y < player[0].getRec().Y)
                {
                    rec.Y++;
                    facingDown = true;
                }
            }//enemy follows player one
            if (followP2[0])
            {
                if (rec.X < player[1].getRec().X)
                {
                    rec.X++;
                    facingRight = true;
                }
                if (rec.X > player[1].getRec().X)
                {
                    rec.X--;
                    facingLeft = true;
                }
                if (rec.Y > player[1].getRec().Y)
                {
                    rec.Y--;
                    facingUp = true;
                }
                if (rec.Y < player[1].getRec().Y)
                {
                    rec.Y++;
                    facingDown = true;
                }
            }//enemy follows player 2
            if(followP3[0])
            {
                if (rec.X < player[2].getRec().X)
                {
                    rec.X++;
                    facingRight = true;
                }
                if (rec.X > player[2].getRec().X)
                {
                    rec.X--;
                    facingLeft = true;
                }
                if (rec.Y > player[2].getRec().Y)
                {
                    rec.Y--;
                    facingUp = true;
                }
                if (rec.Y < player[2].getRec().Y)
                {
                    rec.Y++;
                    facingDown = true;
                }
            }//enemy follows player 3
            if(followP4[0])
            {
                if (rec.X < player[3].getRec().X)
                {
                    rec.X++;
                    facingRight = true;
                }
                if (rec.X > player[3].getRec().X)
                {
                    rec.X--;
                    facingLeft = true;
                }
                if (rec.Y > player[3].getRec().Y)
                {
                    rec.Y--;
                    facingUp = true;
                }
                if (rec.Y < player[3].getRec().Y)
                {
                    rec.Y++;
                    facingDown = true;
                }
            }//enemy follows player 4
            if(followlastPlayer)
            {
                for(int i = 0; i<player.Length; i++)
                {
                    if(player[i]!= null)
                    {
                        if (player[i].getHealth() > 0)
                        {
                            if (rec.X < player[i].getRec().X)
                            {
                                rec.X++;
                                facingRight = true;
                            }
                            if (rec.X > player[i].getRec().X)
                            {
                                rec.X--;
                                facingLeft = true;
                            }
                            if (rec.Y > player[i].getRec().Y)
                            {
                                rec.Y--;
                                facingUp = true;
                            }
                            if (rec.Y < player[i].getRec().Y)
                            {
                                rec.Y++;
                                facingDown = true;
                            }
                        }
                    }
                    
                }
               
            }//will follow the player that is found alive if they are connected
        }

        public void bossTrace(Character[] player, GamePadState[] pad, Enemy boss)
        {
            tick++;//keeps track of the update method
            for(int i = 0; i<playerBossDistance.Length;i++)
            {
                if(pad[i].IsConnected)
                playerBossDistance[i] = Math.Sqrt((rec.Y - player[i].getRec().Y) * (rec.Y - player[i].getRec().Y) + (rec.X - player[i].getRec().X) * (rec.X - player[i].getRec().X));
            }
            //gets each player distance from the boss

            if (player[1] == null && player[2] == null && player[3] == null)//if only one player is connected
            {
                if(boss.getHealth()<3750 && tick%180 == 0)
                {
                    bossDash = true;
                    countdown = true;
                }//once health reaches half and 3 seconds has passed the boss dash bool and the countdown bool are set to true
                else
                {
                    if (rec.X < player[0].getRec().X)
                    {
                        rec.X++;
                    }
                    if (rec.X > player[0].getRec().X)
                    {
                        rec.X--;
                    }
                    if (rec.Y > player[0].getRec().Y)
                    {
                        rec.Y--;
                    }
                    if (rec.Y < player[0].getRec().Y)
                    {
                        rec.Y++;
                    }
                }//otherwise the boss follows the player normally
                if (bossDash)
                {
                    if (rec.X < player[0].getRec().X)
                    {
                        rec.X += 3;
                    }
                    if (rec.X > player[0].getRec().X)
                    {
                        rec.X -= 3;
                    }
                    if (rec.Y > player[0].getRec().Y)
                    {
                        rec.Y -= 3;
                    }
                    if (rec.Y < player[0].getRec().Y)
                    {
                        rec.Y += 3;
                    }
                }//if boss dash is true the boss gains an increase in speed for 1 second
                if (countdown)
                {
                    bossDashTimer--;
                }//if the timer is counting down the timer is counting down by one every update
                if(bossDashTimer == 0)
                {
                    countdown = false;
                    bossDash = false;
                    bossDashTimer = 60;
                }//once the timer reaches 0 the boss returns to normal and the timer is reset
               
            }
            else if(player[2] == null && player[3] == null)
            {
                if (boss.getHealth() < 3750 && tick % 180 == 0)
                {
                    bossDash = true;
                    countdown = true;
                }//once health reaches half and 3 seconds has passed the boss dash bool and the countdown bool are set to true
                if (playerBossDistance[0] < playerBossDistance[1])
                {
                    followP1[1] = true;
                }
                else
                {
                    followP1[1] = false;
                }
                if(playerBossDistance[0] > playerBossDistance[1])
                {
                    followP2[1] = true;
                }
                else
                {
                    followP2[1] = false;
                }
            }//determines which player to follow based on their respective distance from the boss
            else if(player[3] == null)
            {
                if (boss.getHealth() < 3750 && tick % 180 == 0)
                {
                    bossDash = true;
                    countdown = true;
                }//once health reaches half and 3 seconds has passed the boss dash bool and the countdown bool are set to true
                if (playerBossDistance[0] < playerBossDistance[1] && playerBossDistance[0] < playerBossDistance[2])
                {
                    followP1[1] = true;
                }
                else
                {
                    followP1[1] = false;
                }
                if (playerBossDistance[1] < playerBossDistance[0] && playerBossDistance[1] < playerBossDistance[2])
                {
                    followP2[1] = true;
                }
                else
                {
                    followP2[1] = false;
                }
                if(playerBossDistance[2] < playerBossDistance[0] && playerBossDistance[2] < playerBossDistance[1])
                {
                    followP3[1] = true;
                }
                else
                {
                    followP3[1] = false;
                }
            }//determines which player to follow based on repective distance from the boss
            else
            {
                if (boss.getHealth() < 3750 && tick % 180 == 0)
                {
                    bossDash = true;
                    countdown = true;
                }//once health reaches half and 3 seconds has passed the boss dash bool and the countdown bool are set to true
                if (playerBossDistance[0] < playerBossDistance[1] && playerBossDistance[0] < playerBossDistance[2] && playerBossDistance[0] < playerBossDistance[3])
                {
                    followP1[1] = true;
                }
                else
                {
                    followP1[1] = false;
                }
                if (playerBossDistance[1] < playerBossDistance[0] && playerBossDistance[1] < playerBossDistance[2] && playerBossDistance[1] < playerBossDistance[3])
                {
                    followP2[1] = true;
                }
                else
                {
                    followP2[1] = false;
                }
                if (playerBossDistance[2] < playerBossDistance[0] && playerBossDistance[2] < playerBossDistance[1]&& playerBossDistance[2] <playerBossDistance[3])
                {
                    followP3[1] = true;
                }
                else
                {
                    followP3[1] = false;
                }
                if (playerBossDistance[3] < playerBossDistance[0] && playerBossDistance[3] < playerBossDistance[1] && playerBossDistance[3] < playerBossDistance[2])
                {
                    followP4[1] = true;
                }
                else
                {
                    followP4[1] = false;
                }
            }//determines which player to follow based on repective distance from the boss
            if (bossDash && followP1[1])//if the boss is following player 1 and boss dash is true the boss receives a speed boost
            {
                if (rec.X < player[0].getRec().X)
                {
                    rec.X+=3;
                }
                if (rec.X > player[0].getRec().X)
                {
                    rec.X-=3;
                }
                if (rec.Y > player[0].getRec().Y)
                {
                    rec.Y-=3;
                }
                if (rec.Y < player[0].getRec().Y)
                {
                    rec.Y+=3;
                }
            }
            else if (followP1[1])
            {
                if (rec.X < player[0].getRec().X)
                {
                    rec.X++;
                }
                if (rec.X > player[0].getRec().X)
                {
                    rec.X--;
                }
                if (rec.Y > player[0].getRec().Y)
                {
                    rec.Y--;
                }
                if (rec.Y < player[0].getRec().Y)
                {
                    rec.Y++;
                }
            }//boss follows player one
            if (bossDash && followP2[1])//if the boss is following player 2 and boss dash is true the boss receives a speed boost
            {
                if (rec.X < player[1].getRec().X)
                {
                    rec.X += 3;
                }
                if (rec.X > player[1].getRec().X)
                {
                    rec.X -= 3;
                }
                if (rec.Y > player[1].getRec().Y)
                {
                    rec.Y -= 3;
                }
                if (rec.Y < player[1].getRec().Y)
                {
                    rec.Y += 3;
                }
            }
            else if (followP2[1])
            {
                if (rec.X < player[1].getRec().X)
                {
                    rec.X++;
                }
                if (rec.X > player[1].getRec().X)
                {
                    rec.X--;
                }
                if (rec.Y > player[1].getRec().Y)
                {
                    rec.Y--;
                }
                if (rec.Y < player[1].getRec().Y)
                {
                    rec.Y++;
                }
            }//boss follows player 2
            if (bossDash && followP3[1])//if the boss is following player 3 and boss dash is true the boss receives a speed boost
            {
                if (rec.X < player[2].getRec().X)
                {
                    rec.X += 3;
                }
                if (rec.X > player[2].getRec().X)
                {
                    rec.X -= 3;
                }
                if (rec.Y > player[2].getRec().Y)
                {
                    rec.Y -= 3;
                }
                if (rec.Y < player[2].getRec().Y)
                {
                    rec.Y += 3;
                }
            }
            else if (followP3[1])
            {
                if (rec.X < player[2].getRec().X)
                {
                    rec.X++;
                }
                if (rec.X > player[2].getRec().X)
                {
                    rec.X--;
                }
                if (rec.Y > player[2].getRec().Y)
                {
                    rec.Y--;
                }
                if (rec.Y < player[2].getRec().Y)
                {
                    rec.Y++;
                }
            }//boss follows player 3
            if (bossDash && followP4[1])//if the boss is following player 4 and boss dash is true the boss receives a speed boost
            {
                if (rec.X < player[3].getRec().X)
                {
                    rec.X += 3;
                }
                if (rec.X > player[3].getRec().X)
                {
                    rec.X -= 3;
                }
                if (rec.Y > player[3].getRec().Y)
                {
                    rec.Y -= 3;
                }
                if (rec.Y < player[3].getRec().Y)
                {
                    rec.Y += 3;
                }
            }
            else if (followP4[1])
            {
                if (rec.X < player[3].getRec().X)
                {
                    rec.X++;
                }
                if (rec.X > player[3].getRec().X)
                {
                    rec.X--;
                }
                if (rec.Y > player[3].getRec().Y)
                {
                    rec.Y--;
                }
                if (rec.Y < player[3].getRec().Y)
                {
                    rec.Y++;
                }
            }//boss follows player 4
            if (countdown)
            {
                bossDashTimer--;
            }
            if (bossDashTimer == 0)
            {
                countdown = false;
                bossDash = false;
                bossDashTimer = 60;
            }
        }
        
    }
}
