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
    public class Head : Part
    {


        public Head()
        {
            sprite = Gameworld.sprites["head1"];
            position = new Vector2(1000, 1000);
            layer = 1;
        }
    }
}
