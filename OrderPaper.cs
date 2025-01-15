﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam_Jan_2025
{
    public class OrderPaper:GameObject
    {
        //Fields
        private string order;
        private bool visible = false;
        private Rectangle hitbox;
        private Vector2 visiblePos;
        private Vector2 hiddenPos;
        private float clickTimer = 0.2f;
        private float countdown;
        private Vector2 textPosition = new Vector2(20, 50);
        //Properties

        //Constructors
        public OrderPaper(string order,Vector2 position)
        {
            layer = 0.7f;
            this.position = position;
            hiddenPos = position;
            visiblePos = new Vector2(position.X, position.Y - (600 * scale));
            this.order = order;
            sprite = Gameworld.sprites["orderPaper"];
            hitbox = new Rectangle((int)(Position.X - (sprite.Width * scale / 2)), (int)(Position.Y - (sprite.Height * scale / 2)), (int)(sprite.Width * scale), (int)(sprite.Height * scale));
        }

        //Methods
        public override void Update(GameTime gameTime)
        {
            if (countdown > 0)
            {
                countdown-= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            MouseState mouseState = Mouse.GetState();
            Rectangle mouseHitbox = new Rectangle(mouseState.X, mouseState.Y, 2, 2);
            bool isHovering = false;
            bool clicked = false;
            if (mouseHitbox.Intersects(hitbox))
            {
                isHovering = true;
                if (mouseState.LeftButton == ButtonState.Pressed && countdown <= 0) 
                {
                    clicked = true;
                }
            }
            if (isHovering)
            {
                color = Color.LightGray;
            }
            else
            {
                color = Color.White;
            }

            if (clicked)
            {
                countdown = clickTimer;
                if (visible)
                {
                    visible = false;
                    position = hiddenPos;
                }
                else
                {
                    visible = true;
                    position = visiblePos;
                }
                hitbox = new Rectangle((int)(Position.X - (sprite.Width * scale / 2)), (int)(Position.Y - (sprite.Height * scale / 2)), (int)(sprite.Width * scale), (int)(sprite.Height * scale));
                //Do the things that must be done
            }
            base.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(Gameworld.textFont,("Order\n"+order), new Vector2(position.X+textPosition.X,position.Y+textPosition.Y), color,null, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, layer+0.1f);
            base.Draw(spriteBatch);
        }
    }
}
