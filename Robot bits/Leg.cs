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
    public class Leg : Part
    {
        public Leg(int partType)
        {
            position = Gameworld.startingPosition;
            layer = 1;
            scale = 0.5f;

            switch (partType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["robotLegL1"];
                    break;
                case 2:
                    this.sprite = Gameworld.sprites["robotLegL2"];
                    break;
                case 3:
                    this.sprite = Gameworld.sprites["robotLegL3"];
                    break;
                case 4:
                    this.sprite = Gameworld.sprites["robotLegR1"];
                    break;
                case 5:
                    this.sprite = Gameworld.sprites["robotLegR2"];
                    break;
                case 6:
                    this.sprite = Gameworld.sprites["robotLegR3"];
                    break;
                default:
                    break;

            }
        }

    }
}
