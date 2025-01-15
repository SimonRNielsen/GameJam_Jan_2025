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
        private bool timeStopped = false;
        private Color countdownColor = Color.White;

        //Properties

        public float Countdown { get => countdown; }

        //Constructors
        public Timer() 
        {
            
        }

        public Timer(float timeInSeconds)
        {
            totalTimeSeconds = timeInSeconds;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {
            Position=new Vector2(100,20);
            foreground = Gameworld.sprites["timerForeground"];
            background = Gameworld.sprites["timerBackground"];
            foregroundOffset = new Vector2(29, 17);
            ResetTimer();
        }
        public override void Update(GameTime gameTime)
        {
            if (!timeStopped)
            {
                countdown-= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (countdown <= 0)
                {
                    countdown = 0;
                    TimeUp();
                }
            }
            if (countdown < 10)
            {
                countdownColor = Color.Red;
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(foreground, new Vector2(position.X+(foregroundOffset.X*scale),position.Y+(foregroundOffset.Y*scale)), null, countdownColor, 0, Vector2.Zero, new Vector2((countdown / totalTimeSeconds) * scale, scale), SpriteEffects.None, layer + 0.1f);
            spriteBatch.Draw(background, position, null, Color.White, 0, origin, scale, SpriteEffects.None, layer);
        }

        public void ResetTimer()
        {
            countdown = totalTimeSeconds;
            timeStopped = false;
        }
        public void Stop()
        {
            timeStopped = true;
        }

        private void TimeUp()
        {
            timeStopped = true;
            Gameworld.AddGameObject(new ResultsDisplay(false, Gameworld.snapBoard.Score));
        }
    }
}
