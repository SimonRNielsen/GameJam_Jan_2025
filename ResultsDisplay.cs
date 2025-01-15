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
    public class ResultsDisplay:GameObject
    {
        private bool goodResult;
        private int points;
        private bool finishedInTime;
        private int midRanking = 10;
        private int goodRanking = 20;
        private Texture2D smiley;
        private string response;
        public ResultsDisplay(bool finishedInTime,int points) 
        { 
            this.finishedInTime = finishedInTime;
            this.points = points;
            layer = 0.9f;
            position = new Vector2(960, 540);
            RateResult();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(Gameworld.textFont, response+"\n\nPoints: "+points, position, Color.Black,null,Vector2.Zero,scale,SpriteEffects.None,layer+0.1f);
            spriteBatch.Draw(smiley, new Vector2(position.X+200,position.Y-50), null, Color.White, 0, Vector2.Zero, scale*0.8f, SpriteEffects.None, layer + 0.1f);
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
            }
            else if (points >= goodRanking)
            {
                goodResult= true;
                smiley = Gameworld.sprites["goodSmiley"];
                response = "Amazing! It's better than I imagined!";
            }
            else if (points >= midRanking)
            {
                if (points >= (midRanking + (goodRanking - midRanking) / 2))
                {
                    goodResult = true;
                    response = "This will do just fine.";
                }
                else
                {
                    goodResult = false;
                    response = "This will do the job, but it could be better...";
                }

                smiley = Gameworld.sprites["midSmiley"];
                
            }
            else 
            {
                goodResult = false;
                smiley = Gameworld.sprites["badSmiley"];
                response = "What?! This isn't what I ordered at all!";
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
