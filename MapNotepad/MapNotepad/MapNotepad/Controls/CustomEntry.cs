using Xamarin.Forms;

namespace MapNotepad.Controls
{
    public class CustomEntry : Entry
    {
        #region -- Public Properties --

        public bool IsTextValid
        {
            get => (bool)GetValue(IsTextValidProperty);

            set => SetValue(IsTextValidProperty, value);
        }

        public static readonly BindableProperty IsTextValidProperty = BindableProperty.Create(
                                                         propertyName: nameof(IsTextValid),
                                                         returnType: typeof(bool),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: IsTextValidPropertyChanged);

        #endregion

        #region -- Private Helpers --

        private static void IsTextValidPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var entry = bindable as CustomEntry;
            var isValid = (bool)newValue;

            if (entry != null)
            {
                if (isValid)
                {
                    entry.TextColor = Color.Default;
                }
                else
                {
                    entry.TextColor = Color.Red;
                }
            }

        }

        #endregion

    }
}
