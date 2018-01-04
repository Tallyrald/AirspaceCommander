//-----------------------------------------------------------------------
// <copyright file="LargeWarhead.cs" company="Tallyrald">
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
    /// This class represents a large warhead
    /// </summary>
    internal class LargeWarhead : IWarhead
    {
        /// <summary>
        /// Contains the name of this warhead for display
        /// </summary>
        private string name;

        /// <summary>
        /// Contains the damage this warhead deals
        /// </summary>
        private int damage;

        /// <summary>
        /// Contains the explosion's radius
        /// </summary>
        private int radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="Airspace_Commander.LargeWarhead" /> class.
        /// </summary>
        /// <param name="newName">The name to display</param>
        /// <param name="extraDmg">Additional damage this warhead will deal (base: 1)</param>
        /// <param name="extraRadius">Additional radius in which this warhead will be effective (base: 1)</param>
        public LargeWarhead(string newName, int extraDmg, int extraRadius)
        {
            name = newName;
            damage = 1 + extraDmg;
            radius = 50 + extraRadius;
        }

        /// <summary>
        /// Gets the name of this warhead
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the damage this warhead deals
        /// </summary>
        public int Damage
        {
            get { return damage; }
        }

        /// <summary>
        /// Gets the radius this warhead is effective in
        /// </summary>
        public int Radius
        {
            get { return radius; }
        }

        /// <summary>
        /// Converts the warhead to string for display
        /// </summary>
        /// <returns>Returns the stringified version of the warhead</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
