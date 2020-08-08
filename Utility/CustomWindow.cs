using System;
using HandyControl.Controls;

namespace Epide.Utility
{
    public class CustomWindow : Window
    {
        protected override void OnActivated(EventArgs e)
        {
            // this.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/icon.ico"));
            base.OnActivated(e);
        }
    }
}