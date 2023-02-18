using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР7_ВПКС.models
{
    public class CompleteCategory
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [ForeignKey(typeof(User))]
        public int UserId { get; set; }
        [ForeignKey(typeof(Category))]
        public int CategoryId { get; set; }
        [ManyToOne]
        public User User { get; set; }
        [ManyToOne]
        public Category Category { get; set; }

        public bool IsStuded { get; set; }
        public bool IsChoose { get; set; }

        public string IsStudedText
        {
            get
            {
                return IsStuded ? "Изучено" : "Не изучено";
            }
        }
        public bool CanChange { get; set; }
    }
}
