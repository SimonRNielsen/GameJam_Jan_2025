﻿using Microsoft.Xna.Framework.Content;
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
        


        public Head(int partType, Enum RobotType)
        {
            position = Gameworld.startingPosition;
            layer = 1;
            scale = 0.5f;

            switch (partType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["head1"];
                    if (Gameworld.order == "baker")
                    {
                        foreach (Part.Baker Enum in )
                        {

                        }
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
