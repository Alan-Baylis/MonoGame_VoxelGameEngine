﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using VoxelRPGGame.MenuSystem;
using VoxelRPGGame.MenuSystem.MenuElements;

using VoxelRPGGame.MenuSystem.Screens;

using VoxelRPGGame.GameEngine.EnvironmentState;
using VoxelRPGGame.GameEngine.UI.Inventory;
using VoxelRPGGame.GameEngine.InventorySystem.Tools;
using VoxelRPGGame.GameEngine.UI.Tooltips;
using VoxelRPGGame.GameEngine.InventorySystem;
using VoxelRPGGame.GameEngine.UI.Inventory.Trade;
using VoxelRPGGame.GameEngine.InventorySystem.Trade;
using VoxelRPGGame.GameEngine.UI.UIComponents;

namespace VoxelRPGGame.GameEngine.UI
{
    /// <summary>
    /// Class that handles the rendering of all 2D elements of the game state
    /// NOTE: Will need a layout class to define the HUD elements for a particular game
    /// </summary>
    public class GameHUDScreen: AbstractHUDElement
    {
        protected LinkedList<UIElement> _UIElements;
        private static GameHUDScreen _hudScreen = null;



        private string TickText = "";
           
        private float elapsedTime = 0.0f;
        private int ups = 0;

        PlayerInventory playerInventory;
        VoxelRPGGame.GameEngine.InventorySystem.Inventory tempInventory2;

        protected Vector2 _defaultPosition = Vector2.Zero;

       
        public static GameHUDScreen GetInstance()
        {
            if(_hudScreen==null)
            {
                _hudScreen = new GameHUDScreen();
               
            }

            return _hudScreen;
        }

        public static void Dispose()
        {
            _hudScreen = null;
        }

        /// <summary>
        /// The object must be created before initializing any screen elements, so that when the elements
        /// call get instance to attach to the hud's event handlers, they get a valid object reference and
        /// dont'attempt to create a new object which would reult in a stack overflow
        /// </summary>
        private GameHUDScreen()
        {
            hasFocus = true;
            isActive = true;
            _UIElements = new LinkedList<UIElement>();
            _positionAbsolute = Vector2.Zero;
            _positionRelative = _positionAbsolute;
        }


        public void InitializeElements()
        {
            PlayerInventory shopInventory = new PlayerInventory(10);
            shopInventory.Currency = 50;

            InventorySystem.Tools.ToolInventoryItem namedHammer1 = new InventorySystem.Tools.ToolInventoryItem("Hammerfell",InventorySystem.Tools.ToolType.Hammer, "Textures\\UI\\TestIconTool", EquipConstraint.Secondary);
            namedHammer1.CustomerBuyPrice = 10;
            namedHammer1.CustomerSellPrice = 5;
            namedHammer1.Rarity = Rarity.Rare;
            shopInventory.AddItem(namedHammer1);


            playerInventory = new PlayerInventory(22);
            playerInventory.AddItem(new InventorySystem.InventoryItem("Test","Textures\\UI\\TestIcon"));
            playerInventory.Currency = 100;

            playerInventory.AddItem(new InventorySystem.Abilities.Build.BlockInventoryItem(GameEngine.World.Voxels.MaterialType.Dirt,"Test",5));
            playerInventory.AddItem(new InventorySystem.Abilities.Build.RemoveBlockAbility());

            InventorySystem.Tools.ToolInventoryItem namedHammer = new InventorySystem.Tools.ToolInventoryItem("Hammer of Doom",InventorySystem.Tools.ToolType.Hammer, "Textures\\UI\\TestIconTool", EquipConstraint.Secondary);
            namedHammer.Rarity = Rarity.Epic;
            namedHammer.BaseValue = 20;
            namedHammer.CustomerBuyPrice = 35;
            namedHammer.CustomerSellPrice = 15;
            playerInventory.AddItem(namedHammer);
            playerInventory.AddItem(new InventorySystem.Tools.ToolInventoryItem("Iron Hammer",InventorySystem.Tools.ToolType.Hammer, "Textures\\UI\\TestIconTool", EquipConstraint.None) { Rarity = Rarity.Common, BaseValue = 5, CustomerSellPrice = 4,CustomerBuyPrice=6});
            playerInventory.AddItem(new InventorySystem.Abilities.Build.BlockInventoryItem(GameEngine.World.Voxels.MaterialType.Dirt, "Test", 50));
            tempInventory2 = new InventorySystem.Inventory(43);
           // tempInventory2.AddItem(new InventorySystem.InventoryItem("Textures\\UI\\TestIcon"));
            
            _UIElements.AddLast(new PlayerBackpackGridView(playerInventory, 4, new Vector2(ScreenManager.GetInstance().GraphicsDevice.Viewport.Width - 200, ScreenManager.GetInstance().GraphicsDevice.Viewport.Height- 300),_positionAbsolute));
        //    _UIElements.Add(new InventoryGridView(tempInventory2, 6, new Vector2(200,300)));

            InventorySystem.CharacterInventory characterInventory = new InventorySystem.CharacterInventory();
            _UIElements.AddLast(new PlayerToolInventoryView(characterInventory.EquippedItems, new Vector2((ScreenManager.GetInstance().GraphicsDevice.Viewport.Width / 2) - 270, ScreenManager.GetInstance().GraphicsDevice.Viewport.Height - 60), _positionAbsolute));


            if (playerInventory is ITradeInventory)
            {
                _UIElements.AddLast(new ShopInterface(new Vector2(100, 100), (shopInventory as ITradeInventory), (playerInventory as ITradeInventory)) {HasFocus=true });

            }

         
          /*  TickText = new TextElement("");
            TickText.Alpha = 1.0f;
            TickText.Font = ScreenManager.GetInstance().DefaultMenuFont;
            TickText.Position = new Vector2(ScreenManager.GetInstance().GraphicsDevice.Viewport.Width-100, 20);*/
        }

