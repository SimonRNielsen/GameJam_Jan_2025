using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam_Jan_2025
{
    public class Timer : GameObject
    {
        //Fields
        private float totalTimeSeconds=20;
        private float countdown;
        private Texture2D foreground;
        private Texture2D background;
        private Vector2 foregroundOffset;

        //Properties

        public float Countdown { get => countdown; }

        //Constructors
        public Timer() 
        {
            
        }

        public Timer(float time)
        {
            totalTimeSeconds = time;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            Position=new Vector2(100,20);
            foreground = Gameworld.sprites["timerForeground"];
            background = Gameworld.sprites["timerBackground"];
            foregroundOffset = new Vector2(20, 20);
            ResetTimer();
        }
        public override void Update(GameTime gameTime)
        {
            countdown-= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (countdown <= 0)
            {
                countdown = 0;
                TimeUp();
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(foreground, position, null, Color.White, 0, new Vector2(origin.X - (30 * scale), origin.Y - (16 * scale)), new Vector2((countdown / totalTimeSeconds) * scale, scale), SpriteEffects.None, layer + 0.1f);
            spriteBatch.Draw(background, position, null, Color.White, 0, origin, scale, SpriteEffects.None, layer);
        }

        public void ResetTimer()
        {
            countdown = totalTimeSeconds;
        }

        private void TimeUp()
        {
            //also lose customer or something
            Gameworld.RemoveGameObject(this);
        }
    }
}
