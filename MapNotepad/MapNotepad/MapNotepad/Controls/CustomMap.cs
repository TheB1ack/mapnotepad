using MapNotepad.Extentions;
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
        public ObservableCollection<Pin> PinsSource
        {
            get { return (ObservableCollection<Pin>)GetValue(PinsSourceProperty); }
            set { SetValue(PinsSourceProperty, value); }
        }

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                         propertyName: "PinsSource",
                                                         returnType: typeof(ObservableCollection<Pin>),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: PinsSourcePropertyChanged);

        public CustomMap()
        {
            PinsSource = new ObservableCollection<Pin>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;
        }
        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var thisInstance = bindable as CustomMap;
            var newPinsSource = newValue as ObservableCollection<Pin>;

            if (thisInstance != null && newPinsSource != null)
            {
                UpdatePinsSource(thisInstance, newPinsSource);
            }
        }
        private void PinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePinsSource(this, sender as IEnumerable<Pin>);
        }

        private static void UpdatePinsSource(Map bindableMap, IEnumerable<Pin> newSource)
        {
            bindableMap.Pins.Clear();
            foreach (var pin in newSource)
            {
                bindableMap.Pins.Add(pin);
            }
        }

    }
}
