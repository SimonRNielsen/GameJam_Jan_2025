using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameJam_Jan_2025.Robot_bits;

namespace GameJam_Jan_2025
{
    public class SnapBoard : GameObject
    {

        //Vectors for rectangles positions
        private Vector2 headPos;
        private Vector2 torsoPos;
        private Vector2 leftArmPos;
        private Vector2 rightArmPos;
        private Vector2 leftLegPos;
        private Vector2 rightLegPos;
        private Vector2 storagePos1;
        private Vector2 storagePos2;
        private Vector2 storagePos3;

        //Rectangles for collision with parts
        private Rectangle head;
        private Rectangle torso;
        private Rectangle leftArm;
        private Rectangle rightArm;
        private Rectangle leftLeg;
        private Rectangle rightLeg;
        private Rectangle storage1;
        private Rectangle storage2;
        private Rectangle storage3;

        //Reference dictionaries
        public Dictionary<Rectangle, Part> parts = new Dictionary<Rectangle, Part>();
        public Dictionary<Rectangle, Vector2> partsPositions = new Dictionary<Rectangle, Vector2>();

        /// <summary>
        /// Constructor for SnapBoard
        /// </summary>
        public SnapBoard()
        {
            layer = 0.01f;
            scale = 1f;
            AddAssemblyArea();
            //sprite = Gameworld.sprites["snapBoard"];
        }

        /// <summary>
        /// Override of base Update method for custom code
        /// </summary>
        /// <param name="gameTime">Gameworld logic</param>
        /// <param name="screenSize">Huh?</param>
        public override void Update(GameTime gameTime, Vector2 screenSize)
        {

            RobotBuilt(out bool robotBuilt, out bool incompatible);
            if (robotBuilt && !incompatible)
                ClearBench();
            else if (robotBuilt && incompatible)
            {
                //Display incompatible warning
            }

            base.Update(gameTime, screenSize);

        }

        /// <summary>
        /// Adds CollisionBoxes, Positions and references for interaction
        /// </summary>
        private void AddAssemblyArea()
        {

            int size = 200;
            int spacer = 50;
            float robotPosX = 300;
            float robotPosY = 25;
            float storagePosY = 800;

            headPos = new Vector2(robotPosX + (size / 2), robotPosY + (size / 2));
            torsoPos = new Vector2(robotPosX + (size / 2), robotPosY + size + (size / 2) + spacer);
            leftArmPos = new Vector2(robotPosX + size + (size / 2) + spacer, robotPosY + size + (size / 2) + spacer);
            rightArmPos = new Vector2(robotPosX - size + (size / 2) - spacer, robotPosY + size + (size / 2) + spacer);
            leftLegPos = new Vector2(robotPosX + (size / 2) + (size / 2) + (spacer / 2), robotPosY + (size * 2) + (size / 2) + (spacer * 2));
            rightLegPos = new Vector2(robotPosX - (size / 2) + (size / 2) - (spacer / 2), robotPosY + (size * 2) + (size / 2) + (spacer * 2));
            storagePos1 = new Vector2(robotPosX - size + (size / 2) - spacer, storagePosY + (size / 2));
            storagePos2 = new Vector2(robotPosX + (size / 2), storagePosY + (size / 2));
            storagePos3 = new Vector2(robotPosX + size + (size / 2) + spacer, storagePosY + (size / 2));

            head = new Rectangle((int)headPos.X - (size / 2), (int)headPos.Y - (size / 2), size, size);
            torso = new Rectangle((int)torsoPos.X - (size / 2), (int)torsoPos.Y - (size / 2), size, size);
            leftArm = new Rectangle((int)leftArmPos.X - (size / 2), (int)leftArmPos.Y - (size / 2), size, size);
            rightArm = new Rectangle((int)rightArmPos.X - (size / 2), (int)rightArmPos.Y - (size / 2), size, size);
            leftLeg = new Rectangle((int)leftLegPos.X - (size / 2), (int)leftLegPos.Y - (size / 2), size, size);
            rightLeg = new Rectangle((int)rightLegPos.X - (size / 2), (int)rightLegPos.Y - (size / 2), size, size);
            storage1 = new Rectangle((int)storagePos1.X - (size / 2), (int)storagePos1.Y - (size / 2), size, size);
            storage2 = new Rectangle((int)storagePos2.X - (size / 2), (int)storagePos2.Y - (size / 2), size, size);
            storage3 = new Rectangle((int)storagePos3.X - (size / 2), (int)storagePos3.Y - (size / 2), size, size);
            
            parts.Add(head, null);
            parts.Add(torso, null);
            parts.Add(leftArm, null);
            parts.Add(rightArm, null);
            parts.Add(leftLeg, null);
            parts.Add(rightLeg, null);
            parts.Add(storage1, null);
            parts.Add(storage2, null);
            parts.Add(storage3, null);

            partsPositions.Add(head, headPos);
            partsPositions.Add(torso, torsoPos);
            partsPositions.Add(leftArm, leftArmPos);
            partsPositions.Add(rightArm, rightArmPos);
            partsPositions.Add(leftLeg, leftLegPos);
            partsPositions.Add(rightLeg, rightLegPos);
            partsPositions.Add(storage1, storagePos1);
            partsPositions.Add(storage2, storagePos2);
            partsPositions.Add(storage3, storagePos3);

        }

