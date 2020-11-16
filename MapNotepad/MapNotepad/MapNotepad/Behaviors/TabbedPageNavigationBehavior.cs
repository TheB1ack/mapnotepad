using Prism.Behaviors;
using Prism.Common;
using Prism.Navigation;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MapNotepad.Behaviors
{
    public class TabbedPageNavigationBehavior : BehaviorBase<TabbedPage>
    {
        private Page CurrentPage;

        #region -- BehaviorBase implementation --

        protected override void OnAttachedTo(TabbedPage bindable)
        {
            bindable.CurrentPageChanged += OnCurrentPageChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(TabbedPage bindable)
        {
            bindable.CurrentPageChanged -= OnCurrentPageChanged;
            base.OnDetachingFrom(bindable);
        }

        #endregion

        #region -- Private helpers --

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            var newPage = AssociatedObject.CurrentPage;

            var parameters = new NavigationParameters();

            if (CurrentPage != null)
            {               
                PageUtilities.OnNavigatedFrom(CurrentPage, parameters);             
            }
            else
            {
                Debug.WriteLine("CurrentPage is null");
            }

            PageUtilities.OnNavigatedTo(newPage, parameters);

            CurrentPage = newPage;
        }

        #endregion

    }
}
