using MapNotepad.Extentions;
using MapNotepad.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Clustering;

namespace MapNotepad.Control
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

            PinsSource = new ObservableCollection<CustomPin>();
        }

        public ObservableCollection<CustomPin> PinsSource
        {
            get
            {
                return (ObservableCollection<CustomPin>)GetValue(PinsSourceProperty);
            }
            set
            {
                SetValue(PinsSourceProperty, value);
            }
        }
        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                         propertyName: nameof(PinsSource),
                                                         returnType: typeof(ObservableCollection<CustomPin>),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: PinsSourcePropertyChanged);
        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var map = bindable as CustomMap;
            var newPinsSource = newValue as ObservableCollection<CustomPin>;

            if (map != null && newPinsSource != null)
            {
                UpdatePinsSource(map, newPinsSource);
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

        public CustomPin FocusedPin
        {
            get
            {
                return (CustomPin)GetValue(FocusedPinProperty);
            }
            set
            {
                SetValue(FocusedPinProperty, value);
            }
        }
        public static readonly BindableProperty FocusedPinProperty = BindableProperty.Create(
                                                         propertyName: nameof(FocusedPin),
                                                         returnType: typeof(CustomPin),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: OnFocusedPinPropertyChanged);
        private static void OnFocusedPinPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var map = bindable as CustomMap;
            var newPin = newValue as CustomPin;

            if (map != null && newPin != null)
            {
                FocuseOnPin(map, newPin);
            }
        }
        private static void FocuseOnPin(Map bindableMap, CustomPin newPin)
        {
            bool isAnimated = newPin.IsAnimated;
            var pin = newPin.ConvertToPin();

            bindableMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)), isAnimated);
        }

        public CustomPin OnlyOneFocusedPin
        {
            get
            {
                return (CustomPin)GetValue(OnlyOneFocusedPinProperty);
            }
            set
            {
                SetValue(OnlyOneFocusedPinProperty, value);
            }
        }
        public static readonly BindableProperty OnlyOneFocusedPinProperty = BindableProperty.Create(
                                                         propertyName: nameof(OnlyOneFocusedPin),
                                                         returnType: typeof(CustomPin),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: OnOnlyOneFocusedPinPropertyChanged);
        private static void OnOnlyOneFocusedPinPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {

            var map = bindable as CustomMap;
            var newPin = newValue as CustomPin;

            var collection = new ObservableCollection<CustomPin>() { newPin };

            if (map != null && newPin != null)
            {
                UpdatePinsSource(map, collection);
                FocuseOnPin(map, newPin);
            }
        }

        public CameraPosition CameraPositionOnMap
        {
            get
            {
                return (CameraPosition)GetValue(CameraPositionOnMapProperty);
            }
            set
            {
                SetValue(CameraPositionOnMapProperty, value);
            }
        }
        public static readonly BindableProperty CameraPositionOnMapProperty = BindableProperty.Create(
                                                         propertyName: nameof(CameraPositionOnMap),
                                                         returnType: typeof(CameraPosition),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: CameraPositionOnMapPropertyChanged);
        private static void CameraPositionOnMapPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {

            var map = bindable as CustomMap;
            var newPosition = newValue as CameraPosition;

            if (map != null && newPosition != null)
            {
                MoveCameraToPosition(map, newPosition);
            }
        }
        private static void MoveCameraToPosition(Map bindableMap, CameraPosition newPosition)
        {
            var position = new Position(newPosition.Target.Latitude, newPosition.Target.Longitude);
            var cameraUpdate = CameraUpdateFactory.NewPositionZoom(position, newPosition.Zoom);

            bindableMap.InitialCameraUpdate = cameraUpdate;
            bindableMap.MoveCamera(cameraUpdate);
        }
    }
}
