using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Models
{
    [Table("CustomPin")]
    public class CustomPin : IBaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public double PositionLat { get; set; }
        public double PositionLong { get; set; }

        [Ignore]
        public bool IsAnimated { get; set; }
    }
}