        public override void Update(GameTime theTime, GameState state, Vector2 parentPosition)
        {
        }

        public override void Update(GameTime theTime, GameState state)
        {

           /* foreach (AbstractDrawable2DGameObject gameObject in state.Get2DRenderState())
            {
                gameObject.Update();
            }*/

            //Use a temp List in case list is modified during update
            List<UIElement> tempElements = new List<UIElement>();
            foreach (UIElement element in _UIElements)
            {
                tempElements.Add(element);
            }

            foreach (UIElement e in tempElements)
            {
                if (e.IsActive)
                {
                    //NOTE: May not need access to the game state
                    e.Update(theTime,state,_positionAbsolute);
                }
            }

        //    TickText = "Simulation Tick: " + Engine.SimulationTick+"\nCycle No.: "+Engine.CycleNo+"\nExperiment Length: "+0;
           

        }

        public override void HandleInput(GameTime gameTime, InputState input, GameState state)
        {
            float offsetDampning=10;

            //Treats middle of screen as default mouse position
          //  Vector2 mouseMoveOffset = new Vector2((ScreenManager.GetInstance().GraphicsDevice.Viewport.Width/2 - input.CurrentMouseState.X)/offsetDampning, (ScreenManager.GetInstance().GraphicsDevice.Viewport.Height/2 - input.CurrentMouseState.Y)/offsetDampning);

            //Treats top left of screen as default mouse position
            Vector2 mouseMoveOffset = new Vector2((0 - input.CurrentMouseState.X) / offsetDampning, (0 - input.CurrentMouseState.Y) / offsetDampning);
            _positionAbsolute = _defaultPosition + mouseMoveOffset;


               //Use a temp List in case list is modified during update
            List<UIElement> tempElements = new List<UIElement>();
            foreach (UIElement element in _UIElements)
            {
                tempElements.Add(element);
            }

            foreach (UIElement e in tempElements)
            {
            
                if (e.HasFocus)
                {
                    //NOTE: May not need access to the game state, input handling will be centralised
                    e.HandleInput(gameTime, input, state);
                }
            }



            if (input.CurrentKeyboardState.IsKeyDown(Keys.O) && input.PreviousKeyboardState.IsKeyUp(Keys.O))
            {
                playerInventory.AddItem(new InventorySystem.InventoryItem("Test","Textures\\UI\\TestIcon"));
            }
        /*    if (input.CurrentKeyboardState.IsKeyDown(Keys.P) && input.PreviousKeyboardState.IsKeyUp(Keys.P))
            {
                _UIElements.Add(new InventoryGridView(tempInventory, 4, new Vector2(400, 200)));
            }*/
            //NOTE: Need to determine if UI has focus in order to handle input i.e. if mouse is over UI elements

            /*foreach (AbstractDrawable2DGameObject gameObject in state.Get2DRenderState())
            {
                if (gameObject.HasFocus)
                {
                    gameObject.HandleInput(gameTime, input);
                }
            }*/


        }


        public override void Draw(SpriteBatch Batch, GameState state)
        {
            Vector2 pos= new Vector2(ScreenManager.GetInstance().GraphicsDevice.Viewport.Width, 0);
            float currY = 20;
            
           // Batch.DrawString(ScreenManager.GetInstance().DefaultMenuFont, TickText, new Vector2(pos.X - 220, currY), Color.White);

            //Use a temp List in case list is modified during update
            List<UIElement> tempElements = new List<UIElement>();
            foreach (UIElement element in _UIElements)
            {
                tempElements.Add(element);
            }

            foreach (UIElement e in tempElements)
            {
                if (e.IsVisible)
                {
                    //NOTE: May not need access to the game state
                    e.Draw(Batch, state);
                }
            }

          /*  foreach (AbstractDrawable2DGameObject gameObject in state.Get2DRenderState())
            {
                if (gameObject.IsDrawable)
                {
                    gameObject.Draw(Batch);
                }
            }
          */
        }


        #region Event Handlers

        /// <summary>
        /// Move a UI element to the front of the screen
        /// </summary>
        /// <param name="element"></param>
        public void MoveToFront(UIElement element)
        {
            if(_UIElements.Contains(element))
            {
                _UIElements.Remove(element);
                _UIElements.AddLast(element);
            }
        }

        public void AddTooltip(UIElement requestor,Tooltip tooltip)
        {
            //NOTE: Use requestor to determine where on screen to draw tooltip so it doesn't clip


            if(!_UIElements.Contains(tooltip))
            {
                _UIElements.AddLast(tooltip);
            }
        }

        public void RemoveTooltip( Tooltip tooltip)
        {
          
            if (_UIElements.Contains(tooltip))
            {
                _UIElements.Remove(tooltip);
            }
        }


        #endregion
    }
}
