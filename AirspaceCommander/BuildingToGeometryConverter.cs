//-----------------------------------------------------------------------
// <copyright file="BuildingToGeometryConverter.cs" company="Tallyrald">
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
    /// Converter between building logic and geometry
    /// </summary>
    internal class BuildingToGeometryConverter : IValueConverter
    {
        /// <summary>
        /// Converts a BindingList of buildings into a GeometryGroup
        /// </summary>
        /// <param name="value">A BindingList containing buildings</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a GeometryGroup which has all buildings included</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BindingList<Building> buildings = (BindingList<Building>)value;
            GeometryGroup allBuildingGeo = new GeometryGroup();

            foreach (Building building in buildings)
            {
                RectangleGeometry bGeo = new RectangleGeometry(new Rect(building.Position.X, building.Position.Y, Building.W, Building.H));
                allBuildingGeo.Children.Add(bGeo);

                if (building.Shield != null && building.Shield.CurHP > 0)
                {
                    GeometryGroup gg = new GeometryGroup();
                    RectangleGeometry sGeo = new RectangleGeometry(new Rect(building.Position.X + ((Building.W / 2) - 18), building.Position.Y + (Building.H / 3), Shield.W, Shield.H));
                    FormattedText text = new FormattedText(building.Shield.CurHP.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
                    Geometry textGeometry = text.BuildGeometry(new Point(building.Position.X + ((Building.W / 2) + 10), building.Position.Y + (Building.H / 3)));
                    gg.Children.Add(sGeo);
                    gg.Children.Add(textGeometry);
                    allBuildingGeo.Children.Add(gg);
                }
                else
                {
                    FormattedText text = new FormattedText(building.CurHP + "/" + building.MaxHP, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
                    Geometry textGeometry = text.BuildGeometry(new Point(building.Position.X + ((Building.W / 2) - 11), building.Position.Y + (Building.H / 3)));
                    allBuildingGeo.Children.Add(textGeometry);
                }
            }

            return allBuildingGeo;
        }

        /// <summary>
        /// The method is not used
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
