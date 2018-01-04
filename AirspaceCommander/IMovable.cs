//-----------------------------------------------------------------------
// <copyright file="IMovable.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airspace_Commander
{
    /// <summary>
    /// An interface for all moving game objects
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// Gets the position of the object
        /// </summary>
        Point Position { get; }

        /// <summary>
        /// Gets the start coordinate of the game object on the X axis
        /// </summary>
        double StartX { get; }

        /// <summary>
        /// Gets the start coordinate of the game object on the Y axis
        /// </summary>
        double StartY { get; }

        /// <summary>
        /// Gets the target coordinate of the game object on the X axis
        /// </summary>
        double TargetX { get; }

        /// <summary>
        /// Gets the start coordinate of the game object on the Y axis
        /// </summary>
        double TargetY { get; }

        /// <summary>
        /// Gets a value indicating whether the game object is destroyed
        /// </summary>
        bool IsDestroyed { get; }

        /// <summary>
        /// Gets the angle of the object
        /// </summary>
        int Angle { get; }

        /// <summary>
        /// Moves the game object
        /// </summary>
        /// <param name="gm">The GameManager instance to use</param>
        /// <param name="buildings">The buildings currently in game</param>
        /// <returns>Returns true if the Move is successful, false if the object has been destroyed</returns>
        bool Move(GameManager gm, BindingList<Building> buildings);

        /// <summary>
        /// Defines how the object should explode
        /// </summary>
        /// <param name="gm">The GameManager to use</param>
        /// <param name="buildings">The building participating in the collision</param>
        void Explode(GameManager gm, Building buildings);

        /// <summary>
        /// Simulates taking a hit by an explosion
        /// </summary>
        /// <param name="explosion">The explosion that causes the hit</param>
        /// <returns>Returns true if the object was destroyed by the explosion, false otherwise</returns>
        bool GetHit(Explosion explosion);
    }
}
