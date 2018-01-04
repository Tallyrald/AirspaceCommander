//-----------------------------------------------------------------------
// <copyright file="IMovableToGeometryConverter.cs" company="Tallyrald">
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
using System.Windows.Data;
using System.Windows.Media;

namespace Airspace_Commander
{
    /// <summary>
    /// Converter between IMovable elements and GeometryDrawing
    /// </summary>
    internal class IMovableToGeometryConverter : IValueConverter
    {
        /// <summary>
        /// Converts a BindingList of IMovable elements into a GeometryDrawing
        /// </summary>
        /// <param name="value">A BindingList containing IMovable elements</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The ViewModel instance to use</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a GeometryDrawing that has all elements</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RocketToGeometryConverter rConv = new RocketToGeometryConverter();
            MeteorToGeometryConverter mConv = new MeteorToGeometryConverter();
            UfoToGeometryConverter uConv = new UfoToGeometryConverter();
            JetToGeometryConverter jConv = new JetToGeometryConverter();

            ViewModel model = (ViewModel)parameter;
            BindingList<IMovable> elements = (BindingList<IMovable>)value;
            List<GeometryDrawing> gd = new List<GeometryDrawing>();

            foreach (IMovable element in elements)
            {
                if (element is Rocket)
                {
                    Rocket rElement = (Rocket)element;
                    Pen pen = new Pen();
                    if (rElement.Warhead is NuclearWarhead)
                    {
                        pen = new Pen(Brushes.Red, 2);
                    }
                    else if (rElement.Warhead is SmallWarhead && !rElement.IsFriendly)
                    {
                        pen = new Pen(Brushes.White, 0.5);
                    }
                    else if (rElement.Warhead is MediumWarhead && !rElement.IsFriendly)
                    {
                        pen = new Pen(Brushes.Yellow, 2);
                    }
                    else if (rElement.Warhead is LargeWarhead && !rElement.IsFriendly)
                    {
                        pen = new Pen(Brushes.Orange, 2);
                    }

                    RectangleGeometry rocket = (RectangleGeometry)rConv.Convert(element, null, null, null);
                    gd.Add(new GeometryDrawing(model.GetRocketBrush(rocket.Bounds.X, rocket.Bounds.Y, element.Angle), pen, rocket));
                }
                else if (element is Meteor)
                {
                    RectangleGeometry meteor = (RectangleGeometry)mConv.Convert(element, null, null, null);
                    gd.Add(new GeometryDrawing(model.GetMeteorBrush(meteor.Bounds.X, meteor.Bounds.Y), null, meteor));
                }
                else if (element is Ufo)
                {
                    RectangleGeometry ufo = (RectangleGeometry)uConv.Convert(element, null, null, null);
                    gd.Add(new GeometryDrawing(model.GetUfoBrush(ufo.Bounds.X, ufo.Bounds.Y), null, ufo));
                }
                else if (element is Jet)
                {
                    RectangleGeometry jet = (RectangleGeometry)jConv.Convert(element, null, null, null);
                    gd.Add(new GeometryDrawing(model.GetJetBrush(jet.Bounds.X, jet.Bounds.Y, element.Angle), null, jet));
                }
            }

            return gd;
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
