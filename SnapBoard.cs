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
        public Dictionary<Rectangle, Part> Storage = new Dictionary<Rectangle, Part>();

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

            headPos = new Vector2(robotPosX, robotPosY);
            torsoPos = new Vector2(robotPosX, robotPosY + size);
            leftArmPos = new Vector2(robotPosX + size, robotPosY + size);
            rightArmPos = new Vector2(robotPosX - size, robotPosY + size);
            leftLegPos = new Vector2(robotPosX + (size / 2), robotPosY + (size * 2));
            rightLegPos = new Vector2(robotPosX - (size / 2), robotPosY + (size * 2));
            storagePos1 = new Vector2(robotPosX - size, storagePosY);
            storagePos2 = new Vector2(robotPosX, storagePosY);
            storagePos3 = new Vector2(robotPosX + size, storagePosY);

            head = new Rectangle((int)headPos.X, (int)headPos.Y, size, size);
            torso = new Rectangle((int)torsoPos.X, (int)torsoPos.Y, size, size);
            leftArm = new Rectangle((int)leftArmPos.X, (int)leftArmPos.Y, size, size);
            rightArm = new Rectangle((int)rightArmPos.X, (int)rightArmPos.Y, size, size);
            leftLeg = new Rectangle((int)leftLegPos.X, (int)leftLegPos.Y, size, size);
            rightLeg = new Rectangle((int)rightLegPos.X, (int)rightLegPos.Y, size, size);
            storage1 = new Rectangle((int)storagePos1.X, (int)storagePos1.Y, size, size);
            storage2 = new Rectangle((int)storagePos2.X, (int)storagePos2.Y, size, size);
            storage3 = new Rectangle((int)storagePos3.X, (int)storagePos3.Y, size, size);

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

    }

}
