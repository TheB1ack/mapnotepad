using Prism.Behaviors;
using Prism.Common;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace MapNotepad.Behaviors
{
    class TabbedPageNavigationBehavior : BehaviorBase<TabbedPage>
    {
        private Page CurrentPage;

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

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            var newPage = AssociatedObject.CurrentPage;

            var parameters = new NavigationParameters();
            if (CurrentPage != null)
            {               
                PageUtilities.OnNavigatedFrom(CurrentPage, parameters);             
            }
            PageUtilities.OnNavigatedTo(newPage, parameters);

            CurrentPage = newPage;
        }
    }
}
