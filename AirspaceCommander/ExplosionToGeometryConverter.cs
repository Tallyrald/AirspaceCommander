//-----------------------------------------------------------------------
// <copyright file="ExplosionToGeometryConverter.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Airspace_Commander
{
    /// <summary>
    /// Converter between explosion logic and geometry
    /// </summary>
    internal class ExplosionToGeometryConverter : IValueConverter
    {
        /// <summary>
        /// Converts a BindingList of explosions into a GeometryGroup
        /// </summary>
        /// <param name="value">A BindingList containing explosions</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a GeometryGroup containing all explosions</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BindingList<Explosion> explosions = (BindingList<Explosion>)value;

            GeometryGroup allExplosionGeo = new GeometryGroup();

            foreach (Explosion explosion in explosions)
            {
                EllipseGeometry eGeo = new EllipseGeometry(new Point(explosion.Position.X, explosion.Position.Y), explosion.Radius, explosion.Radius);
                allExplosionGeo.Children.Add(eGeo);
            }

            return allExplosionGeo;
        }

        /// <summary>
        /// The method is not used.
        /// </summary>
        /// <param name="value">The parameter is not used.</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a NotImplementedException</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
