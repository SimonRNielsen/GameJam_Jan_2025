﻿

namespace GameJam_Jan_2025
{
    internal class CustomerCounter : GameObject
    {

        public bool orderDone;
        //public string[] order = { "baker", "musician", "soldier" };

        public int customer = 1;

        public CustomerCounter()
        {
            if (orderDone)
            {
                customer++;
                orderDone = false;

            }
            


        }

        
        

    }
}
