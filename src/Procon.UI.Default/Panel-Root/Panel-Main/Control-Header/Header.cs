﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Procon.UI.Default.Root.Main.Header
{
    using Procon.UI.API;
    using Procon.UI.API.Commands;
    using Procon.UI.API.Utils;

    [Extension(
        Alters    = new String[] { },
        Replaces  = new String[] { },
        DependsOn = new String[] { })]
    public class Header : IExtension
    {
        #region IExtension Properties

        public String Author
        { get { return "Imisnew2"; } }

        public Uri Link
        { get { return new Uri("www.TeamPlayerGaming.com/members/Imisnew2.html"); } }

        public String LinkText
        { get { return "TeamPlayer Gaming"; } }

        public String Name
        { get { return GetType().Namespace; } }

        public String Description
        { get { return ""; } }

        public Version Version
        { get { return new Version(1, 0, 0, 0); } }

        #endregion IExtension Properties

        
        // An easy accessor for Properties and Commands of this control.
        private readonly ArrayDictionary<String, Object>   mProps;
        private readonly ArrayDictionary<String, ICommand> mComms;
        public Header()
        {
            mProps = ExtensionApi.GetProperties(GetType());
            mComms = ExtensionApi.GetCommands(GetType());
        }


        [STAThread]
        public bool Entry(Window root)
        {
            // Find the controls I want to use and check for issues.
            Grid tLayout = ExtensionApi.FindControl<Grid>(root, "MainLayout");
            if (tLayout == null) {
                return false;
            }


            // Do what I need to setup my control.
            HeaderView tView = new HeaderView();
            Grid.SetRow(tView, 0);
            tLayout.Children.Add(tView);


            // Setup the default settings.
            mProps["Selected"].Value = tView.MhSelected;
            View_Checked(new AttachedCommandArgs(null, null, ExtensionApi.Settings["View"].Value));


            // Commands.
            mComms["View"].Value     = new RelayCommand<AttachedCommandArgs>(View_Checked);
            mComms["OmniText"].Value = new RelayCommand<AttachedCommandArgs>(Omni_TextChanged);
            mComms["OmniSend"].Value = new RelayCommand<AttachedCommandArgs>(Omni_KeyDown);


            // Exit with good status.
            return true;
        }


        /// <summary>
        /// Handles changing the view whenever a button is selected.
        /// </summary>
        private void View_Checked(AttachedCommandArgs args)
        {
            // Set the next view.
            if ((String)args.Parameter != "Overview") {
                ExtensionApi.Settings["View"].Value = "Connection";
                ExtensionApi.Settings["ViewMoc"].Value = args.Parameter;
            }
            else {
                ExtensionApi.Settings["View"].Value = args.Parameter;
            }


            // Get the selected ring for animation.
            Image tSelected = mProps["Selected"].Value as Image;
            if (tSelected != null) {

                // Calculate the offset given which view we're navigating to.
                Int32 tOffset = 
                    (String)args.Parameter == "Players"  ? 52  :
                    (String)args.Parameter == "Bans"     ? 104 :
                    (String)args.Parameter == "Maps"     ? 156 :
                    (String)args.Parameter == "Plugins"  ? 208 :
                    (String)args.Parameter == "Settings" ? 260 :
                    /* Overview */ 0;

                // Animate the ring moving to the new value.
                Storyboard story = new Storyboard();
                Storyboard.SetTarget(story, tSelected);
                Storyboard.SetTargetProperty(story, new PropertyPath(Image.MarginProperty));

                ThicknessAnimation movement = new ThicknessAnimation();
                movement.DecelerationRatio = 1.0;
                movement.Duration = TimeSpan.FromMilliseconds(400);
                movement.From = tSelected.Margin;
                movement.To   = new Thickness(tOffset, 0, 0, 0);

                story.Children.Add(movement);
                story.Begin();
            }
        }
        

        /// <summary>
        /// Handles sending the omni command when enter is pressed.
        /// </summary>
        private void Omni_TextChanged(AttachedCommandArgs args)
        {
            TextChangedEventArgs tTextArgs = args.Args as TextChangedEventArgs;
            if (tTextArgs != null) {

                // TODO: Update omni command mProps.
                String tText = mProps["Omni"].Value as String;
            }
        }

        /// <summary>
        /// Handles sending the omni command when enter is pressed.
        /// </summary>
        private void Omni_KeyDown(AttachedCommandArgs args)
        {
            KeyEventArgs tKeyArgs = args.Args as KeyEventArgs;
            if (tKeyArgs != null && tKeyArgs.Key == Key.Return) {

                // TODO: Select or Send the omni command.
                mProps["Omni"].Value = String.Empty;
            }
        }
    }
}
