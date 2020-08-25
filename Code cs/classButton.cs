using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Revenge_of_the_Non_Living
{
    class classButton
    {
        string btnType;
        Texture2D BTexture;
        Vector2 Bposition;
        Rectangle BRect;
        Rectangle clickRec;

        bool down;

        Color colour = new Color(255, 255, 255, 255);

        public Vector2 size;

        public classButton(Texture2D newTexture, GraphicsDevice graphics)
        {
            BTexture = newTexture;
            size = new Vector2(graphics.Viewport.Width / 5, graphics.Viewport.Height / 15);
            btnType = "other";
        }

     public void setBtntype(string newBtntype)
            {
            btnType = newBtntype;
            }
        

        public bool isClicked;
        public void Update (GamePadState pad)
        {
            BRect = new Rectangle((int)Bposition.X, (int)Bposition.Y, (int)size.X, (int)size.Y);
            if (btnType == "Active")
                {
                clickRec = new Rectangle((int)Bposition.X, (int)Bposition.Y, (int)size.X, (int)size.Y);

                if (clickRec.Intersects(BRect))
                {
                    if (colour.A == 255) down = false;
                    if (colour.A == 0) down = true;
                    if (down) colour.A += 3;
                    else colour.A -= 3;
                    if ((pad.Buttons.A == ButtonState.Pressed)) isClicked = true;
                }
                else if (colour.A < 255)
                {
                    colour.A += 3;
                    isClicked = false;
                }
            
            }
           else if(btnType != "Active")
                {
                colour.A = 255;
            }
            
           
        }

        public void setPosition(Vector2 newPosition)
        {
            Bposition = newPosition;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(BTexture, BRect, colour);
            Console.WriteLine(colour.A.ToString());
        }



    }
}
