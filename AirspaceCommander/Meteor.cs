//-----------------------------------------------------------------------
// <copyright file="Meteor.cs" company="Tallyrald">
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
    /// Represents a meteor
    /// </summary>
    public class Meteor : Bindable, IMovable
    {
        /// <summary>
        /// The width of the Meteor
        /// </summary>
        public const int W = 30;

        /// <summary>
        /// The height of the Meteor
        /// </summary>
        public const int H = 30;

        /// <summary>
        /// Contains the position of the meteor
        /// </summary>
        private Point position;

        /// <summary>
        /// The max HP of the meteor
        /// </summary>
        private int maxHP;

        /// <summary>
        /// The current (actual) HP of the meteor
        /// </summary>
        private int curHP;

        /// <summary>
        /// Contains the delta speed on the X axis
        /// </summary>
        private double dx = 0;

        /// <summary>
        /// Contains the delta speed on the Y axis
        /// </summary>
        private double dy = 0;

        /// <summary>
        /// Contains the target coordinate on the X axis
        /// </summary>
        private double startX;

        /// <summary>
        /// Contains the target coordinate on the Y axis
        /// </summary>
        private double startY;

        /// <summary>
        /// Contains the target coordinate on the X axis
        /// </summary>
        private double targetX;

        /// <summary>
        /// Contains the target coordinate on the Y axis
        /// </summary>
        private double targetY;

        /// <summary>
        /// Contains the explosions that already hit the UFO (multi-tick damage protection)
        /// </summary>
        private List<Explosion> alreadyHit;

        /// <summary>
        /// Initializes a new instance of the <see cref="Meteor" /> class.
        /// </summary>
        /// <param name="coords">The position of the meteor</param>
        /// <param name="newTargetY">The target coordinate on the Y axis</param>
        /// <param name="newMaxHP">The max HP of the meteor</param>
        public Meteor(Point coords, double newTargetY, int newMaxHP)
        {
            position = coords;
            startX = position.X;
            startY = position.Y;
            targetX = position.X;
            targetY = newTargetY;
            dy = 3;
            maxHP = newMaxHP;
            curHP = maxHP;
            alreadyHit = new List<Explosion>();
        }

        /// <summary>
        /// Gets the position of this meteor
        /// </summary>
        public Point Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the target coordinate on the X axis
        /// </summary>
        public double TargetX
        {
            get { return targetX; }
        }

        /// <summary>
        /// Gets the target coordinate on the Y axis
        /// </summary>
        public double TargetY
        {
            get { return targetY; }
        }

        /// <summary>
        /// Gets the start coordinate on the X axis
        /// </summary>
        public double StartX
        {
            get { return startX; }
        }

        /// <summary>
        /// Gets the start coordinate on the Y axis
        /// </summary>
        public double StartY
        {
            get { return startY; }
        }

        /// <summary>
        /// Gets the maxHP of the Meteor
        /// </summary>
        public int MaxHP
        {
            get { return maxHP; }
        }

        /// <summary>
        /// Gets or sets the current HP of the meteor
        /// </summary>
        public int CurHP
        {
            get { return curHP; }
            set { SetProperty(ref curHP, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the meteor has any HP left
        /// </summary>
        public bool IsDestroyed
        {
            get
            {
                if (CurHP <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the direction (display) of the Meteor (0 - down)
        /// </summary>
        public int Angle
        {
            get { return 0; }
        }

        /// <summary>
        /// Moves the meteor
        /// </summary>
        /// <param name="gm">The GameManager to use</param>
        /// /// <param name="buildings">A list containing all buildings</param>
        /// <returns>Returns true if the move was successful, false if the the meteor is no more</returns>
        public bool Move(GameManager gm,  BindingList<Building> buildings = null)
        {
            if (IsDestroyed)
            {
                return false;
            }

            // Check for collision
            foreach (Building building in buildings)
            {
                if (GameManager.Intersects(this, building))
                {
                    Explode(gm, building);
                    return false;
                }
            }

            double targetDistance = Math.Sqrt((Math.Abs(targetX - startX) * Math.Abs(targetX - startX)) + (Math.Abs(targetY - startY) * Math.Abs(targetY - startY)));
            double curDistance = Math.Sqrt((Math.Abs(position.X - startX) * Math.Abs(position.X - startX)) + (Math.Abs(position.Y - startY) * Math.Abs(position.Y - startY)));

            if (curDistance >= targetDistance)
            {
                return false;
            }
            else
            {
                position.X += dx;
                position.Y += dy;
            }

            return true;
        }

        /// <summary>
        /// Simulates collision with a building
        /// </summary>
        /// <param name="gm">The GameManager to use</param>
        /// <param name="building">The building participating in the collision</param>
        public void Explode(GameManager gm, Building building = null)
        {
            // If a building's hit, we do dmg
            if (building != null)
            {
                // Always does 1 dmg
                building.Hit(1);
            }
        }

        /// <summary>
        /// Simulates taking a hit from an explosion
        /// </summary>
        /// <param name="explosion">The explosion that caused the hit</param>
        /// <returns>Returns true if the meteor was destroyed, false otherwise</returns>
        public bool GetHit(Explosion explosion)
        {
            if (!alreadyHit.Contains(explosion))
            {
                CurHP -= explosion.Damage;
                alreadyHit.Add(explosion);
            }

            return IsDestroyed;
        }
    }
}
