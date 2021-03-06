﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VoxelRPGGame.GameEngine.EnvironmentState;

namespace VoxelRPGGame.GameEngine.UI
{
    public abstract class AbstractHUDElement
    {
        /// <summary>
        /// Denotes whether the screen has focus and can be allowed to handle input
        /// </summary>
        protected bool hasFocus = false; 

        /// <summary>
        /// Denotes whether the screen is active and can be allowed to update
        /// </summary>
        protected bool isActive = false; 

        /// <summary>
        /// Denotes whether the screen is visible and can be drawn
        /// </summary>
        protected bool isVisible = true;

        protected Vector2 _positionAbsolute;
        protected Vector2 _positionRelative;


        public abstract void Update(GameTime theTime, GameState state);

        /// <summary>
        /// Use if the element's position is set relative to the parent
        /// </summary>
        /// <param name="theTime"></param>
        /// <param name="state"></param>
        /// <param name="parentPosition"></param>
        public abstract void Update(GameTime theTime, GameState state, Vector2 parentPosition);

        //  public virtual void HandleInput(GameTime gameTime, InputState input) { }

        //Handles user input. This is separate to Update, as a screen can still be updated even if it cannot handle input  
        public abstract void HandleInput(GameTime gameTime, InputState input, GameState state);


        public abstract void Draw(SpriteBatch Batch, GameState state);
 


#region Properties
        /// <summary>
        /// Denotes whether the screen has focus and can be allowed to handle input
        /// </summary>
        public bool HasFocus
        {
            get
            {
                return hasFocus;
            }

            set
            {
                hasFocus = value;
            }
        }
        /// <summary>
        /// Denotes whether the screen is active and can be allowed to update
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
            }

        }

        /// <summary>
        /// Denotes whether the screen is visible and can be drawn
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
            }
        }


        #endregion
    }
}
