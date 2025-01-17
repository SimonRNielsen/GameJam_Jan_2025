

namespace GameJam_Jan_2025
{
    public class Arm : Part, ISnapable
    {

        public Arm(int partType)

        {
            position = Gameworld.startingPosition;
            scale = 1f;
            
            switch (partType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["robotArmL1"];
                    this.partType = partType;
                    break;
                case 2:
                    this.sprite = Gameworld.sprites["robotArmL2"];
                    this.partType = partType;
                    break;
                case 3:
                    this.sprite = Gameworld.sprites["robotArmL3"];
                    this.partType = partType;
                    break;
                case 4:
                    this.sprite = Gameworld.sprites["robotArmR1"];
                    this.partType = 1;
                    break;
                case 5:
                    this.sprite = Gameworld.sprites["robotArmR2"];
                    this.partType = 2;
                    break;
                case 6:
                    this.sprite = Gameworld.sprites["robotArmR3"];
                    this.partType = 3;
                    break;
                default:
                    break;

            }
        }

    }
}
