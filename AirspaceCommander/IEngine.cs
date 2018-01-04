//-----------------------------------------------------------------------
// <copyright file="IEngine.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airspace_Commander
{
    /// <summary>
    /// This is the interface for all rocket engines in the game
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Gets the name of the engine type
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the engine's speed 
        /// </summary>
        double Speed { get; set; }

        /// <summary>
        /// Gets or sets the rate at which the engine accelerates the rocket
        /// </summary>
        double SpeedRate { get; set; }

        /// <summary>
        /// Clones the engine
        /// </summary>
        /// <returns>Returns the cloned engine</returns>
        IEngine Clone();
    }
}
