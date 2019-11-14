﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace DartsScoreMaster.Controls
{
    public class CloseFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var flyout = sender as Flyout;
            if (flyout == null)
                throw new ArgumentException("CloseFlyoutAction can be used only with Flyout");

            flyout.Hide();

            return null;
        }
    }
}