# xaml-page-transitions
A demonstration of xaml page transitions in Universal Apps

See [videos of all transitions and explained code snippets on my blog](http://amadeusw.com/xaml/animated-navigation-universal-app).

# Notable code:

To create an illusion of swiping in one particular direction

```
        // In the navigation logic:
        private void navigateRight()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MyView), parameter: true);
        }
        
        // In the view:
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
```
