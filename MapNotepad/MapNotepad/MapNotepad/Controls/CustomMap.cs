﻿using MapNotepad.Extentions;
using MapNotepad.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
    
namespace MapNotepad.Control
{
    public class CustomMap : Map
    {
        public CustomMap()
        {
            UiSettings.ZoomControlsEnabled = false;
            UiSettings.MyLocationButtonEnabled = true;
            PinsSource = new ObservableCollection<CustomPin>();
            //PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;
        }

        public ObservableCollection<CustomPin> PinsSource
        {
            get { return (ObservableCollection<CustomPin>)GetValue(PinsSourceProperty); }
            set { SetValue(PinsSourceProperty, value); }
        }

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                         propertyName: "PinsSource",
                                                         returnType: typeof(ObservableCollection<CustomPin>),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: PinsSourcePropertyChanged);

        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var thisInstance = bindable as CustomMap;
            var newPinsSource = newValue as ObservableCollection<CustomPin>;

            if (thisInstance != null && newPinsSource != null)
            {
                UpdatePinsSource(thisInstance, newPinsSource);
            }
        }
        //private void PinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    UpdatePinsSource(this, sender as IEnumerable<Pin>);
        //}

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
            get { return (CustomPin)GetValue(FocusedPinProperty); }
            set { SetValue(FocusedPinProperty, value); }
        }

        public static readonly BindableProperty FocusedPinProperty = BindableProperty.Create(
                                                         propertyName: "FocusedPin",
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
            
            FocuseOnPin(map, newPin);
        }
        private static void FocuseOnPin(Map map, CustomPin newPin)
        {
            if (newPin != null)
            {
                bool isAnimated = newPin.IsAnimated;
                var pin = newPin.ConvertToPin();

                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)), isAnimated);
            }
        }

        public CustomPin OnlyOneFocusedPin
        {
            get { return (CustomPin)GetValue(OnlyOneFocusedPinProperty); }
            set { SetValue(OnlyOneFocusedPinProperty, value); }
        }

        public static readonly BindableProperty OnlyOneFocusedPinProperty = BindableProperty.Create(
                                                         propertyName: "OnlyOneFocusedPin",
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
            var collection = new ObservableCollection<CustomPin>(){ newPin};
  
            UpdatePinsSource(map, collection);
            FocuseOnPin(map, newPin);
        }
    }
}
