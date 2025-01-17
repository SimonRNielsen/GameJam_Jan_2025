

namespace GameJam_Jan_2025
{
    public class Head : Part, ISnapable
    {
        


        public Head(int partType)
        {
            position = Gameworld.startingPosition;
            scale = 1f;
            this.partType = partType;
            switch (partType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["head1"];
                    this.partType = partType;
                    if (Gameworld.order == "baker")
                    {
                        //foreach (Part.Baker Enum in )
                        //{

                        //}
                    }
                    break;
                case 2:
                    this.sprite = Gameworld.sprites["head2"];
                    break;
                case 3:
                    this.sprite = Gameworld.sprites["head3"];
                    break;
                default:
                    break;

            }
        }

        
    }
}
