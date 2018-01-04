//-----------------------------------------------------------------------
// <copyright file="UfoToGeometryConverter.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
    /// Converts between a UFO and a Geometry
    /// </summary>
    internal class UfoToGeometryConverter : IValueConverter
    {
        /// <summary>
        /// Converts a UFO into a Geometry
        /// </summary>
        /// <param name="value">The UFO</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a Geometry containing the UFO</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Ufo ufo = (Ufo)value;
            RectangleGeometry uGeo = new RectangleGeometry(new Rect(ufo.Position.X, ufo.Position.Y, Ufo.W, Ufo.H));

            return uGeo;
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
