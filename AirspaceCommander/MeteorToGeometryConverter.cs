//-----------------------------------------------------------------------
// <copyright file="MeteorToGeometryConverter.cs" company="Tallyrald">
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
    /// Converter between a Meteor and a Geometry
    /// </summary>
    internal class MeteorToGeometryConverter : IValueConverter
    {
        /// <summary>
        /// Converts a meteor into a Geometry
        /// </summary>
        /// <param name="value">The meteor</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a Geometry containing the meteor</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Meteor meteor = (Meteor)value;
            RectangleGeometry mGeo = new RectangleGeometry(new Rect(meteor.Position.X, meteor.Position.Y, Meteor.W, Meteor.H));

            return mGeo;
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
