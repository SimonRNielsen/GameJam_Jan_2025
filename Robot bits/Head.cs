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
    public class Head : Part, ISnapable
    {
        


        public Head(int partType)
        {
            position = Gameworld.startingPosition;
            scale = 1f;

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
                    this.partType = partType;
                    break;
                case 3:
                    this.sprite = Gameworld.sprites["head3"];
                    this.partType = partType;
                    break;
                default:
                    break;

            }
        }

        
    }
}
