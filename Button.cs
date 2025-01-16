using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam_Jan_2025
{
    public class Button : GameObject
    {
        //Fields
        private Rectangle hitbox;

        //Properties

        //Constructors
        public Button() 
        {
            hitbox = new Rectangle((int)origin.X,(int)origin.Y, (int)(sprite.Width*scale), (int)(sprite.Height*scale));
            sprite = Gameworld.sprites["button"];
        }
        //Methods
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Rectangle mouseHitbox = new Rectangle(mouseState.X, mouseState.Y, 2, 2);
            bool isHovering = false;
            bool clicked = false;
            if (mouseHitbox.Intersects(hitbox))
            {
                isHovering = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    clicked = true;
                }
            }
            if (isHovering)
            {
                color = Color.Gray;
            }
            else
            {
                color = Color.White;
            }

            if (clicked)
            {
                //Do the things that must be done
            }
            base.Update(gameTime);
        }
    }
}
