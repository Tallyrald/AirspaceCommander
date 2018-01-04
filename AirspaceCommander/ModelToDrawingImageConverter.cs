//-----------------------------------------------------------------------
// <copyright file="ModelToDrawingImageConverter.cs" company="Tallyrald">
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
    /// Converter between a ViewModel and a DrawingImage
    /// </summary>
    internal class ModelToDrawingImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts a ViewModel into a DrawingImage
        /// </summary>
        /// <param name="value">A ViewModel</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>Returns a DrawingImage that has The visualized ViewModel on it</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ViewModel model = (ViewModel)value;
            if (model == null)
            {
                return null;
            }

            BuildingToGeometryConverter bConv = new BuildingToGeometryConverter();
            ExplosionToGeometryConverter eConv = new ExplosionToGeometryConverter();
            IMovableToGeometryConverter movableConv = new IMovableToGeometryConverter();

            GeometryGroup buildingGeo = (GeometryGroup)bConv.Convert(model.Buildings, null, null, null);
            GeometryGroup explosionGeo = (GeometryGroup)eConv.Convert(model.Explosions, null, null, null);
            List<GeometryDrawing> movableDrawings = (List<GeometryDrawing>)movableConv.Convert(model.Movables, null, model, null);

            DrawingGroup dg = new DrawingGroup();

            // We need to make sure that our play area is as big as we want it by applying a background
            dg.Children.Add(new GeometryDrawing(new SolidColorBrush(Color.FromRgb(0, 0, 0)), null, new RectangleGeometry(new Rect(0, 0, MainWindow.Grid.ActualWidth, MainWindow.Grid.ActualHeight))));

            // We want a single texture on each building, so we need to apply the brush on each building geometry
            foreach (Geometry singleBuilding in buildingGeo.Children)
            {
                if (singleBuilding is RectangleGeometry)
                {
                    // The building itself
                    Pen bPen = new Pen(Brushes.White, 0.5);
                    bPen.DashStyle = DashStyles.Dash;
                    dg.Children.Add(new GeometryDrawing(model.GetBuildingBrush(singleBuilding.Bounds.X, singleBuilding.Bounds.Y), bPen, singleBuilding));
                }
                else if (singleBuilding is GeometryGroup)
                {
                    GeometryGroup gg = (GeometryGroup)singleBuilding;
                    if (gg.Children.Count == 2)
                    {
                        // A shield with its HP
                        dg.Children.Add(new GeometryDrawing(model.GetShieldBrush(singleBuilding.Bounds.X, singleBuilding.Bounds.Y), null, gg.Children[0]));
                        dg.Children.Add(new GeometryDrawing(Brushes.Black, null, gg.Children[1]));
                    }
                    else
                    {
                        // The building's curHP/maxHP
                        dg.Children.Add(new GeometryDrawing(Brushes.Black, null, gg));
                    }
                }
            }

            foreach (EllipseGeometry singleExplosion in explosionGeo.Children)
            {
                SolidColorBrush brush;
                if (singleExplosion.Bounds.Height > MainWindow.Grid.ActualHeight)
                {
                    brush = Brushes.Orange;
                }
                else
                {
                    brush = Brushes.Transparent;
                }

                dg.Children.Add(new GeometryDrawing(brush, new Pen(Brushes.White, 2), singleExplosion));
            }

            foreach (GeometryDrawing movableDrawing in movableDrawings)
            {
                dg.Children.Add(movableDrawing);
            }

            DrawingImage img = new DrawingImage();
            img.Drawing = dg;
            return img;
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
