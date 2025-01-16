using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam_Jan_2025
{
    public class StartOrderAndEndResultsBoard:GameObject
    {
        private string order1="Yes, robot please";
        private string order2="Text goes here";
        private string order3 = "Text goes here";
        //text for the end of the game
        private string loseText = "Text goes here";
        private string winText = "Text goes here";
        private int customer;
        private Texture2D[] sprites = new Texture2D[5];
        private int spriteNumber=1;
        private float timeBetweenImages = 1;
        private float countdown;
        private bool imagesDone = false;
        private bool playOnce = true;
        private Vector2 textPosition = new Vector2(-850,-450);
        //change this
        private Vector2 buttonPosition = new Vector2(400, 600);

        public string Order { get; private set; }

        public StartOrderAndEndResultsBoard(int orderNumber) 
        { 
            customer = orderNumber;
        }

        public override void LoadContent(ContentManager content)
        {
            position = new Vector2(960, 540);
            layer = 0.9f;
            sprites[0] = Gameworld.sprites["enterCutscene1"];
            sprites[1] = Gameworld.sprites["enterCutscene2"];
            sprites[2] = Gameworld.sprites["enterCutscene3"];
            sprites[3] = Gameworld.sprites["enterCutscene4"];
            sprites[4] = Gameworld.sprites["orderCutscene"];

            sprite=sprites[0];
            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (!imagesDone)
            {
                countdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (countdown <= 0)
                {
                    switch (spriteNumber)
                    {
                        case 0:
                            countdown = timeBetweenImages;

                            break;
                        case 1:
                            countdown = timeBetweenImages;
                            sprite = sprites[1];
                            break;
                        case 2:
                            countdown = timeBetweenImages;
                            sprite = sprites[2];
                            break;
                        case 3:
                            countdown = timeBetweenImages;
                            sprite = sprites[3];
                            break;
                        case 4:
                            countdown = 0;
                            sprite = sprites[4];
                            if (playOnce)
                            {
                                playOnce = false;
                                Gameworld.sounds["ding"].Play();
                            }
                            imagesDone = true;
                            Button btn = new Button(false,this);
                            btn.Position = buttonPosition;
                            Gameworld.AddGameObject(btn);
                            break;
                        default:
                            break;
                            
                    }
                    spriteNumber++;
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (imagesDone)
            {
                string order="";
                switch (customer)
                {
                    case 1:
                        order = order1;
                        Order = order1;
                        break;
                    case 2: 
                        order = order2;
                        Order = order2;
                        break;
                    case 3:
                        order = order3;
                        Order = order3;
                        break;
                }
                spriteBatch.DrawString(Gameworld.textFont, ("Order:\n\n" + order), new Vector2(position.X + textPosition.X, position.Y + textPosition.Y), Color.Black, 0, Vector2.Zero, scale * 2, SpriteEffects.None, 1);
            }
            base.Draw(spriteBatch);
        }
    }
}
