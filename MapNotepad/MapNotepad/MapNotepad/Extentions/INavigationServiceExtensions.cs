using Prism.Common;
using Prism.Navigation;
using System;
using System.Linq;
using Xamarin.Forms;

namespace MapNotepad.Extentions
{
    public static class INavigationServiceExtensions
    {
        public static INavigationResult FixedSelectTab(this INavigationService navigationService, Type targetPageType, INavigationParameters parameters = null)
        {
            var result = new NavigationResult();

            try
            {
                TabbedPage parentPage = null;

                var page = ((IPageAware)navigationService).Page;

                if (page is TabbedPage tp1)
                {
                    parentPage = tp1;
                }
                else if (page.Parent is TabbedPage tp2)
                {
                    parentPage = tp2;
                }
                else
                {
                    throw new ArgumentException("Call outside of tabbed page");
                }

                var currentPage = parentPage.CurrentPage;
                var targetPage = parentPage.Children.First(x => x.GetType() == targetPageType);

                parentPage.CurrentPage = targetPage;

                PageUtilities.OnNavigatedFrom(currentPage, parameters);
                PageUtilities.OnNavigatedTo(targetPage, parameters);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }
    }
}
