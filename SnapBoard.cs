using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private Vector2 reviewCounters;

        //Text strings
        private string displayedTextString;
        private string incompleteTextString = "Robot not completed";
        private string incompatibleTextString = "Not all parts compatible";
        private string readyToBuildTextString = "Robot ready to be built";
        private string robotCompleteTextString = "Robot built "; //Extra space is not an error
        private string scoreTextString;

        //Score logic
        private static int goodReview;
        private static int averageReview;
        private static int badReview;
        private byte latestReview;
        private int buildScore;
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
        private float textScale = 1.6f;
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

        //(unneccesary) bool to check when finish button is pressed
        private static bool stillBuilding = true;
        #endregion
        #region Properties

        /// <summary>
        /// For extracting score when game ends
        /// </summary>
        public int Score { get => score; }

        /// <summary>
        /// Gets the score from the last build
        /// </summary>
        public int BuildScore { get => buildScore; }

        /// <summary>
        /// For exporting what last review was, if necessary
        /// </summary>
        public byte LatestReview { get => latestReview; }

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
        public static int GoodReview { get => goodReview; private set => goodReview = value; }
        public static int AverageReview { get => averageReview; private set => averageReview = value; }
        public static int BadReview { get => badReview; private set => badReview = value; }

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
            //sprite = Gameworld.sprites["snapBoardBackground"];
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
                if (rectangle != trashcan)
                {
                    spriteBatch.Draw(Gameworld.sprites["snapBoard"], new Vector2(partsPositions[rectangle].X - (Gameworld.sprites["snapBoard"].Width / 2), partsPositions[rectangle].Y - (Gameworld.sprites["snapBoard"].Height / 2)), rectangle, opaque, 0f, Vector2.Zero, scale, SpriteEffects.None, layer);
                }
                else
                {
                    spriteBatch.Draw(Gameworld.sprites["trashcan"], new Vector2(partsPositions[rectangle].X - (Gameworld.sprites["trashcan"].Width / 2), partsPositions[rectangle].Y - (Gameworld.sprites["trashcan"].Height / 2)), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layer);
                }
                
            }
            spriteBatch.DrawString(Gameworld.textFont, displayedTextString, workshopText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, "Head", headText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, "Torso", torsoText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, "Left arm", leftArmText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, "Right arm", rightArmText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, "Left leg", leftLegText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, "Right leg", rightLegText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, "Mess box", storageText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, "Trashcan", trashcanText, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
            spriteBatch.DrawString(Gameworld.textFont, $"Good reviews: {goodReview}\nAverage reviews: {averageReview}\nBad reviews: {badReview}", reviewCounters, textColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, customLayer);
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
                Button.ActivateBtn(true);
                if (!stillBuilding)
                {
                    duration = 0;
                    ClearBench();
                    stillBuilding = true;
                }
            }
            else if (robotBuilt && incompatible)
            {
                Button.ActivateBtn(false);
                displayedTextString = incompatibleTextString;
            }
            else
            {
                Button.ActivateBtn(false);
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
        /// Method to set Vectors and string
        /// </summary>
        private void SetVectorsAndString()
        {

            int textYDisplacement = 110;
            int textXDisplacement = -6;

            headText = new Vector2(partsPositions[head].X + (textXDisplacement * 4), partsPositions[head].Y + textYDisplacement);
            torsoText = new Vector2(partsPositions[torso].X + (textXDisplacement * 5), partsPositions[torso].Y + textYDisplacement);
            leftArmText = new Vector2(partsPositions[leftArm].X + (textXDisplacement * 8), partsPositions[leftArm].Y + textYDisplacement);
            rightArmText = new Vector2(partsPositions[rightArm].X + (textXDisplacement * 9), partsPositions[rightArm].Y + textYDisplacement);
            leftLegText = new Vector2(partsPositions[leftLeg].X + (textXDisplacement * 8), partsPositions[leftLeg].Y + textYDisplacement);
            rightLegText = new Vector2(partsPositions[rightLeg].X + (textXDisplacement * 9), partsPositions[rightLeg].Y + textYDisplacement);
            storageText = new Vector2(partsPositions[storage2].X + (textXDisplacement * 8), partsPositions[storage2].Y + textYDisplacement);
            trashcanText = new Vector2(partsPositions[trashcan].X + (textXDisplacement * 8), partsPositions[trashcan].Y + textYDisplacement);
            reviewCounters = new Vector2(partsPositions[rightArm].X + 400 - (6 * 13), partsPositions[rightArm].Y + 200);

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

            if (CheckSlots())
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

        /// <summary>
        /// Calculates score from assembled robot
        /// </summary>
        public void ScoreCalculation()
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
                    if (parts[leftArm].Sprite.Name.Contains("armR"))
                        correctLeftArmOrientation = 10;
                    else
                        correctLeftArmOrientation = 1;
                    break;
                case MathHelper.Pi / 2:
                    correctLeftArmOrientation = 5;
                    break;
                case MathHelper.Pi:
                    if (parts[leftArm].Sprite.Name.Contains("armR"))
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
                    if (parts[rightArm].Sprite.Name.Contains("armL"))
                        correctRightArmOrientation = 10;
                    else
                        correctRightArmOrientation = 1;
                    break;
                case MathHelper.Pi / 2:
                    correctRightArmOrientation = 5;
                    break;
                case MathHelper.Pi:
                    if (parts[rightArm].Sprite.Name.Contains("armL"))
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
                    if (parts[leftLeg].Sprite.Name.Contains("legR"))
                        correctLeftLegOrientation = 10;
                    else
                        correctLeftLegOrientation = 1;
                    break;
                case MathHelper.Pi / 2:
                    correctLeftLegOrientation = 5;
                    break;
                case MathHelper.Pi:
                    if (parts[leftLeg].Sprite.Name.Contains("legR"))
                        correctLeftLegOrientation = 1;
                    else
                    {
                        if (parts[leftLeg].PartType == desiredLeftLeg)
                            correctLeftLegOrientation = 10;
                        else
                            correctLeftLegOrientation = 1;
                    }
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
                    if (parts[rightLeg].Sprite.Name.Contains("legL"))
                        correctRightLegOrientation = 10;
                    else
                        correctRightLegOrientation = 1;
                    break;
                case MathHelper.Pi / 2:
                    correctRightLegOrientation = 5;
                    break;
                case MathHelper.Pi:
                    if (parts[rightLeg].Sprite.Name.Contains("legL"))
                        correctRightLegOrientation = 1;
                    else
                    {
                        if (parts[rightLeg].PartType == desiredRightLeg)
                            correctRightLegOrientation = 10;
                        else
                            correctRightLegOrientation = 1;
                    }
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

            latestReview = RobotReview(correctHeadType, correctTorsoType, correctLeftArmType, correctRightArmType, correctLeftLegType, correctRightLegType, correctHeadOrientation, correctTorsoOrientation, correctLeftArmOrientation, correctRightArmOrientation, correctLeftLegOrientation, correctRightLegOrientation);

            switch (latestReview)
            {
                case 1:
                    goodReview++;
                    break;
                case 2:
                    averageReview++;
                    break;
                case 3:
                    badReview++;
                    break;
            }

            buildScore = MathHelper.Max(1, (int)timer.Countdown) * (correctHeadType + correctHeadOrientation + correctTorsoType + correctTorsoOrientation + correctLeftArmType + correctLeftArmOrientation + correctRightArmType + correctRightArmOrientation + correctLeftLegType + correctLeftLegOrientation + correctRightLegType + correctRightLegOrientation);
            score += buildScore;
            scoreTextString = $"{score}";

        }

        public static void FinishUp()
        {
            stillBuilding = false;
        }
        /// <summary>
        /// Assertains how many "errors" have been made in the building process, with a max of 12 assessment-points
        /// </summary>
        /// <param name="headType">Heads type</param>
        /// <param name="torsoType">Torsos type</param>
        /// <param name="leftArmType">Left arms type</param>
        /// <param name="rightArmType">Right arms type</param>
        /// <param name="leftLegType">Left legs type</param>
        /// <param name="rightLegType">Right legs type</param>
        /// <param name="headOrientation">Heads rotation</param>
        /// <param name="torsoOrientation">Torsos rotation</param>
        /// <param name="leftArmOrientation">Left arms rotation</param>
        /// <param name="rightArmOrientation">Right arms rotation</param>
        /// <param name="leftLegOrientation">Left legs rotation</param>
        /// <param name="rightLegOrientation">right legs rotation</param>
        /// <returns>1 (1 or less errors), 2 (2-5 errors) or 3 (more than 5 errors)</returns>
        private byte RobotReview(int headType, int torsoType, int leftArmType, int rightArmType, int leftLegType, int rightLegType, int headOrientation, int torsoOrientation, int leftArmOrientation, int rightArmOrientation, int leftLegOrientation, int rightLegOrientation)
        {
            byte review;
            int errors = 0;
            int[] errorpoints = new int[12] { headType, torsoType, leftArmType, rightArmType, leftLegType, rightLegType, headOrientation, torsoOrientation, leftArmOrientation, rightArmOrientation, leftLegOrientation, rightLegOrientation};

            foreach (int i in errorpoints)
            {
                if (i < 10)
                    errors++;
            }

            switch (errors)
            {
                case 0:
                case 1:
                    review = 1;
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    review = 2;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    review = 3;
                    break;
                default:
                    review = 2;
                    break;
            }

            return review;
        }

        /// <summary>
        /// Bool to check if all slots are filled with correct items
        /// </summary>
        /// <returns>true if all slots are filled, else false</returns>
        public bool CheckSlots()
        {

            bool checkSlots = false;
            if (parts[head] != null && parts[torso] != null && parts[leftArm] != null && parts[rightArm] != null && parts[leftLeg] != null && parts[rightLeg] != null)
                checkSlots = true;
            return checkSlots;

        }

        #endregion

    }

}
