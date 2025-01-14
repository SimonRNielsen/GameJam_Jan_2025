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

        private Part part1;
        private Part part2;
        private Part part3;

        private int speed=2;

        private Vector2 objectPos;
        int chainNumber;
        //Properties
        public Part Part1 { get => part1; set => part1 = value; }
        public Part Part2 { get => part2; set => part2 = value; }
        public Part Part3 { get => part3; set => part3 = value; }
        //Constructors
        public ConveyorBelt(Vector2 position) 
        { 
            chainNumber = 0;
            Position = position;
            objectPos = Position;
            sprite = Gameworld.sprites["conveyorBelt"];
        }

        /// <summary>
        /// Instantiates on its own when a conveyor belt is created, should not be instantiated seperately
        /// </summary>
        /// <param name="position"></param>
        /// <param name="numberInChain"></param>
        public ConveyorBelt(Vector2 position,int numberInChain)
        {
            chainNumber = numberInChain;
            objectPos = position;
            sprite = Gameworld.sprites["conveyorBelt"];
        }


        //Methods
        public override void LoadContent(ContentManager content)
        {
            itemPos1 = new Vector2(20, 30);
            itemPos2 = new Vector2(70, 80);
            itemPos3 = new Vector2(50, 120);

            
            scale = 0.8f;
            if (chainNumber == 0)
            {
                Gameworld.AddGameObject(new ConveyorBelt(position,chainNumber + 1));
            }
            else
            {
                Position = new Vector2(objectPos.X, objectPos.Y - (sprite.Height * scale * chainNumber));
            }
            base.LoadContent(content);
        }
        public override void Update(GameTime gameTime)
        {
            Position = new Vector2(position.X, position.Y + speed);
            if (Position.Y >= objectPos.Y + (sprite.Height * scale)) 
            {
                float difference=objectPos.Y + (sprite.Height * scale) - Position.Y;
                Position = new Vector2(objectPos.X, objectPos.Y - (sprite.Height * scale)+difference);
            }
            base.Update(gameTime);
        }
    }
}
