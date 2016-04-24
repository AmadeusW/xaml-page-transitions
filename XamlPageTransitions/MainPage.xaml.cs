using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XamlPageTransitions
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            toTransitions.ItemsSource = TransitionProvider.TransitionNames;
            toTransitions.SelectedIndex = TransitionProvider.GetSelectedIndex("to");
            fromTransitions.ItemsSource = TransitionProvider.TransitionNames;
            fromTransitions.SelectedIndex = TransitionProvider.GetSelectedIndex("from");
        }

        #region Event handlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), parameter: false);
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage), parameter: true);
        }

        private void toTransitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTypeName = toTransitions.SelectedItem.ToString();
            var selectedIndex = toTransitions.SelectedIndex;
            var transition = TransitionProvider.SelectTransition("to", selectedTypeName, selectedIndex);
            
            if (transition != null)
            {
                dynamic transitionType = Activator.CreateInstance(transition);
                this.Transitions = new TransitionCollection() { transitionType };
            }
        }

        private void fromTransitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTypeName = fromTransitions.SelectedItem.ToString();
            var selectedIndex = fromTransitions.SelectedIndex;
            var transition = TransitionProvider.SelectTransition("from", selectedTypeName, selectedIndex);

            if (transition != null)
            {
                dynamic transitionType = Activator.CreateInstance(transition);
                this.Transitions = new TransitionCollection() { transitionType };
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e?.Parameter == null || !(e.Parameter is bool))
                return;
            setUpTransitionDirection(!(bool)e.Parameter); // note: parameter is reverted
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e?.Parameter == null || !(e.Parameter is bool))
                return;
            setUpTransitionDirection((bool)e.Parameter); 
        }

        #endregion

        private void setUpTransitionDirection(bool towardsRight)
        {
            var transition = this.Transitions.FirstOrDefault();
            if (transition == null)
                return;

            if (transition is EdgeUIThemeTransition)
            {
                var edgeAnimation = transition as EdgeUIThemeTransition;
                edgeAnimation.Edge = towardsRight ? EdgeTransitionLocation.Right : EdgeTransitionLocation.Left;
            }
            else if (transition is EntranceThemeTransition)
            {
                var entranceAnimation = transition as EntranceThemeTransition;
                entranceAnimation.FromHorizontalOffset = towardsRight ? 300 : -300;
            }
            else if (transition is PopupThemeTransition)
            {
                var popupAnimation = transition as PopupThemeTransition;
                popupAnimation.FromHorizontalOffset = towardsRight ? 300 : -300;
            }
            else if (transition is ContentThemeTransition)
            {
                var contentAnimation = transition as ContentThemeTransition;
                contentAnimation.HorizontalOffset = towardsRight ? 300 : -300;
            }
            else if (transition is PaneThemeTransition)
            {
                var paneAnimation = transition as PaneThemeTransition;
                paneAnimation.Edge = towardsRight ? EdgeTransitionLocation.Right : EdgeTransitionLocation.Left;
            }
        }
    }
}

