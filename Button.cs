using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam_Jan_2025
{
    public class Button : GameObject
    {
        //Fields
        private bool isFinishBtn;
        private Rectangle hitbox;
        private static bool btnActive = false;
        private GameObject boardToClose;
        private bool alreadyClicked = false;

        //Properties

        //Constructors
        public Button(bool isFinishBtn) 
        {
            this.isFinishBtn = isFinishBtn;
            if (isFinishBtn)
            {
                sprite = Gameworld.sprites["button"];
            }
            else
            {
                //use other button sprite
                sprite = Gameworld.sprites["button"];
            }
            layer = 0.6f;
        }
        public Button(bool isFinishBtn,GameObject boardToClose)
        {
            this.isFinishBtn = isFinishBtn;
            if (isFinishBtn)
            {
                sprite = Gameworld.sprites["button"];
            }
            else
            {
                //use other button sprite
                sprite = Gameworld.sprites["button"];
            }
            layer = 1f;
            this.boardToClose = boardToClose;
        }
        //Methods
        public override void LoadContent(ContentManager content)
        {
            hitbox = new Rectangle((int)(Position.X- (sprite.Width * scale/2)), (int)(Position.Y - (sprite.Height * scale / 2)), (int)(sprite.Width * scale), (int)(sprite.Height * scale));
            base.LoadContent(content);
        }
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Rectangle mouseHitbox = new Rectangle(mouseState.X, mouseState.Y, 2, 2);
            bool isHovering = false;
            bool clicked = false;
            if (mouseHitbox.Intersects(hitbox))
            {
                isHovering = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    clicked = true;
                }
            }
            else
            {
                alreadyClicked = false;
            }
            if (isHovering &&(btnActive||!isFinishBtn))
            {
                color = Color.LightGray;
            }
            else if ( !btnActive&& isFinishBtn)
            {
                color = Color.Gray;
            }
            else
            {
                color = Color.White;
            }

            if (clicked&& btnActive&&isFinishBtn&&!alreadyClicked)
            {
                Gameworld.AddGameObject(new ResultsDisplay(true, Gameworld.snapBoard.BuildScore));
                Timer.Stop();
                alreadyClicked = true;
            }
            else if (clicked&&!isFinishBtn&&!alreadyClicked)
            {
                Timer.ResetTimer();
                if(boardToClose is ResultsDisplay)
                {
                    Gameworld.orderNumber += 1;
                    Gameworld.AddGameObject(new StartOrderAndEndResultsBoard(Gameworld.orderNumber));
                    SnapBoard.FinishUp();
                    Gameworld.RemoveGameObject(boardToClose);
                    Gameworld.RemoveGameObject(this);
                    alreadyClicked = true;
                }
                else 
                {
                    OrderPaper.NewOrder(StartOrderAndEndResultsBoard.Order);
                    
                    Gameworld.RemoveGameObject(boardToClose);
                    Gameworld.RemoveGameObject(this);
                    alreadyClicked=true;
                }
                
            }
            base.Update(gameTime);
        }
        public static void ActivateBtn(bool active)
        {
            btnActive = active;
            
        }
    }
}
