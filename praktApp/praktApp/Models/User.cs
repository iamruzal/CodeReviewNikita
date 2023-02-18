using praktApp;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР7_ВПКС.models
{
    public class User
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [NotNull]
        public string Fullname { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public string Papaname { get; set; }

        public string FIO { get { return $"{Fullname} {Name} {Papaname}"; } }

        [NotNull]
        public string Email { get; set; }
        [NotNull]
        public string Password { get; set; }
        [ForeignKey(typeof(Role))]
        public int RoleId { get; set; }
        [ManyToOne]
        public Role Role { get; set; }
        [OneToMany]
        public List<CompleteCategory> CategoriesComlList { get; set; }

        [ForeignKey(typeof(Class))]
        public int ClassId { get; set; }
        [ManyToOne]
        public Class Class { get; set; }

        public int StudedCategory { get
            {
                return CategoriesComlList.Where(x => x.IsStuded).ToList().Count;
            } }
        public int ChooseCategory
        {
            get
            {
                return CategoriesComlList.Where(x => x.IsChoose).ToList().Count;
            }
        }
    }
}
