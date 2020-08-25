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
    class GameItem
    {
        protected Rectangle rec;
        protected Texture2D pic;
        //constructor
        public GameItem(Rectangle Rec, Texture2D Pic)
        {
            setRec(Rec);
            setPic(Pic);
        }//sets the rectangle and the picture from the given parameter

        //getters(accessors)
        public Rectangle getRec()
        {
            return rec;
        }//returns the rectangle
        public Texture2D getPic()
        {
            return pic;
        }//returns the picture

        //setters(mutators)
        public void setRec(Rectangle newRec)
        {
            rec = newRec;
        }//set the rectangle
        public void setPic(Texture2D newPic)
        {
            pic = newPic;
        }//sets the picture

        //methods
        public void Draw(SpriteBatch sb)
        {
            
            sb.Draw(pic, rec, Color.White);
            
        }//draws a gameitem with a picture and a rectangle
    }
}
