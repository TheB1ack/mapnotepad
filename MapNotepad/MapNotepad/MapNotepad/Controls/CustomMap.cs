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
                Debug.WriteLine("UiSettings is null");
            }

            PinsSource = new ObservableCollection<CustomPin>();
        }

        #region -- Public Properties --

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                         propertyName: nameof(PinsSource),
                                                         returnType: typeof(ObservableCollection<CustomPin>),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: OnPinsSourcePropertyChanged);
        public ObservableCollection<CustomPin> PinsSource
        {
            get => (ObservableCollection<CustomPin>)GetValue(PinsSourceProperty);
            set => SetValue(PinsSourceProperty, value);
        }

        public static readonly BindableProperty FocusedPinProperty = BindableProperty.Create(
                                                 propertyName: nameof(FocusedPin),
                                                 returnType: typeof(CustomPin),
                                                 declaringType: typeof(CustomMap),
                                                 defaultValue: null,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 validateValue: null,
                                                 propertyChanged: OnFocusedPinPropertyChanged);
        public CustomPin FocusedPin
        {
            get => (CustomPin)GetValue(FocusedPinProperty);
            set => SetValue(FocusedPinProperty, value);
        }

        public static readonly BindableProperty CameraPositionOnMapProperty = BindableProperty.Create(
                                                         propertyName: nameof(CameraPositionOnMap),
                                                         returnType: typeof(CameraPosition),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: OnCameraPositionOnMapPropertyChanged);
        public CameraPosition CameraPositionOnMap
        {
            get => (CameraPosition)GetValue(CameraPositionOnMapProperty);
            set => SetValue(CameraPositionOnMapProperty, value);
        }

        #endregion

        #region -- Private Helpers --

        private static void OnPinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var map = bindable as CustomMap;
            var newPinsSource = newValue as ObservableCollection<CustomPin>;

            if (map != null && newPinsSource != null)
            {
                UpdatePinsSource(map, newPinsSource);
            }
            else
            {
                Debug.WriteLine("map or newSource is null");
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
                Debug.WriteLine("map or newPin is null");
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
                Debug.WriteLine("map or newPosition is null");
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