        /// <summary>
        /// Method to update position and index of Part
        /// </summary>
        /// <param name="gameObject">Transfered Part reference</param>
        public bool UpdateSlot(GameObject gameObject)
        {

            bool success = true;
            float distance = 1000;
            Rectangle rectangle = new Rectangle();

            foreach (var partPosition in partsPositions)
            {
                float newDistance = Vector2.Distance(partPosition.Value, gameObject.Position);
                if (distance > newDistance)
                {
                    rectangle = partPosition.Key;
                    distance = newDistance;
                }
            }

            if (parts[rectangle] == null)
            {
                gameObject.Position = partsPositions[rectangle];
                parts[rectangle] = gameObject as Part;
            }
            else
                success = false;

            foreach (var part in parts)
            {
                if (part.Key == rectangle)
                    continue;
                else if (part.Value == gameObject as Part)
                {
                    parts[part.Key] = null;
                }
            }

            return success;
        }

        /// <summary>
        /// Checks if all robot slots are populated
        /// </summary>
        /// <param name="completed">returns true if all building slots are populated</param>
        /// <param name="incompatible">returns false if parts are in the correct slot</param>
        private void RobotBuilt(out bool completed, out bool incompatible)
        {

            completed = false;
            incompatible = true;

            if (parts[head] != null && parts[torso] != null && parts[leftArm] != null && parts[rightArm] != null && parts[leftLeg] != null && parts[rightLeg] != null)
            {

                completed = true;
                incompatible = CompatibilityCheck();

            }

        }

        /// <summary>
        /// Removes parts after the robot is "built"
        /// </summary>
        private void ClearBench()
        {

            parts[head].RemoveThis = true;
            parts[torso].RemoveThis = true;
            parts[leftArm].RemoveThis = true;
            parts[rightArm].RemoveThis = true;
            parts[leftLeg].RemoveThis = true;
            parts[rightLeg].RemoveThis = true;

            parts[head] = null;
            parts[torso] = null;
            parts[leftArm] = null;
            parts[rightArm] = null;
            parts[leftLeg] = null;
            parts[rightLeg] = null;

        }

        /// <summary>
        /// Method to check if the correct parts are in the correct slots
        /// </summary>
        /// <returns>true if parts don't fit together, else false</returns>
        private bool CompatibilityCheck()
        {

            bool incompatible = true;

            if (parts[head] is Head && parts[torso] is Torso && parts[leftArm] is Arm && parts[rightArm] is Arm && parts[leftLeg] is Leg && parts[rightLeg] is Leg)
                incompatible = false;

            return incompatible;

        }

    }

}
