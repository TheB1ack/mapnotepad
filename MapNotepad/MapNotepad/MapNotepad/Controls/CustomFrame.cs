using System.Diagnostics;
using Xamarin.Forms;

namespace MapNotepad.Controls
{
    public class CustomFrame : Frame
    {
        #region -- Public Properties --

        public bool IsMoveFrame
        {
            get => (bool)GetValue(IsMoveFrameProperty);

            set => SetValue(IsMoveFrameProperty, value);
        }

        public static readonly BindableProperty IsMoveFrameProperty = BindableProperty.Create(
                                                         propertyName: nameof(IsMoveFrame),
                                                         returnType: typeof(bool),
                                                         declaringType: typeof(CustomFrame),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: IsMoveFramePropertyChanged);

        #endregion

        #region -- Private Helpers --

        private static void IsMoveFramePropertyChanged(BindableObject bindable, object oldvalue, object newValue) 
        {
            var frame = bindable as CustomFrame;
            var isValid = (bool)newValue;

            if (frame != null)
            {
                if (isValid)
                {
                    frame.TranslateTo(0, -130, 400);
                }
                else
                {
                    frame.TranslateTo(0, 40, 500);
                }
            }
            else
            {
                Debug.WriteLine("frame is null");
            }

        }

        #endregion
    }
}
