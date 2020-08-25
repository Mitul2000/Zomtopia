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
    class Character:GameItem
    {
        protected int health;
        private int speedX;
        private int speedY;
        private int count = -1;
        private bool enemyDamage;
        private int damageTimer;
        private int points;
 
        private List<Projectile> projList = new List<Projectile>();
        

        //constructor
        public Character (int Points,int Health, Rectangle player, Texture2D playerpic):base(player,playerpic)
        {
            setHealth(Health);
            setPoints(Points);
         
        }//sets the value of the character health and points

        //getters(accessors)
        public int getHealth()
        {
            return health;
        }//returns the value of health
        public int getPoints()
        {
            return points;
        }//returns the value of points
        public int getCount()
        {
            return count;
        }//returns the value of count

        //setters(mutators)
        public void setHealth(int newHealth)
        {
            health = newHealth;
        }//sets the value of health
        public void setPoints(int newPoints)
        {
            points = newPoints;
        }//sets the value of points
       

        //methods
        public void loseHealth(int damage)
        {
            health -= damage;

        }//health is subtracted by 25
        public void losePoints()
        {
            points -= 1000;
        }
        public void Move(GamePadState pad, int screenwidth, int screenheight)
        {
            if (pad.ThumbSticks.Left.X > 0 && rec.X < screenwidth-rec.Width)
            {
                rec.X += 5;
            
            }
            if (pad.ThumbSticks.Left.X < 0 && rec.X > 0)
            {
                rec.X -= 5;
                
            }
            if (pad.ThumbSticks.Left.Y > 0 && rec.Y > 0)
            {
                rec.Y -= 5;
                
            }
            if (pad.ThumbSticks.Left.Y < 0 && rec.Y < screenheight-rec.Height)
            {
                rec.Y += 5;
                
            }//moves a player in the direction of the left thumbstick as long as they are in the screen
        }
        public void Update(GamePadState pad, GamePadState oldpad, Rectangle projectileType, Texture2D projectilePic, int maxCount, int projSpeedX, int projSpeedY)
        {
            if (pad.ThumbSticks.Left.X > 0)
            {               
                speedX = projSpeedX;
                speedY = 0;
            }
            if (pad.ThumbSticks.Left.X < 0)
            {
              
                speedX = -projSpeedX;
                speedY = 0;
            }
            if (pad.ThumbSticks.Left.Y > 0)
            {
                
                speedX = 0;
                speedY = -projSpeedY;
            }
            if (pad.ThumbSticks.Left.Y < 0)
            {
               
                speedX = 0;
                speedY = projSpeedY;    
            }//makes the projectile move in the direction of the player
            if (pad.ThumbSticks.Left.Y > 0 && pad.ThumbSticks.Left.X > 0)
            {
                speedX = projSpeedX;
                speedY = -projSpeedY;
            }
            if (pad.ThumbSticks.Left.Y > 0 && pad.ThumbSticks.Left.X < 0)
            {
                speedX = -projSpeedX;
                speedY = -projSpeedY;
            }
            if (pad.ThumbSticks.Left.Y < 0 && pad.ThumbSticks.Left.X < 0)
            {
                speedX = -projSpeedX;
                speedY = projSpeedY;
            }
            if (pad.ThumbSticks.Left.Y < 0 && pad.ThumbSticks.Left.X > 0)
            {
                speedX = projSpeedX;
                speedY = projSpeedY;
            }//allows the player to shoot diagonally
            if (pad.Buttons.A == ButtonState.Pressed && oldpad.Buttons.A == ButtonState.Released && count <=maxCount)
            {
                projList.Add(new Projectile(speedX,speedY,25,projectileType,projectilePic));
                count++;
            }//adds a projectile to the list for a player and removes one from the current count
            if(pad.Buttons.X == ButtonState.Pressed)
            {
                count = 0;
            }
            if(count >=0)
            {
                for(int j = 0; j<=projList.Count-1; j++)
                {
                    if(projList[j] != null)
                    projList[j].Move();
                  
                }
            }//when count is 0 or greater a player can move the projectile if it is not null
           
        }
        public void Damage(List<Enemy> zombos, int damage)
        {
            damageTimer++;//adds one to the damage timer every tick
            for (int i = 0; i<= projList.Count-1;i++)
            {
                if(projList[i] != null)
                {
                    projList[i].Intersection(zombos, damage);

                    //if a projectile is not null and  hits a zombo it loses health
                    if (projList[i].getProjhit() && damageTimer % 5 == 0)
                    {
                        points += 5;
                        projList[i] = null;
                    }//if a projectile hits a zombo or boss the player gets 5 points and the projectile is set to null(can only happen every 1/12 of a second)
                }
     
            }
            for (int k = 0; k <= zombos.Count - 1; k++)
            {
                if (rec.Intersects(zombos[k].getRec()) && damageTimer % 15 == 0)
                {
                    enemyDamage = true;
                }//if a player intersects with a zombie they will receive damage(can only happen once every 1/4 of a second)

                if (zombos[k].getHealth() <= 0)
                {
                    zombos.RemoveAt(k);
                    k--;
                }//once a zombo's health reaches 0 they are removed from the list then the next zombo takes the place of the previous zombo

            }
            
            if (enemyDamage)
            {
                if(health == 0)
                {
                    health = 0;
                }
                else
                {
                    health -= 25;
                    
                }
                enemyDamage = false;
            }//the player receives only a set amount of damage

        }
        public void bossDamage(Enemy boss, int damage)
        {
            for(int i = 0; i<=projList.Count-1; i++)
            {
                if (damageTimer % 15 == 0 && projList[i] !=null)
                {
                    projList[i].bossIntersection(boss, damage);
                }//if a projectile hits a boss they lose health(can only happen every 1/4 of a second)
            }
            
            if (rec.Intersects(boss.getRec()) && damageTimer % 15 == 0 && boss.getHealth() > 0)
            {
               
                if (health == 0)
                {
                    health = 0;
                }
                else
                {
                    health -= 50;
                }
            }//if a player intersects with a boss that is not defeated they lose 50 hp (can only happen once every 1/4 of a second)
        }
        public void projDraw(SpriteBatch sb)
        {
            for (int i = 0; i <= projList.Count - 1; i++)
            {
                if(projList[i] != null)
                {
                    projList[i].Draw(sb);
                }
                

            }//draws a player's projectiles if they are not null
        }
    }
}
