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
    public class Torso : Part, ISnapable
    {
        public Torso(int partType)
        {

            position = Gameworld.startingPosition;
            scale = 1f;
            this.partType = partType;
            switch (partType)
            {
                case 1:
                    this.sprite = Gameworld.sprites["robotBody1"];
                    
                    break;
                case 2:
                    this.sprite = Gameworld.sprites["robotBody2"];
                    
                    break;
                case 3:
                    this.sprite = Gameworld.sprites["robotBody3"];
                    
                    break;
                default:
                    break;

            }
        }
    }
}
