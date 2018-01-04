//-----------------------------------------------------------------------
// <copyright file="IWarhead.cs" company="Tallyrald">
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
    /// This is the interface for all rocket warheads in the game
    /// </summary>
    public interface IWarhead
    {
        /// <summary>
        /// Gets the name of the warhead
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the explosion radius
        /// </summary>
        int Radius { get; }

        /// <summary>
        /// Gets the damage this warhead deals
        /// </summary>
        int Damage { get; }
    }
}
