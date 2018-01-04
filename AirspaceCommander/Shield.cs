//-----------------------------------------------------------------------
// <copyright file="Shield.cs" company="Tallyrald">
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
    /// Represents a shield over a building
    /// </summary>
    public class Shield : Bindable
    {
        /// <summary>
        /// The width of the shield
        /// </summary>
        public const int W = 20;

        /// <summary>
        /// The height of the shield
        /// </summary>
        public const int H = 20;

        /// <summary>
        /// The max HP of the shield
        /// </summary>
        private int maxHP;

        /// <summary>
        /// The current (actual) HP of the shield
        /// </summary>
        private int curHP;

        // TO-DO: review if shape is needed

        /// <summary>
        /// Initializes a new instance of the <see cref="Shield" /> class.
        /// </summary>
        /// <param name="newMaxHP">Max hitpoints this shield will have</param>
        public Shield(int newMaxHP)
        {
            maxHP = newMaxHP;
            curHP = maxHP;
        }

        /// <summary>
        /// Gets the maxHP of the shield
        /// </summary>
        public int MaxHP
        {
            get { return maxHP; }
        }

        /// <summary>
        /// Gets or sets the current HP of the shield
        /// </summary>
        public int CurHP
        {
            get
            {
                return curHP;
            }

            set
            {
                if (value >= 0)
                {
                    SetProperty(ref curHP, value);
                }
            }
        }

        /// <summary>
        /// Damages the shield
        /// </summary>
        /// <param name="damage">The amount of damage to deal</param>
        public void Hit(int damage)
        {
            if (CurHP - damage > 0)
            {
                CurHP -= damage;
            }
            else
            {
                CurHP = 0;
            }
        }
    }
}
