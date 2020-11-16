using SQLite;

namespace MapNotepad.Models
{
    [Table("Users")]
    public class User : IBaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
