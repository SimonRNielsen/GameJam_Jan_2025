using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
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
        private static bool btnActive = false;
        //Properties

        //Constructors
        public Button() 
        {
            sprite = Gameworld.sprites["button"];
        }
        //Methods
        public override void LoadContent(ContentManager content)
        {
            hitbox = new Rectangle((int)(Position.X- (sprite.Width * scale/2)), (int)(Position.Y - (sprite.Height * scale / 2)), (int)(sprite.Width * scale), (int)(sprite.Height * scale));
            base.LoadContent(content);
        }
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
            if (isHovering &&btnActive)
            {
                color = Color.LightGray;
            }
            else if ( btnActive)
            {
                color = Color.White;
            }
            else
            {
                color = Color.Gray;
            }

            if (clicked&& btnActive)
            {
                Gameworld.AddGameObject(new ResultsDisplay(true, Gameworld.snapBoard.Score));
            }
            base.Update(gameTime);
        }
        public static void ActivateBtn(bool active)
        {
            btnActive = active;
        }
    }
}
