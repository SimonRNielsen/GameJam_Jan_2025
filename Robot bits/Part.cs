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
    public abstract class Part:GameObject
    {

        protected int partType;

        /// <summary>
        /// For identifying part type for score
        /// </summary>
        public int PartType { get => partType; }

        public Part() { }

        //Methods
        public override void LoadContent(ContentManager content)
        {

        }
        public override void Update(GameTime gameTime)
        {

        }
    }
}
