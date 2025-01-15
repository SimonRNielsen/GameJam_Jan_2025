using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace GameJam_Jan_2025
{
    public class ConveyorBelt : GameObject
    {

        //Fields
        private Vector2 itemPos1;
        private Vector2 itemPos2;
        private Vector2 itemPos3;
        private Vector2 itemPos4;
        private Vector2 itemPos5;
        private Vector2 itemPos6;
        private Vector2 itemPos7;
        private Vector2 itemPos8;
        private Vector2 itemPos9;
        private Vector2 itemPos10;


        private Part[] allParts;

        private int speed = 3;

        private static bool stopped = false;

        private Vector2 objectPos;
        int chainNumber;

        private Random rnd = new Random();

        //Properties
        public Part[] AllParts { get => allParts; set => allParts = value; }

        //Constructors
        public ConveyorBelt(Vector2 position)
        {
            scale = 0.8f;
            chainNumber = 1;
            sprite = Gameworld.sprites["conveyorBelt"];
            if (position.Y < 1080 - ((sprite.Height * scale) / 2))
            {
                Position = new Vector2(position.X, 1080 - ((sprite.Height * scale) / 2));
            }
            else
            {
                Position = position;
            }
            objectPos = Position;
        }

        /// <summary>
        /// Instantiates on its own when a conveyor belt is created, should not be instantiated seperately
        /// </summary>
        /// <param name="position"></param>
        /// <param name="numberInChain"></param>
        public ConveyorBelt(Vector2 position, int numberInChain)
        {
            chainNumber = numberInChain;
            objectPos = position;
            sprite = Gameworld.sprites["conveyorBelt"];
        }


        //Methods
        public override void LoadContent(ContentManager content)
        {
            //starting positions of items, will be automatically adjusted if needed
            itemPos1 = new Vector2(20, 200);
            itemPos2 = new Vector2(-100, 280);
            itemPos3 = new Vector2(50, 420);
            itemPos4 = new Vector2(0, 550);
            itemPos5 = new Vector2(-50, 660);
            itemPos6 = new Vector2(170, 750);
            itemPos7 = new Vector2(60, 880);
            itemPos8 = new Vector2(-100, 1050);
            itemPos9 = new Vector2(-20, 1150);
            itemPos10 = new Vector2(300, 1260);

            scale = 0.8f;

            //only one of the conveyors need to make this 
            if (chainNumber == 1)
            {
                //adds the other belts for seamless looping
                Gameworld.AddGameObject(new ConveyorBelt(position, chainNumber + 1));
                Gameworld.AddGameObject(new ConveyorBelt(position, chainNumber + 2));


                allParts = new Part[10];

                //randomize all parts
                for (int i = 0; i < allParts.Length; i++)
                {
                    allParts[i] = RandomizeItem();
                    Gameworld.AddGameObject(allParts[i]);
                }
                //checks whteher the positions are on the conveyor, adjusts them, then sets the parts to those positions
                if (itemPos1.X > (sprite.Width * scale / 2) - 100)
                {
                    itemPos1 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos1.Y);
                }
                else if (itemPos1.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos1.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[0].Position = new Vector2(objectPos.X + itemPos1.X, -itemPos1.Y);

                if (itemPos2.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos2 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos2.Y);
                }
                else if (itemPos2.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos2.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[1].Position = new Vector2(objectPos.X + itemPos2.X, -itemPos2.Y);

                if (itemPos3.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos3 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos3.Y);
                }
                else if (itemPos3.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos3.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[2].Position = new Vector2(objectPos.X + itemPos3.X, -itemPos3.Y);

                if (itemPos4.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos4 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos4.Y);
                }
                else if (itemPos4.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos4.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[3].Position = new Vector2(objectPos.X + itemPos4.X, -itemPos4.Y);

                if (itemPos5.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos5 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos5.Y);
                }
                else if (itemPos5.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos5.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[4].Position = new Vector2(objectPos.X + itemPos5.X, -itemPos5.Y);

                if (itemPos6.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos6 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos6.Y);
                }
                else if (itemPos6.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos6.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[5].Position = new Vector2(objectPos.X + itemPos6.X, -itemPos6.Y);

                if (itemPos7.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos7 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos7.Y);
                }
                else if (itemPos7.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos7.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[6].Position = new Vector2(objectPos.X + itemPos7.X, -itemPos7.Y);

                if (itemPos8.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos8 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos8.Y);
                }
                else if (itemPos8.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos8.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[7].Position = new Vector2(objectPos.X + itemPos8.X, -itemPos8.Y);

                if (itemPos9.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos9 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos9.Y);
                }
                else if (itemPos9.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos9.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[8].Position = new Vector2(objectPos.X + itemPos9.X, -itemPos9.Y);

                if (itemPos10.X > (sprite.Width * scale) / 2 - 100)
                {
                    itemPos10 = new Vector2((sprite.Width * scale) / 2 - 100, itemPos10.Y);
                }
                else if (itemPos10.X < -(sprite.Width * scale / 2) + 100)
                {
                    itemPos10.X = -(sprite.Width * scale / 2) + 100;
                }
                allParts[9].Position = new Vector2(objectPos.X + itemPos10.X, -itemPos10.Y);
            }
            else
            {
                //other conveyors are placed above the main one, in order
                Position = new Vector2(objectPos.X, objectPos.Y - (sprite.Height * scale * (chainNumber - 1)));
            }

            base.LoadContent(content);
        }
        public override void Update(GameTime gameTime)
        {
            if (!stopped)
            {
                //moves conveyor
                Position = new Vector2(position.X, position.Y + speed);

                //if past the bottom of the screen, conveyor is teleported to the top
                if (Position.Y >= 1080 + (sprite.Height * scale / 2))
                {
                    float difference = Position.Y - (1080 + (sprite.Height * scale / 2));
                    Position = new Vector2(objectPos.X, 1080 + (sprite.Height * scale / 2) - (sprite.Height * scale * 2.5f) + difference);
                }
                base.Update(gameTime);

                if (chainNumber == 1)
                {
                    //moves each part
                    for (int i = 0; i < allParts.Length; i++)
                    {
                        if (allParts[i] != null)
                        {

                            allParts[i].Position = new Vector2(allParts[i].Position.X, allParts[i].Position.Y + speed);
                            //teleports part to the top as with conveyor, but rerandomizes them
                            if (allParts[i].Position.Y >= 1180)
                            {
                                allParts[i] = RandomizeItem();
                                Gameworld.AddGameObject(allParts[i]);
                                switch (i)
                                {
                                    case 0:
                                        allParts[i].Position = new Vector2(objectPos.X + itemPos1.X, -itemPos1.Y);
                                        break;
                                    case 1:
                                        allParts[i].Position = new Vector2(objectPos.X + itemPos2.X, -itemPos2.Y);
                                        break;
                                    case 2:
                                        allParts[i].Position = new Vector2(objectPos.X + itemPos3.X, -itemPos3.Y);
                                        break;
                                    case 3:
                                        allParts[i].Position = new Vector2(objectPos.X + itemPos4.X, -itemPos4.Y);
                                        break;
                                    case 4:
                                        allParts[i].Position = new Vector2(objectPos.X + itemPos5.X, -itemPos5.Y);
                                        break;
                                }

                                switch (rnd.Next(1, 5))
                                {
                                    case 2:
                                        allParts[i].Rotation = MathHelper.Pi / 2;
                                        break;
                                    case 3:
                                        allParts[i].Rotation = MathHelper.Pi;
                                        break;
                                    case 4:
                                        allParts[i].Rotation = (MathHelper.Pi / 2) * 3;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                            allParts[i] = RandomPart();
                    }
                }
            }
            
        }

        private Part RandomizeItem()
        {
            switch (rnd.Next(0, 5))
            {
                case 0:
                    return new Arm(rnd.Next(1, 6));
                case 1:
                    return new Head(rnd.Next(1, 4));
                case 2:
                    return new Leg(rnd.Next(1, 6));
                case 3:
                    return new Torso(rnd.Next(1, 4));
                case 4:
                    return new TrickPart(rnd.Next(1, 4));
                default:
                    return null;
            }
        }

        /// <summary>
        /// Locates and removes specified object from AllParts-array
        /// </summary>
        /// <param name="gameObject">Object to compare</param>
        public void RemoveFromAllParts(GameObject gameObject)
        {

            for (int i = 0; i < AllParts.Length; i++)
            {
                if (AllParts[i] == gameObject)
                    AllParts[i] = null;
            }

        }

        /// <summary>
        /// Function to create a new random Part
        /// </summary>
        /// <returns>New random Part</returns>
        private Part RandomPart()
        {

            Part part;

            switch (rnd.Next(1, 8))
            {
                case 1:
                    part = new Head(rnd.Next(1, 4));
                    break;
                case 2:
                    part = new Torso(rnd.Next(1, 4));
                    break;
                case 3:
                case 4:
                    part = new Arm(rnd.Next(1, 7));
                    break;
                case 5:
                case 6:
                    part = new Leg(rnd.Next(1, 7));
                    break;
                default:
                    part = new TrickPart(rnd.Next(1, 4));
                    break;
            }
            switch (rnd.Next(1, 6))
            {
                case 1:
                    part.Position = new Vector2(itemPos1.X + rnd.Next(-20, 21), itemPos1.Y + rnd.Next(-20, 21));
                    break;
                case 2:
                    part.Position = new Vector2(itemPos2.X + rnd.Next(-20, 21), itemPos2.Y + rnd.Next(-20, 21));
                    break;
                case 3:
                    part.Position = new Vector2(itemPos3.X + rnd.Next(-20, 21), itemPos3.Y + rnd.Next(-20, 21));
                    break;
                case 4:
                    part.Position = new Vector2(itemPos4.X + rnd.Next(-20, 21), itemPos4.Y + rnd.Next(-20, 21));
                    break;
                case 5:
                    part.Position = new Vector2(itemPos5.X + rnd.Next(-20, 21), itemPos5.Y + rnd.Next(-20, 21));
                    break;
                default:
                    break;
            }

            return part;
        }
        public static void Stop()
        {
            stopped = true;
        }
        public static void Start()
        {
            stopped = false;
        }
    }
}
