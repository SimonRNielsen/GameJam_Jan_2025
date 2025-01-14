using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

        //Reference dictionaries and list
        public List<Rectangle> Slots = new List<Rectangle>();
        public Dictionary<Rectangle, Part> Parts = new Dictionary<Rectangle, Part>();
        public Dictionary<Rectangle, Vector2> PartsPositions = new Dictionary<Rectangle, Vector2>();

        /// <summary>
        /// Constructor for SnapBoard
        /// </summary>
        public SnapBoard()
        {
            AddAssemblyArea();
            /* Sprite not added yet
            sprite = Gameworld.sprites["snapBoard"];
            */
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
                ClearBench(robotBuilt);
            else if (robotBuilt && incompatible)
            {

            }
            base.Update(gameTime, screenSize);

        }

        /// <summary>
        /// Adds CollisionBoxes and references for interaction
        /// </summary>
        private void AddAssemblyArea()
        {

            int size = 256;

            float robotPosX = 300;
            float robotPosY = 10;
            float storagePosY = 800;

            headPos = new Vector2(robotPosX + (size / 2), robotPosY + (size / 2));
            torsoPos = new Vector2(robotPosX + (size / 2), robotPosY + size + (size / 2));
            leftArmPos = new Vector2(robotPosX + size + (size / 2), robotPosY + size + (size / 2));
            rightArmPos = new Vector2(robotPosX - size + (size / 2), robotPosY + size + (size / 2));
            leftLegPos = new Vector2(robotPosX + (size / 2) + (size / 2), robotPosY + (size * 2) + (size / 2));
            rightLegPos = new Vector2(robotPosX - (size / 2) + (size / 2), robotPosY + (size * 2) + (size / 2));
            storagePos1 = new Vector2(robotPosX - size + (size / 2), storagePosY + (size / 2));
            storagePos2 = new Vector2(robotPosX + (size / 2), storagePosY + (size / 2));
            storagePos3 = new Vector2(robotPosX + size + (size / 2), storagePosY + (size / 2));

            head = new Rectangle((int)headPos.X - (size / 2), (int)headPos.Y - (size / 2), size, size);
            torso = new Rectangle((int)torsoPos.X - (size / 2), (int)torsoPos.Y - (size / 2), size, size);
            leftArm = new Rectangle((int)leftArmPos.X - (size / 2), (int)leftArmPos.Y - (size / 2), size, size);
            rightArm = new Rectangle((int)rightArmPos.X - (size / 2), (int)rightArmPos.Y - (size / 2), size, size);
            leftLeg = new Rectangle((int)leftLegPos.X - (size / 2), (int)leftLegPos.Y - (size / 2), size, size);
            rightLeg = new Rectangle((int)rightLegPos.X - (size / 2), (int)rightLegPos.Y - (size / 2), size, size);
            storage1 = new Rectangle((int)storagePos1.X - (size / 2), (int)storagePos1.Y - (size / 2), size, size);
            storage2 = new Rectangle((int)storagePos2.X - (size / 2), (int)storagePos2.Y - (size / 2), size, size);
            storage3 = new Rectangle((int)storagePos3.X - (size / 2), (int)storagePos3.Y - (size / 2), size, size);

            Slots.Add(head);
            Slots.Add(torso);
            Slots.Add(leftArm);
            Slots.Add(rightArm);
            Slots.Add(leftLeg);
            Slots.Add(rightLeg);
            Slots.Add(storage1);
            Slots.Add(storage2);
            Slots.Add(storage3);

            Parts.Add(head, null);
            Parts.Add(torso, null);
            Parts.Add(leftArm, null);
            Parts.Add(rightArm, null);
            Parts.Add(leftLeg, null);
            Parts.Add(rightLeg, null);
            Parts.Add(storage1, null);
            Parts.Add(storage2, null);
            Parts.Add(storage3, null);

            PartsPositions.Add(head, headPos);
            PartsPositions.Add(torso, torsoPos);
            PartsPositions.Add(leftArm, leftArmPos);
            PartsPositions.Add(rightArm, rightArmPos);
            PartsPositions.Add(leftLeg, leftLegPos);
            PartsPositions.Add(rightLeg, rightLegPos);
            PartsPositions.Add(storage1, storagePos1);
            PartsPositions.Add(storage2, storagePos2);
            PartsPositions.Add(storage3, storagePos3);

        }

        /// <summary>
        /// Method to update position and index of Part
        /// </summary>
        /// <param name="gameObject">Transfered Part reference</param>
        public void UpdateSlot(GameObject gameObject)
        {
            float distance = 1000;
            Rectangle rectangle = new Rectangle();

            foreach (var partPosition in PartsPositions)
            {
                float newDistance = Vector2.Distance(partPosition.Value, gameObject.Position);
                if (distance > newDistance)
                {
                    rectangle = partPosition.Key;
                    distance = newDistance;
                }
            }

            gameObject.Position = PartsPositions[rectangle];
            Parts[rectangle] = gameObject as Part;

            foreach (var part in Parts)
            {
                if (part.Key == rectangle)
                    continue;
                else if (part.Value == gameObject as Part)
                {
                    Parts[part.Key] = null;
                }
            }
        }


        private void RobotBuilt(out bool completed, out bool incompatible)
        {
            completed = false;
            incompatible = true;

            if (Parts[head] != null && Parts[torso] != null && Parts[leftArm] != null && Parts[rightArm] != null && Parts[leftLeg] != null && Parts[rightLeg] != null)
            {

                completed = true;

                incompatible = false;

            }

        }


        private void ClearBench(bool robotBuilt)
        {

            Parts[head].RemoveThis = true;
            Parts[torso].RemoveThis = true;
            Parts[leftArm].RemoveThis = true;
            Parts[rightArm].RemoveThis = true;
            Parts[leftLeg].RemoveThis = true;
            Parts[rightLeg].RemoveThis = true;

            Parts[head] = null;
            Parts[torso] = null;
            Parts[leftArm] = null;
            Parts[rightArm] = null;
            Parts[leftLeg] = null;
            Parts[rightLeg] = null;

        }

    }

}
