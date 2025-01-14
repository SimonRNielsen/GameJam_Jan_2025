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
    public class Arm : Part
    {

        public Arm(int partType)

        {
            position = Gameworld.startingPosition;
            layer = 1;
            scale = 0.5f;

            switch (partType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["robotArmL1"];
                    break;
                case 2:
                    this.sprite = Gameworld.sprites["robotArmL2"];
                    break;
                case 3:
                    this.sprite = Gameworld.sprites["robotArmL3"];
                    break;
                case 4:
                    this.sprite = Gameworld.sprites["robotArmR1"];
                    break;
                case 5:
                    this.sprite = Gameworld.sprites["robotArmR2"];
                    break;
                case 6:
                    this.sprite = Gameworld.sprites["robotArmR3"];
                    break;
                default:
                    break;

            }
        }

    }
}
