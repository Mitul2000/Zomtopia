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
    class Projectile:GameItem
    {
        private int damage;
        private int speedx;
        private int speedy;
        private bool projhit;
        //constructor
        public Projectile (int Speedx,int Speedy, int Damage, Rectangle projectileRec, Texture2D projectilePic):base(projectileRec, projectilePic)
        {
           
            setDamage(Damage);
            setSpeedx(Speedx);
            setSpeedy(Speedy);
        }//sets the values of damage, ammo, speed and hit

        //getters(accessors)
        public int getDamage()
        {
            return damage;
        }//returns the value of damage
        public int getSpeedx()
        {
            return speedx;
        }//returns the value of speedx
        public int getSpeedy()
        {
            return speedy;
        }//returns the value of speedy
        public bool getProjhit()
        {
            return projhit;
        }//returns the state of projhit
        
        //setters(mutators)
        public void setDamage(int newDamage)
        {
            damage = newDamage;
        }//sets the amount of damage of each projectile type
        public void setSpeedx(int newSpeedx)
        {
            speedx = newSpeedx;
        }//sets the speed in the x direction
        public void setSpeedy(int newSpeedy)
        {
            speedy = newSpeedy;
        }//sets the speed in the y direction
        public void setProjhit(bool newProjhit)
        {
            projhit = newProjhit;
        }//sets the state of projhit
        
        //methods
         public void Move()
        {
           rec.X += 1+speedx;
           rec.Y += speedy; 
        }//moves the projectile in the X and Y direction based on the given values
        public void Intersection(List<Enemy> enemy, int damage)
        {
            for(int i = 0; i<enemy.Count;i++)
            {
                if(rec.Intersects(enemy[i].getRec()))
                {
                    enemy[i].loseHealth(damage);
                    projhit = true;
                }
                
            }
           
        }//if a projectile intersects a zombo the zombo will lose health and the projectile will be declared making contact
        public void bossIntersection(Enemy boss, int damage)
        {
            if(rec.Intersects(boss.getRec()))
            {
                boss.loseHealth(damage);
                projhit = true;
            }
            else
            {
                projhit = false;
            }
            
        }//if a projectile intersects with a boss the boss will lose hp and the bool for projectile contact will be set to true
        public void playerIntersection(Character player,int damage)
        {
            if(rec.Intersects(player.getRec()))
            {
                player.loseHealth(damage);
            }
        }//if a projectile intersects with a player the player will lose hp
     
   
    }
}
