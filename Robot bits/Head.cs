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
        


        public Head( Vector2 placement, int robotType)
        {
            position = placement;
            layer = 1;
            scale = 0.5f;

            switch (robotType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["head1"];
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
