using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР7_ВПКС.models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        [ForeignKey(typeof(User))]
        public int UserId { get; set; }
        [OneToMany]
        public List<Word> Words { get; set; }
        [OneToMany]
        public List<CompleteCategory> CompleteCatList { get; set; }
    }
}
