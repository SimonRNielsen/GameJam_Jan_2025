﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam_Jan_2025
{
    public class StartOrderAndEndResultsBoard:GameObject
    {
        private string order1 = "Our head chef just quit \nlast minute and we need a \nreplacement, fast!";
        private string order2 = "I need to prototype a new \ntype of cembat drone. \nGot anything that might \ndo the job?";
        private string order3 = "The band just can't seem \nto get their act together. \nAny chance your bots \nhave musical talent?";
        //text for the end of the game
        private string loseText = "Your store was shut down, due to bad reviews...";
        private string winText = "You are amazing, and your robot building business will be known for years!";
        private int customer;
        private Texture2D[] sprites = new Texture2D[5];
        private int spriteNumber=0;
        private float timeBetweenImages = 1;
        private float countdown;
        private bool imagesDone = false;
        private int finalScore;
        private Vector2 textPosition = new Vector2(-850,-450);
        //change this
        private Vector2 buttonPosition = new Vector2(400, 600);

        public static string Order { get; private set; }

        public StartOrderAndEndResultsBoard(int orderNumber) 
        { 
            customer = orderNumber;
        }

        public override void LoadContent(ContentManager content)
        {
            countdown = timeBetweenImages;
            position = new Vector2(960, 540);
            layer = 0.9f;
            sprites[0] = Gameworld.sprites["enterCutscene1"];
            sprites[1] = Gameworld.sprites["enterCutscene2"];
            sprites[2] = Gameworld.sprites["enterCutscene3"];
            sprites[3] = Gameworld.sprites["enterCutscene4"];
            sprites[4] = Gameworld.sprites["orderCutscene"];
            if (Gameworld.numberOfOrders == 1)
            {
                sprite = sprites[0];

            }
            
            else if (Gameworld.numberOfOrders > 3)
            {
                Button restartBtn = new Button(false, this);
                restartBtn.Position = new Vector2(900, 900);
                Gameworld.AddGameObject(restartBtn);
                finalScore = (SnapBoard.GoodReview * 3) + SnapBoard.AverageReview - (SnapBoard.BadReview * 3);
                if (finalScore > 4)
                {
                    sprite = Gameworld.sprites["winScreen"];
                    spriteNumber = 5;
                    imagesDone = true;
                }
                else
                {
                    sprite = Gameworld.sprites["loseScreen"];
                    spriteNumber = 5;
                    imagesDone = true;
                }
                Gameworld.numberOfOrders = 5;
            }
            else 
            {
                sprite = sprites[1];
                spriteNumber = 1;
            }
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
                if (Gameworld.numberOfOrders<4)
                {
                    string order = "";
                    switch (customer)
                    {
                        case 1:
                            order = order1;
                            Order = order1;
                            DesignateOrder(RobotType.Baker);
                            break;
                        case 2:
                            order = order2;
                            Order = order2;
                            DesignateOrder(RobotType.Soldier);
                            break;
                        case 3:
                            order = order3;
                            Order = order3;
                            DesignateOrder(RobotType.Musician);
                            break;
                    }
                    spriteBatch.DrawString(Gameworld.textFont, ("Order:\n\n" + order), new Vector2(position.X + textPosition.X, position.Y + textPosition.Y), Color.Black, 0, Vector2.Zero, scale * 3, SpriteEffects.None, 1);
                }
                else 
                {
                    if (finalScore > 4)
                    {
                        spriteBatch.DrawString(Gameworld.textFont, winText + "\nFinal score: " + Gameworld.snapBoard.Score, new Vector2(position.X-500, position.Y + 150), Color.Black, 0, Vector2.Zero, scale * 3, SpriteEffects.None, 1);
                    }
                    else
                    {
                        spriteBatch.DrawString(Gameworld.textFont, loseText + "\nFinal score: " + Gameworld.snapBoard.Score, new Vector2(position.X-500, position.Y +150), Color.Black, 0, Vector2.Zero, scale * 3, SpriteEffects.None, 1);
                    }
                }
            }
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Method to change type of robot requested
        /// </summary>
        /// <param name="desiredRobot">Changes parameters dependant on which type of robot is requested</param>
        private void DesignateOrder(RobotType desiredRobot)
        {

            switch (desiredRobot)
            {
                case RobotType.Baker:
                    Gameworld.snapBoard.DesiredHead = 1;
                    Gameworld.snapBoard.DesiredTorso = 1;
                    Gameworld.snapBoard.DesiredLeftArm = 1;
                    Gameworld.snapBoard.DesiredRightArm = 1;
                    Gameworld.snapBoard.DesiredLeftLeg = 1;
                    Gameworld.snapBoard.DesiredRightLeg = 1;
                    break;
                case RobotType.Soldier:
                    Gameworld.snapBoard.DesiredHead = 2;
                    Gameworld.snapBoard.DesiredTorso = 2;
                    Gameworld.snapBoard.DesiredLeftArm = 2;
                    Gameworld.snapBoard.DesiredRightArm = 2;
                    Gameworld.snapBoard.DesiredLeftLeg = 2;
                    Gameworld.snapBoard.DesiredRightLeg = 2;
                    break;
                case RobotType.Musician:
                    Gameworld.snapBoard.DesiredHead = 3;
                    Gameworld.snapBoard.DesiredTorso = 3;
                    Gameworld.snapBoard.DesiredLeftArm = 3;
                    Gameworld.snapBoard.DesiredRightArm = 3;
                    Gameworld.snapBoard.DesiredLeftLeg = 3;
                    Gameworld.snapBoard.DesiredRightLeg = 3;
                    break;
            }

        }
    }
}
