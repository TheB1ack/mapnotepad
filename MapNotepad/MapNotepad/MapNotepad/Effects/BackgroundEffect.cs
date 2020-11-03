using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MapNotepad.Effects
{
    public class BackgroundEffect: RoutingEffect
    {
        public BackgroundEffect() : base($"MyCompany.{nameof(BackgroundEffect)}")
        {
        }
    }
}
