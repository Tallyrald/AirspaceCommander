//-----------------------------------------------------------------------
// <copyright file="MediumEngine.cs" company="Tallyrald">
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
    /// This class represents a medium engine
    /// </summary>
    internal class MediumEngine : IEngine
    {
        /// <summary>
        /// Contains the name of the engine for display
        /// </summary>
        private string name;

        /// <summary>
        /// Contains the base speed the engine has
        /// </summary>
        private double speed;

        /// <summary>
        /// Contains the acceleration rate the engine uses
        /// </summary>
        private double speedRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Airspace_Commander.MediumEngine" /> class.
        /// </summary>
        /// <param name="newName">The name to display</param>
        /// <param name="extraSpeed">Additional speed from bonuses</param>
        /// <param name="extraSr">Additional acceleration from bonuses</param>
        public MediumEngine(string newName, double extraSpeed, double extraSr)
        {
            name = newName;
            speed = 15 + extraSpeed;
            speedRate = 0 + extraSr;
        }

        /// <summary>
        /// Gets the name of the engine
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets the speed the engine has
        /// </summary>
        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// Gets or sets the acceleration rate the engine uses
        /// </summary>
        public double SpeedRate
        {
            get { return speedRate; }
            set { speedRate = value; }
        }

        /// <summary>
        /// Clones the whole engine
        /// </summary>
        /// <returns>Returns an exact copy of the engine</returns>
        public IEngine Clone()
        {
            return (IEngine)MemberwiseClone();
        }

        /// <summary>
        /// Converts the engine to string for display
        /// </summary>
        /// <returns>Returns the stringified version of the engine</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
