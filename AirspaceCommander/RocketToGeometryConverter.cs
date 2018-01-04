//-----------------------------------------------------------------------
// <copyright file="RocketToGeometryConverter.cs" company="Tallyrald">
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
    /// Converter between a Rocket and a Geometry
    /// </summary>
    internal class RocketToGeometryConverter : IValueConverter
    {
        /// <summary>
        /// Converts a rocket into a Geometry
        /// </summary>
        /// <param name="value">The rocket</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a Geometry containing the rocket</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rocket rocket = (Rocket)value;
            RectangleGeometry rGeo = new RectangleGeometry(new Rect(rocket.Position.X - (Rocket.W / 2), rocket.Position.Y - (Rocket.H / 2), Rocket.W, Rocket.H));

            return rGeo;
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
