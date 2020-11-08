using SQLite;

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
        public bool IsFavourite { get; set; }
        public string FavouriteImageSource { get; set; }
        public int Category { get; set; }

        [Ignore]
        public bool IsAnimated { get; set; }
    }
}
