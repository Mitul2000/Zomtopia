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
    class Tobu:Enemy
    {
        private List<Projectile> bossProj = new List<Projectile>();
        private int speedX;
        private int speedY;
        private int count = -1;
        private int tick;
        private int attackTimer = 240;
        private bool attacking;
        private bool countdown;
        private string[] potentialattackPattern = new string[10];
        private string currentAttack;
        //constructor
        public Tobu(Rectangle bossRec, Texture2D bossPic):base(0,25, bossRec, bossPic)
        {

        }

        //methods
        public void Attack(int projSpeedX, int projSpeedY, Rectangle projRec, Texture2D projPic, Random rnd, Tobu boss, Character player, int damage)
        {
            
            potentialattackPattern[0] = "up";
            potentialattackPattern[1] = "down";
            potentialattackPattern[2] = "left";
            potentialattackPattern[3] = "right";
            potentialattackPattern[4] = "spread";
            potentialattackPattern[5] = "up/left";
            potentialattackPattern[6] = "up/right";
            potentialattackPattern[7] = "down/left";
            potentialattackPattern[8] = "down/right";//a list of all potential attack patterns
            potentialattackPattern[9] = "rate";
            tick++;
            if(boss.getHealth() >=3750)
            {
                if (tick % 120 == 0 && !attacking)
                {
                    currentAttack = potentialattackPattern[rnd.Next(0, 8)];

                }
            }//if the boss is above half health and 2 seconds has passed a new attack is chosen
            else
            {
                if (tick % 60 == 0 && !attacking)
                {
                    currentAttack = potentialattackPattern[rnd.Next(0, 9)];
                   
                }
            }//if the boss is below half health and 1 second has passed a new attack is chosen
            
            if(currentAttack == "up")
            {
                attacking = true;
                speedX = 0;
                speedY = -projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for(int j = 0; j<=bossProj.Count-1;j++)
                {
                    if(count>=0)
                    bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//moves projectiles in the upward direction
            if(currentAttack == "down")
            {
                attacking = true;
                speedX = 0;
                speedY = projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//moves projectiles in the downward direction
            if(currentAttack == "left")
            {
                attacking = true;
                speedX = -projSpeedX;
                speedY = 0;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//moves projectiles in the left direction
            if(currentAttack == "right")
            {
                attacking = true;
                speedX = projSpeedX;
                speedY = 0;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//moves projectiles in the right direction
            if(currentAttack == "up/left")
            {
                attacking = true;
                speedX = -projSpeedX;
                speedY = -projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//moves projectiles in the up and left direction
            if(currentAttack == "up/right")
            {
                attacking = true;
                speedX = projSpeedX;
                speedY = -projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//moves projections in the up and right direction
            if(currentAttack == "down/left")
            {
                attacking = true;
                speedX = -projSpeedX;
                speedY = projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//moves projectiles in the down and left direction
            if(currentAttack == "down/right")
            {
                attacking = true;
                speedX = projSpeedX;
                speedY = projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//shoots projectiles in the direction specified over a specific interval by changing the x and y values the projectiles move
            if(currentAttack == "rate")
            {
                attacking = true;
                for(int i = 0; i < 20; i++)
                {
                    for(int j = 0; j>20; j--)
                    {
                        speedX = i;
                        speedY = j;
                    }
                }
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;

            }
            if(currentAttack == "spread" && boss.getHealth()<=3750)
            {
                attacking = true;
                speedX = 0;
                speedY = -projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                speedX = 0;
                speedY = projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                speedX = -projSpeedX;
                speedY = 0;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                speedX = projSpeedX;
                speedY = 0;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                speedX = -projSpeedX;
                speedY = -projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                speedX = -projSpeedX;
                speedY = projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                speedX = projSpeedX;
                speedY = -projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                speedX = projSpeedX;
                speedY = projSpeedY;
                bossProj.Add(new Projectile(speedX, speedY, 20, projRec, projPic));
                count++;
                for (int j = 0; j <= bossProj.Count - 1; j++)
                {
                    if (count >= 0)
                        bossProj[j].Move();
                    bossProj[j].playerIntersection(player, damage);
                }
                countdown = true;
            }//shoot projectiles in a spread pattern for a specific amount of time when the boss is below half hp
            
            if(countdown)
            {
                attackTimer--;
            }//counts down the attack timer
            if(attackTimer == 0)
            {
                attacking = false;
                countdown = false;
                attackTimer = 240;
            }//once the timer equals 0 the boss is set to not attacking, the timer stops counting down and is reset to 4 seconds
          
        }
        public void drawBossProj(SpriteBatch sb)
        {
            for (int i = 0; i < bossProj.Count; i++)
            {
                bossProj[i].Draw(sb);
            }//draws the projectiles
        }
    }
}
