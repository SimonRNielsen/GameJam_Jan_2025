using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameJam_Jan_2025
{
    public class ConveyorBelt : GameObject
    {
        //Fields
        private Vector2 itemPos1;
        private Vector2 itemPos2;
        private Vector2 itemPos3;
        //Properties
        //Constructors
        public ConveyorBelt() { }
        //Methods
        public override void LoadContent(ContentManager content)
        {

            base.LoadContent(content);
        }
    }
}
