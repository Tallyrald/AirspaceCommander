//-----------------------------------------------------------------------
// <copyright file="JetToGeometryConverter.cs" company="Tallyrald">
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
    /// This class converts between a Jet and a Geometry
    /// </summary>
    internal class JetToGeometryConverter : IValueConverter
    {
        /// <summary>
        /// Converts a Jet into Geometry
        /// </summary>
        /// <param name="value">The Jet to convert</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a Geometry containing the Jet</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Jet jet = (Jet)value;
            RectangleGeometry jGeo = new RectangleGeometry(new Rect(jet.Position.X, jet.Position.Y, Jet.W, Jet.H));

            return jGeo;
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
