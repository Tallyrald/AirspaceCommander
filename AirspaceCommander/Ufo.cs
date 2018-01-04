//-----------------------------------------------------------------------
// <copyright file="Ufo.cs" company="Tallyrald">
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
    /// Ufo business logic
    /// </summary>
    public class Ufo : Bindable, IMovable
    {
        /// <summary>
        /// The width of the UFO
        /// </summary>
        public const int W = 70;

        /// <summary>
        /// The height of the UFO
        /// </summary>
        public const int H = 40;

        /// <summary>
        /// Contains the delta speed on the X axis
        /// </summary>
        private double dx = 0;

        /// <summary>
        /// Contains the position of the UFO
        /// </summary>
        private Point position;

        /// <summary>
        /// Contains the starting coordinate on the X axis
        /// </summary>
        private double startX;

        /// <summary>
        /// Contains the starting coordinate on the Y axis
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
        /// Contains the current HP of the UFO
        /// </summary>
        private int curHP;

        /// <summary>
        /// Contains the explosions that already hit the UFO (multi-tick damage protection)
        /// </summary>
        private List<Explosion> alreadyHit;

        /// <summary>
        /// Contains the UFO's engine (OMG it's using our technology!!!)
        /// </summary>
        private IEngine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ufo" /> class.
        /// </summary>
        /// <param name="coords">The starting coordinates</param>
        /// <param name="newTargetX">The target coordinate on the X axis</param>
        /// <param name="newCurHP">Starting HP</param>
        /// <param name="newEngine">The engine to use</param>
        public Ufo(Point coords, double newTargetX, int newCurHP, IEngine newEngine)
        {
            position = coords;
            startX = position.X;
            startY = position.Y;
            targetX = newTargetX;
            targetY = position.Y;
            curHP = newCurHP;
            engine = newEngine;

            if (position.X < newTargetX)
            {
                dx = engine.Speed;
            }
            else
            {
                dx = -1 * engine.Speed;
            }

            alreadyHit = new List<Explosion>();
        }

        /// <summary>
        /// Gets the position of the UFO
        /// </summary>
        public Point Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the starting coordinate on the X axis
        /// </summary>
        public double StartX
        {
            get { return startX; }
        }

        /// <summary>
        /// Gets the starting coordinate on the Y axis
        /// </summary>
        public double StartY
        {
            get { return startY; }
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
        /// Gets or sets the current HP of the UFO
        /// </summary>
        public int CurHP
        {
            get { return curHP; }
            set { SetProperty(ref curHP, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the UFO is destroyed
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
        /// Gets the direction (display) of the UFO (0 - left/right)
        /// </summary>
        public int Angle
        {
            get { return 0; }
        }

        /// <summary>
        /// Moves the UFO
        /// </summary>
        /// <param name="gm">The GameManager to use</param>
        /// <param name="buildings">The parameter is not used.</param>
        /// <returns>Returns false if the UFO is no more, true otherwise</returns>
        public bool Move(GameManager gm, BindingList<Building> buildings = null)
        {
            if (IsDestroyed)
            {
                Explode(gm);
                return false;
            }

            ChangeSpeed();

            double targetDistance = Math.Sqrt((Math.Abs(targetX - startX) * Math.Abs(targetX - startX)) + (Math.Abs(targetY - startY) * Math.Abs(targetY - startY)));
            double curDistance = Math.Sqrt((Math.Abs(position.X - startX) * Math.Abs(position.X - startX)) + (Math.Abs(position.Y - startY) * Math.Abs(position.Y - startY)));

            if (curDistance >= targetDistance)
            {
                return false;
            }
            else
            {
                if (position.X > targetX && position.X + dx < targetX)
                {
                    position.X = targetX;
                }
                else
                {
                    position.X += dx;
                }
            }

            return true;
        }

        /// <summary>
        /// Increases the rocket's delta speed defined by the engine's properties
        /// </summary>
        public void ChangeSpeed()
        {
            if (position.X < targetX)
            {
                dx += engine.SpeedRate / 100;
            }
            else
            {
                dx += (-1 * engine.SpeedRate) / 100;
            }
        }

        /// <summary>
        /// Defines how the UFO explodes
        /// </summary>
        /// <param name="gm">The GameManager to use</param>
        /// <param name="building">The parameter is not used.</param>
        public void Explode(GameManager gm, Building building = null)
        {
            Point explosionCoords = new Point(position.X + (W / 2), position.Y + (H / 2));
            gm.GenerateExplosion(explosionCoords, 50, 1);
            gm.Score();
        }

        /// <summary>
        /// Simulates taking a hit from an explosion
        /// </summary>
        /// <param name="explosion">The explosion that caused the hit</param>
        /// <returns>Returns false</returns>
        public bool GetHit(Explosion explosion)
        {
            if (!alreadyHit.Contains(explosion))
            {
                CurHP -= explosion.Damage;
                alreadyHit.Add(explosion);
            }

            return false;
        }
    }
}
