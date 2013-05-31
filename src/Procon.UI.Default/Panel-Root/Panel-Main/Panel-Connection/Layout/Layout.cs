﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Procon.UI.Default.Root.Main.Connection.Layout
{
    using Procon.UI.API;

    [Extension(
        Alters    = new String[] { },
        Replaces  = new String[] { },
        DependsOn = new String[] { })]
    public class Layout : IExtension
    {
        #region IExtension Properties

        public String Author
        { get { return "Imisnew2"; } }

        public Uri Link
        { get { return new Uri("www.TeamPlayerGaming.com/members/Imisnew2.html"); } }

        public String LinkText
        { get { return "Team Player Gaming"; } }

        public String Name
        { get { return "Main Connection Layout"; } }

        public String Description
        { get { return ""; } }

        public Version Version
        { get { return new Version(1, 0, 0, 0); } }

        #endregion IExtension Properties


        [STAThread]
        public bool Entry(Window root)
        {
            // Find the controls I want to use and check for issues.
            Grid tLayout = ExtensionApi.FindControl<Grid>(root, "MainLayout");


            // Do what I need to setup my control.
            LayoutView tView = new LayoutView();
            Grid.SetRow(tView, 1);
            tLayout.Children.Add(tView);


            // Setup the default settings.
            if (ExtensionApi.Settings["View"]["Connection"].Value == null) {
                ExtensionApi.Settings["View"]["Connection"].Value = "Players";
            }


            // Exit with good status.
            return true;
        }
    }
}
