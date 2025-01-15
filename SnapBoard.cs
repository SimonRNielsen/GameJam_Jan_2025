using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJam_Jan_2025
{
    public class SnapBoard : GameObject
    {

        #region Fields

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
        private Vector2 trashcanPos;

        //Vectors for text placement
        private Vector2 workshopText;
        private Vector2 headText;
        private Vector2 torsoText;
        private Vector2 leftLegText;
        private Vector2 rightLegText;
        private Vector2 leftArmText;
        private Vector2 rightArmText;
        private Vector2 storageText;
        private Vector2 trashcanText;

        //Text strings
        private string displayedTextString;
        private string incompleteTextString = "Robot not completed";
        private string incompatibleTextString = "Not all parts compatible";
        private string readyToBuildTextString = "Robot ready to be built";
        private string robotCompleteTextString = "Robot built ";
        private string scoreTextString;
        private string headTextString = "Head";
        private string torsoTextString = "Torso";
        private string leftArmTextString = "Left arm";
        private string rightArmTextString = "Right arm";
        private string leftLegTextString = "Left leg";
        private string rightLegTextString = "Right leg";
        private string storageTextString = "Mess Box";
        private string trashcanTextString = "Trashcan";

        //Score logic
        private int score;
        private int desiredHead = 1;
        private int desiredTorso = 1;
        private int desiredLeftArm = 1;
        private int desiredRightArm = 1;
        private int desiredLeftLeg = 1;
        private int desiredRightLeg = 1;
        public Timer timer;

        //Timer floats
        private float duration = 5.1f;
        private float displayTime = 5f;

        //Text and rectangle draw effects
        private float customLayer;
        private float textScale = 1.5f;
        Color opaque = Color.Black * 0.2f;
        Color textColor = Color.Black;

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
        private Rectangle trashcan;

        //Reference dictionaries
        public Dictionary<Rectangle, Part> parts = new Dictionary<Rectangle, Part>();
        public Dictionary<Rectangle, Vector2> partsPositions = new Dictionary<Rectangle, Vector2>();

        #endregion
        #region Properties

        /// <summary>
        /// For extracting score when game ends
        /// </summary>
        public int Score { get => score; }

        /// <summary>
        /// To access if something was put in the trashcan
        /// </summary>
        public Rectangle Trashcan { get => trashcan; }

        //Properties for setting target robot PartType values
        public int DesiredHead { set => desiredHead = value; }
        public int DesiredTorso { set => desiredTorso = value; }
        public int DesiredLeftArm { set => desiredLeftArm = value; }
        public int DesiredRightArm { set => desiredRightArm = value; }
        public int DesiredLeftLeg { set => desiredLeftLeg = value; }
        public int DesiredRightLeg { set => desiredRightLeg = value; }

        #endregion
        #region Constructor

        /// <summary>
        /// Constructor for SnapBoard
        /// </summary>
        public SnapBoard()
        {
            layer = 0.01f;
            customLayer = layer + 0.04f;
            scale = 1f;
            AddAssemblyArea();
            SetVectorsAndString();
            //sprite = Gameworld.sprites["background"];
        }

        #endregion
        #region Methods

        /// <summary>
        /// Custom Draw to display certain strings and rectangles
        /// </summary>
        /// <param name="spriteBatch">Gameworld logic</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach (Rectangle rectangle in partsPositions.Keys)
            {
                spriteBatch.Draw(Gameworld.sprites["snapBoard"], new Vector2(partsPositions[rectangle].X - (Gameworld.sprites["snapBoard"].Width / 2), partsPositions[rectangle].Y - (Gameworld.sprites["snapBoard"].Height / 2)), rectangle, opaque, 0f, Vector2.Zero, scale, SpriteEffects.None, layer);
            }
            spriteBatch.DrawString(Gameworld.textFont, displayedTextString, workshopText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, headTextString, headText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, torsoTextString, torsoText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, leftArmTextString, leftArmText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, rightArmTextString, rightArmText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, leftLegTextString, leftLegText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, rightLegTextString, rightLegText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, storageTextString, storageText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, trashcanTextString, trashcanText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            base.Draw(spriteBatch);

        }

        /// <summary>
        /// Override of base Update method for custom code
        /// </summary>
        /// <param name="gameTime">Gameworld logic</param>
        public override void Update(GameTime gameTime)
        {

            duration += (float)gameTime.ElapsedGameTime.TotalSeconds;

            RobotBuilt(out bool robotBuilt, out bool incompatible);
            if (robotBuilt && !incompatible)
            {
                displayedTextString = readyToBuildTextString;
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    duration = 0;
                    ClearBench();
                    timer.ResetTimer();
                }
            }
            else if (robotBuilt && incompatible)
            {
                displayedTextString = incompatibleTextString;
            }
            else
            {
                if (duration >= displayTime)
                    displayedTextString = incompleteTextString;
                else
                {
                    displayedTextString = robotCompleteTextString;
#if DEBUG
                    displayedTextString += scoreTextString;
#endif
                }
            }

            if (parts[trashcan] != null)
            {
                parts[trashcan].RemoveThis = true;
                parts[trashcan] = null;
            }

            workshopText = new Vector2(partsPositions[rightArm].X + 400 - (6 * displayedTextString.Length), partsPositions[rightArm].Y);

            base.Update(gameTime);

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
            leftArmPos = new Vector2(robotPosX - size + (size / 2) - spacer, robotPosY + size + (size / 2) + spacer);
            rightArmPos = new Vector2(robotPosX + size + (size / 2) + spacer, robotPosY + size + (size / 2) + spacer);
            leftLegPos = new Vector2(robotPosX - (spacer / 2), robotPosY + (size * 2) + (size / 2) + (spacer * 2));
            rightLegPos = new Vector2(robotPosX + size + (spacer / 2), robotPosY + (size * 2) + (size / 2) + (spacer * 2));
            storagePos1 = new Vector2(robotPosX - size + (size / 2) - spacer, storagePosY + (size / 2));
            storagePos2 = new Vector2(robotPosX + (size / 2), storagePosY + (size / 2));
            storagePos3 = new Vector2(robotPosX + size + (size / 2) + spacer, storagePosY + (size / 2));
            trashcanPos = new Vector2(robotPosX + (size * 2) + (spacer * 2), robotPosY + (size * 2) + (size / 2) + (spacer * 2));

            head = new Rectangle((int)headPos.X - (size / 2), (int)headPos.Y - (size / 2), size, size);
            torso = new Rectangle((int)torsoPos.X - (size / 2), (int)torsoPos.Y - (size / 2), size, size);
            leftArm = new Rectangle((int)leftArmPos.X - (size / 2), (int)leftArmPos.Y - (size / 2), size, size);
            rightArm = new Rectangle((int)rightArmPos.X - (size / 2), (int)rightArmPos.Y - (size / 2), size, size);
            leftLeg = new Rectangle((int)leftLegPos.X - (size / 2), (int)leftLegPos.Y - (size / 2), size, size);
            rightLeg = new Rectangle((int)rightLegPos.X - (size / 2), (int)rightLegPos.Y - (size / 2), size, size);
            storage1 = new Rectangle((int)storagePos1.X - (size / 2), (int)storagePos1.Y - (size / 2), size, size);
            storage2 = new Rectangle((int)storagePos2.X - (size / 2), (int)storagePos2.Y - (size / 2), size, size);
            storage3 = new Rectangle((int)storagePos3.X - (size / 2), (int)storagePos3.Y - (size / 2), size, size);
            trashcan = new Rectangle((int)trashcanPos.X - (size / 2), (int)trashcanPos.Y - (size / 2), size, size);

            parts.Add(head, null);
            parts.Add(torso, null);
            parts.Add(leftArm, null);
            parts.Add(rightArm, null);
            parts.Add(leftLeg, null);
            parts.Add(rightLeg, null);
            parts.Add(storage1, null);
            parts.Add(storage2, null);
            parts.Add(storage3, null);
            parts.Add(trashcan, null);

            partsPositions.Add(head, headPos);
            partsPositions.Add(torso, torsoPos);
            partsPositions.Add(leftArm, leftArmPos);
            partsPositions.Add(rightArm, rightArmPos);
            partsPositions.Add(leftLeg, leftLegPos);
            partsPositions.Add(rightLeg, rightLegPos);
            partsPositions.Add(storage1, storagePos1);
            partsPositions.Add(storage2, storagePos2);
            partsPositions.Add(storage3, storagePos3);
            partsPositions.Add(trashcan, trashcanPos);

        }

        /// <summary>
        /// Method to set Vectors
        /// </summary>
        private void SetVectorsAndString()
        {

            int textYDisplacement = 110;
            int textXDisplacement = -6;

            headText = new Vector2(partsPositions[head].X + (textXDisplacement * headTextString.Length), partsPositions[head].Y + textYDisplacement);
            torsoText = new Vector2(partsPositions[torso].X + (textXDisplacement * torsoTextString.Length), partsPositions[torso].Y + textYDisplacement);
            leftArmText = new Vector2(partsPositions[leftArm].X + (textXDisplacement * leftArmTextString.Length), partsPositions[leftArm].Y + textYDisplacement);
            rightArmText = new Vector2(partsPositions[rightArm].X + (textXDisplacement * rightArmTextString.Length), partsPositions[rightArm].Y + textYDisplacement);
            leftLegText = new Vector2(partsPositions[leftLeg].X + (textXDisplacement * leftLegTextString.Length), partsPositions[leftLeg].Y + textYDisplacement);
            rightLegText = new Vector2(partsPositions[rightLeg].X + (textXDisplacement * rightLegTextString.Length), partsPositions[rightLeg].Y + textYDisplacement);
            storageText = new Vector2(partsPositions[storage2].X + (textXDisplacement * storageTextString.Length), partsPositions[storage2].Y + textYDisplacement);
            trashcanText = new Vector2(partsPositions[trashcan].X + (textXDisplacement * trashcanTextString.Length), partsPositions[trashcan].Y + textYDisplacement);

        }

        /// <summary>
        /// Method to update position and index of Part
        /// </summary>
        /// <param name="gameObject">Transfered Part reference</param>
        /// <returns>Bool to indicate if action was successful</returns>
        public bool UpdateSlot(GameObject gameObject)
        {

            bool success = true;
            float distance = 1000;
            Rectangle rectangle = new Rectangle();

            //Determines the most likely target for dropped object
            foreach (var partPosition in partsPositions)
            {
                float newDistance = Vector2.Distance(partPosition.Value, gameObject.Position);
                if (distance > newDistance)
                {
                    rectangle = partPosition.Key;
                    distance = newDistance;
                }
            }

            //Determines if the target slot is empty or not
            if (parts[rectangle] == null)
            {
                gameObject.Position = partsPositions[rectangle];
                parts[rectangle] = gameObject as Part;
            }
            else
                success = false;

            //Redirects reference slot for the object
            if (success)
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

            Gameworld.sounds["buildSound"].Play();
            ScoreCalculation();

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

        /// <summary>
        /// Calculates score from assembled robot
        /// </summary>
        private void ScoreCalculation()
        {

            #region Part Orientation evaluation

            int correctHeadOrientation = 1;
            int correctTorsoOrientation = 1;
            int correctLeftArmOrientation = 1;
            int correctRightArmOrientation = 1;
            int correctLeftLegOrientation = 1;
            int correctRightLegOrientation = 1;

            switch (parts[head].Rotation)
            {
                case 0:
                    correctHeadOrientation = 10;
                    break;
                case MathHelper.Pi / 2:
                    correctHeadOrientation = 5;
                    break;
                case MathHelper.Pi:
                    correctHeadOrientation = 1;
                    break;
                case (MathHelper.Pi / 2) * 3:
                    correctHeadOrientation = 5;
                    break;
                default:
                    correctHeadOrientation = 3;
                    break;
            }

            switch (parts[torso].Rotation)
            {
                case 0:
                    correctTorsoOrientation = 10;
                    break;
                case MathHelper.Pi / 2:
                    correctTorsoOrientation = 5;
                    break;
                case MathHelper.Pi:
                    correctTorsoOrientation = 1;
                    break;
                case (MathHelper.Pi / 2) * 3:
                    correctTorsoOrientation = 5;
                    break;
                default:
                    correctTorsoOrientation = 3;
                    break;
            }

            switch (parts[leftArm].Rotation)
            {
                case 0:
                    if (parts[leftArm].Sprite.Name.Contains("armL"))
                        correctLeftArmOrientation = 10;
                    else
                        correctLeftArmOrientation = 1;
                    break;
                case MathHelper.Pi / 2:
                    correctLeftArmOrientation = 5;
                    break;
                case MathHelper.Pi:
                    if (parts[leftArm].Sprite.Name.Contains("armL"))
                        correctLeftArmOrientation = 1;
                    else
                        correctLeftArmOrientation = 10;
                    break;
                case (MathHelper.Pi / 2) * 3:
                    correctLeftArmOrientation = 5;
                    break;
                default:
                    correctLeftArmOrientation = 3;
                    break;
            }

            switch (parts[rightArm].Rotation)
            {
                case 0:
                    if (parts[rightArm].Sprite.Name.Contains("armR"))
                        correctRightArmOrientation = 10;
                    else
                        correctRightArmOrientation = 1;
                    break;
                case MathHelper.Pi / 2:
                    correctRightArmOrientation = 5;
                    break;
                case MathHelper.Pi:
                    if (parts[rightArm].Sprite.Name.Contains("armR"))
                        correctRightArmOrientation = 1;
                    else
                        correctRightArmOrientation = 10;
                    break;
                case (MathHelper.Pi / 2) * 3:
                    correctRightArmOrientation = 5;
                    break;
                default:
                    correctRightArmOrientation = 3;
                    break;
            }

            switch (parts[leftLeg].Rotation)
            {
                case 0:
                    if (parts[leftLeg].Sprite.Name.Contains("legL"))
                        correctLeftLegOrientation = 10;
                    else
                        correctLeftLegOrientation = 1;
                    break;
                case MathHelper.Pi / 2:
                    correctLeftLegOrientation = 5;
                    break;
                case MathHelper.Pi:
                    if (parts[leftLeg].Sprite.Name.Contains("legL"))
                        correctLeftLegOrientation = 1;
                    else
                        correctLeftLegOrientation = 10;
                    break;
                case (MathHelper.Pi / 2) * 3:
                    correctLeftLegOrientation = 5;
                    break;
                default:
                    correctLeftLegOrientation = 3;
                    break;
            }

            switch (parts[rightLeg].Rotation)
            {
                case 0:
                    if (parts[rightLeg].Sprite.Name.Contains("legR"))
                        correctRightLegOrientation = 10;
                    else
                        correctRightLegOrientation = 1;
                    break;
                case MathHelper.Pi / 2:
                    correctRightLegOrientation = 5;
                    break;
                case MathHelper.Pi:
                    if (parts[rightLeg].Sprite.Name.Contains("legR"))
                        correctRightLegOrientation = 1;
                    else
                        correctRightLegOrientation = 10;
                    break;
                case (MathHelper.Pi / 2) * 3:
                    correctRightLegOrientation = 5;
                    break;
                default:
                    correctRightLegOrientation = 3;
                    break;
            }

            #endregion
            #region PartType comparison

            int correctHeadType = 1;
            int correctTorsoType = 1;
            int correctLeftArmType = 1;
            int correctRightArmType = 1;
            int correctLeftLegType = 1;
            int correctRightLegType = 1;

            if (desiredHead == parts[head].PartType)
                correctHeadType = 10;
            else
                correctHeadType = 5;

            if (desiredTorso == parts[torso].PartType)
                correctTorsoType = 10;
            else
                correctTorsoType = 5;

            if (desiredLeftArm == parts[leftArm].PartType)
                correctLeftArmType = 10;
            else
                correctLeftArmType = 5;

            if (desiredRightArm == parts[rightArm].PartType)
                correctRightArmType = 10;
            else
                correctRightArmType = 5;

            if (desiredLeftLeg == parts[leftLeg].PartType)
                correctLeftLegType = 10;
            else
                correctLeftLegType = 5;

            if (desiredRightLeg == parts[rightLeg].PartType)
                correctRightLegType = 10;
            else
                correctRightLegType = 5;

            #endregion

            if ((int)timer.Countdown < 1)
                score += 1 * (correctHeadType + correctHeadOrientation + correctTorsoType + correctTorsoOrientation + correctLeftArmType + correctLeftArmOrientation + correctRightArmType + correctRightArmOrientation + correctLeftLegType + correctLeftLegOrientation + correctRightLegType + correctRightLegOrientation);
            else
                score += (int)timer.Countdown * (correctHeadType + correctHeadOrientation + correctTorsoType + correctTorsoOrientation + correctLeftArmType + correctLeftArmOrientation + correctRightArmType + correctRightArmOrientation + correctLeftLegType + correctLeftLegOrientation + correctRightLegType + correctRightLegOrientation);
            scoreTextString = $"{Score}";

        }

        #endregion

    }

}
