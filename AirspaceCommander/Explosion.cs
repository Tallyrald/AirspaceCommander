//-----------------------------------------------------------------------
// <copyright file="Explosion.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airspace_Commander
{
    /// <summary>
    /// This class represents an explosion
    /// </summary>
    public class Explosion : Bindable
    {
        /// <summary>
        /// Contains the radius of the explosion
        /// </summary>
        private int radius = 1;

        /// <summary>
        /// Contains the radius of the explosion
        /// </summary>
        private int maxRadius;

        /// <summary>
        /// Contains the position of the explodion (center)
        /// </summary>
        private Point position;

        /// <summary>
        /// Contains the damage the explosion causes
        /// </summary>
        private int damage;

        /// <summary>
        /// Contains the Time To Live until the explosion disappears
        /// </summary>
        private int ttl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Airspace_Commander.Explosion" /> class.
        /// </summary>
        /// <param name="pos">The position of the explosion</param>
        /// <param name="newRadius">The radisu of the explosion</param>
        /// <param name="newDamage">The damage this explosion causes</param>
        /// <param name="newTtl">Time To Live, represents the number of ticks before the explosion disappears</param>
        public Explosion(Point pos, int newRadius, int newDamage, int newTtl)
        {
            ttl = newTtl;
            if (ttl == 2)
            {
                radius = newRadius;
            }
            else
            {
                maxRadius = newRadius;
            }

            position = pos;
            damage = newDamage;
        }

        /// <summary>
        /// Gets the radius of the explosion
        /// </summary>
        public int Radius
        {
            get
            {
                if (radius < maxRadius)
                {
                    return radius++;
                }
                else
                {
                    return radius;
                }
            }
        }

        /// <summary>
        /// Gets the position of the explosion (center)
        /// </summary>
        public Point Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the damage the explosion causes
        /// </summary>
        public int Damage
        {
            get { return damage; }
        }

        /// <summary>
        /// Gets or sets the number of ticks left before the explosion disappears
        /// </summary>
        public int Ttl
        {
            get { return ttl; }
            set { SetProperty(ref ttl, value); }
        }
    }
}
