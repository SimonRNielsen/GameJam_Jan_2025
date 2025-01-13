using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameJam_Jan_2025
{
    internal class TestDummy : Part, ISnapable
    {

        public TestDummy(Vector2 position)
        {
            scale = 1f;
            layer = 0.5f;
            this.position = position;
            sprite = Gameworld.sprites["testdummy"];
        }


    }
}
