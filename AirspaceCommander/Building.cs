//-----------------------------------------------------------------------
// <copyright file="Building.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System.Windows;

namespace Airspace_Commander
{
    /// <summary>
    /// This class represents a building in the city
    /// </summary>
    public class Building : Bindable
    {
        /// <summary>
        /// Width of every Building
        /// </summary>
        public const int W = 100;

        /// <summary>
        /// Height of every Building
        /// </summary>
        public const int H = 100;

        /// <summary>
        /// The max HP of the building
        /// </summary>
        private int maxHP;

        /// <summary>
        /// The current (actual) HP of the building
        /// </summary>
        private int curHP;

        /// <summary>
        /// The active shield on the building
        /// </summary>
        private Shield shield;

        /// <summary>
        /// The buildingshape to refer to
        /// </summary>
        private Point position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Airspace_Commander.Building" /> class.
        /// </summary>
        /// <param name="newMaxHP">Max hitpoints this building will have</param>
        /// <param name="coords">The coordinates to place the building at</param>
        public Building(int newMaxHP, Point coords)
        {
            maxHP = newMaxHP;
            curHP = maxHP;
            position = coords;
        }

        /// <summary>
        /// Gets or sets the maxHP of the building
        /// </summary>
        public int MaxHP
        {
            get { return maxHP; }
            set { SetProperty(ref maxHP, value); }
        }

        /// <summary>
        /// Gets or sets the current HP of the building
        /// </summary>
        public int CurHP
        {
            get { return curHP; }
            set { SetProperty(ref curHP, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the building has any HP left
        /// </summary>
        public bool Is_OK
        {
            get
            {
                if (CurHP <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets the shield currently active on the building
        /// </summary>
        public Shield Shield
        {
            get { return shield; }
        }

        /// <summary>
        /// Gets the shape this logic refers to
        /// </summary>
        public Point Position
        {
            get { return position; }
        }

        /// <summary>
        /// Damages the building
        /// </summary>
        /// <param name="damage">The damage to inflict</param>
        public void Hit(int damage)
        {
            if (shield != null && shield.CurHP > 0)
            {
                shield.Hit(damage);
            }
            else
            {
                CurHP -= damage;
            }
        }

        /// <summary>
        /// Restores 1 HP to the building
        /// </summary>
        public void Renovate()
        {
            if (CurHP < MaxHP)
            {
                CurHP++;
            }
        }

        /// <summary>
        /// Doubles the max HP of the building
        /// </summary>
        public void Modernize()
        {
            MaxHP++;
        }

        /// <summary>
        /// Replaces the currently active shield with a new one.
        /// </summary>
        /// <param name="newShield">The shield which should be placed on the building</param>
        public void ActivateShield(Shield newShield)
        {
            shield = newShield;
        }
    }
}
