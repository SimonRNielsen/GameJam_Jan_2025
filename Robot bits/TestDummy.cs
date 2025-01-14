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

        /// <summary>
        /// Dummy for testing Drag n' drop
        /// </summary>
        /// <param name="position">Starting location of item</param>
        public TestDummy(Vector2 position)
        {
            scale = 1f;
            layer = 0.5f;
            this.position = position;
            sprite = Gameworld.sprites["testdummy"];
        }

    }
}
