using MapNotepad.Extentions;
using MapNotepad.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Clustering;

namespace MapNotepad.Controls
{
    public class CustomMap : ClusteredMap
    {
        public CustomMap()
        {
            if (UiSettings != null)
            {
                UiSettings.ZoomControlsEnabled = false;
                UiSettings.MyLocationButtonEnabled = true;
            }
            else
            {
                Debug.WriteLine("UiSetting was null");
            }

            PinsSource = new ObservableCollection<CustomPin>();
        }

        #region -- Public Properties --

        public ObservableCollection<CustomPin> PinsSource
        {
            get => (ObservableCollection<CustomPin>)GetValue(PinsSourceProperty);

            set => SetValue(PinsSourceProperty, value);
        }
        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                         propertyName: nameof(PinsSource),
                                                         returnType: typeof(ObservableCollection<CustomPin>),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: PinsSourcePropertyChanged);

        public CustomPin FocusedPin
        {
            get => (CustomPin)GetValue(FocusedPinProperty);

            set => SetValue(FocusedPinProperty, value);
        }
        public static readonly BindableProperty FocusedPinProperty = BindableProperty.Create(
                                                 propertyName: nameof(FocusedPin),
                                                 returnType: typeof(CustomPin),
                                                 declaringType: typeof(CustomMap),
                                                 defaultValue: null,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 validateValue: null,
                                                 propertyChanged: OnFocusedPinPropertyChanged);

        public CustomPin OnlyOneFocusedPin
        {
            get => (CustomPin)GetValue(OnlyOneFocusedPinProperty);
            
            set => SetValue(OnlyOneFocusedPinProperty, value);
        }
        public static readonly BindableProperty OnlyOneFocusedPinProperty = BindableProperty.Create(
                                                         propertyName: nameof(OnlyOneFocusedPin),
                                                         returnType: typeof(CustomPin),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: OnOnlyOneFocusedPinPropertyChanged);

        public CameraPosition CameraPositionOnMap
        {
            get => (CameraPosition)GetValue(CameraPositionOnMapProperty);

            set => SetValue(CameraPositionOnMapProperty, value);
        }
        public static readonly BindableProperty CameraPositionOnMapProperty = BindableProperty.Create(
                                                         propertyName: nameof(CameraPositionOnMap),
                                                         returnType: typeof(CameraPosition),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: OnCameraPositionOnMapPropertyChanged);

        #endregion

        #region -- Private Helpers --

        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var map = bindable as CustomMap;
            var newPinsSource = newValue as ObservableCollection<CustomPin>;

            if (map != null && newPinsSource != null)
            {
                UpdatePinsSource(map, newPinsSource);
            }
            else
            {
                Debug.WriteLine("map or newPinsSource was null ");
            }

        }

        private static void OnFocusedPinPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var map = bindable as CustomMap;
            var newPin = newValue as CustomPin;

            if (map != null && newPin != null)
            {
                FocuseOnPin(map, newPin);
            }
            else
            {
                Debug.WriteLine("map or newPin was null ");
            }

        }

        private static void OnOnlyOneFocusedPinPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var map = bindable as CustomMap;
            var newPin = newValue as CustomPin;

            var collection = new ObservableCollection<CustomPin>() { newPin };

            if (map != null && newPin != null)
            {
                var position = new Position(newPin.PositionLat, newPin.PositionLong);

                MoveCameraToPosition(map, new CameraPosition(position, 15));
                UpdatePinsSource(map, collection);
            }
            else
            {
                Debug.WriteLine("map or newPin was null ");
            }

        }

        private static void OnCameraPositionOnMapPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {

            var map = bindable as CustomMap;
            var newPosition = newValue as CameraPosition;

            if (map != null && newPosition != null)
            {
                MoveCameraToPosition(map, newPosition);
            }
            else
            {
                Debug.WriteLine("map or newPosition was null ");
            }

        }

        private static void UpdatePinsSource(Map bindableMap, IEnumerable<CustomPin> newSource)
        {
            bindableMap.Pins.Clear();

            foreach (var item in newSource)
            {
                var pin = item.ConvertToPin();
                bindableMap.Pins.Add(pin);
            }
        }

        private static void FocuseOnPin(Map bindableMap, CustomPin newPin)
        {
            var pin = newPin.ConvertToPin();

            bindableMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)), true);
        }

        private static void MoveCameraToPosition(Map bindableMap, CameraPosition newPosition)
        {
            var position = new Position(newPosition.Target.Latitude, newPosition.Target.Longitude);
            var cameraUpdate = CameraUpdateFactory.NewPositionZoom(position, newPosition.Zoom);

            bindableMap.InitialCameraUpdate = cameraUpdate;
            bindableMap.MoveCamera(cameraUpdate);
        }

        #endregion

    }
}
