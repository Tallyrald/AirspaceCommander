//-----------------------------------------------------------------------
// <copyright file="Bindable.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Airspace_Commander
{
    /// <summary>
    /// Class for managing Bindable objects
    /// </summary>
    public class Bindable : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to indicate property change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This function manages the fired PropertyChanged event
        /// </summary>
        /// <param name="name">The name of the changed property</param>
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Use this function in a property's setter
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="field">The field to set</param>
        /// <param name="newvalue">The value to put into the field</param>
        /// <param name="name">The caller's name</param>
        protected void SetProperty<T>(ref T field, T newvalue, [CallerMemberName] string name = null)
        {
            field = newvalue;
            OnPropertyChanged(name);
        }
    }
}
