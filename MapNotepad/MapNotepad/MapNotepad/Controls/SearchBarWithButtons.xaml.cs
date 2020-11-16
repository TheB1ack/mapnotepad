using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchBarWithButtons : ContentView
    {

        public SearchBarWithButtons()
        {
            InitializeComponent();
        }

        #region -- Public Properties --

        public static event EventHandler<TextChangedEventArgs> TextChanged;

        public ICommand SettingsClick
        {
            get => (ICommand)GetValue(SettingsClickProperty);

            set => SetValue(SettingsClickProperty, value);
        }

        public static readonly BindableProperty SettingsClickProperty = BindableProperty.Create(
                                                         propertyName: nameof(SettingsClick),
                                                         returnType: typeof(ICommand),
                                                         declaringType: typeof(SearchBarWithButtons),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);

            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
                                                         propertyName: nameof(Placeholder),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(SearchBarWithButtons),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null);

        public string Text
        {
            get => (string)GetValue(TextProperty);

            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                         propertyName: nameof(Text),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(SearchBarWithButtons),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: (bindable, oldValue, newValue) => ((SearchBarWithButtons)bindable).OnTextChanged((string)oldValue, (string)newValue));

        #endregion

        #region -- Private Helpers --

        private void CancelButton_Click(object sender, EventArgs args)
        {
            Text = null;
        }

        protected virtual void OnTextChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                Cancel_Button.IsVisible = false;
            }
            else
            {
                Cancel_Button.IsVisible = true;
            }

            TextChanged?.Invoke(this, new TextChangedEventArgs(oldValue, newValue));
        }

        #endregion

    }
}