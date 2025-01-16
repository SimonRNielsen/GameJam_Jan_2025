using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GameJam_Jan_2025
{
    public class ResultsDisplay : GameObject
    {
        private bool goodResult;
        private int points;
        private bool finishedInTime;
        private int midRanking = 10;
        private int goodRanking = 20;
        private Texture2D smiley;
        private string response;
        public ResultsDisplay(bool finishedInTime, int points)
        {
            this.finishedInTime = finishedInTime;
            if (Gameworld.snapBoard.CheckSlots())
                Gameworld.snapBoard.ScoreCalculation();
            this.points = Gameworld.snapBoard.BuildScore;
            layer = 0.9f;
            position = new Vector2(960, 540);
            Button btn = new Button(false,this);
            btn.Position = new Vector2(1000, 800);
            Gameworld.AddGameObject(btn);
            RateResult();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Gameworld.textFont, response + "\n\nPoints: " + points, new Vector2(position.X - 170, position.Y - 200), Color.Black, 0, Vector2.Zero, scale * 2.5f, SpriteEffects.None, layer + 0.1f);
            spriteBatch.Draw(smiley, new Vector2(position.X + 200, position.Y - 50), null, Color.White, 0, Vector2.Zero, scale * 0.8f, SpriteEffects.None, layer + 0.1f);
            base.Draw(spriteBatch);
        }
        private void RateResult()
        {
            if (!finishedInTime)
            {
                goodResult = false;
                points = 0;
                smiley = Gameworld.sprites["badSmiley"];
                response = "This isn't even finished! I'm not paying!";
                Gameworld.sounds["badReview"].Play();
            }
            else if (Gameworld.snapBoard.LatestReview == 1)
            {
                goodResult = true;
                smiley = Gameworld.sprites["goodSmiley"];
                response = "Amazing! It's better than I imagined!";
                Gameworld.sounds["goodReview"].Play();
            }
            else if (Gameworld.snapBoard.LatestReview == 2)
            {
                if (Gameworld.snapBoard.BuildScore > 1500)
                {
                    goodResult = true;
                    response = "This will do just fine.";
                    Gameworld.sounds["averageReview"].Play();
                }
                else
                {
                    goodResult = false;
                    response = "This will do the job, but it could be better...";
                    Gameworld.sounds["averageReview"].Play();
                }

                smiley = Gameworld.sprites["midSmiley"];

            }
            else
            {
                goodResult = false;
                smiley = Gameworld.sprites["badSmiley"];
                response = "What?! This isn't what I ordered at all!";
                Gameworld.sounds["badReview"].Play();
            }
            if (goodResult)
            {
                sprite = Gameworld.sprites["resultBoard1"];
            }
            else
            {
                sprite = Gameworld.sprites["resultBoard2"];
            }
        }
    }
}
