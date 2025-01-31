﻿

namespace GameJam_Jan_2025
{
    public class Leg : Part, ISnapable
    {
        public Leg(int partType)
        {
            position = Gameworld.startingPosition;
            scale = 1f;
            switch (partType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["robotLegL1"];
                    this.partType = 1;
                    break;
                case 2:
                    this.sprite = Gameworld.sprites["robotLegL2"];
                    this.partType = 2;
                    break;
                case 3:
                    this.sprite = Gameworld.sprites["robotLegL3"];
                    this.partType = 3;
                    break;
                case 4:
                    this.sprite = Gameworld.sprites["robotLegR1"];
                    this.partType = 1;
                    break;
                case 5:
                    this.sprite = Gameworld.sprites["robotLegR2"];
                    this.partType = 2;
                    break;
                case 6:
                    this.sprite = Gameworld.sprites["robotLegR3"];
                    this.partType = 3;
                    break;
                default:
                    break;

            }
        }

    }
}
