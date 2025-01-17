
namespace GameJam_Jan_2025
{
    internal class TrickPart : Part, ISnapable
    {
        public TrickPart(int partType)
        {
            position = Gameworld.startingPosition;
            scale = 1f;

            switch (partType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["trickPart1"];
                    break;
                case 2:
                    this.sprite = Gameworld.sprites["trickPart2"];
                    break;
                case 3:
                    this.sprite = Gameworld.sprites["trickPart3"];
                    break;
                default:
                    break;

            }
        }
    }
}
