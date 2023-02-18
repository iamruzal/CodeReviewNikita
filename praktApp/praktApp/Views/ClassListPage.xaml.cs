using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ЛР7_ВПКС.Data;

namespace praktApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassListPage : ContentPage
    {
        public ClassListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            collectionViewUser.ItemsSource = null;
            collectionViewUser.ItemsSource = ElectronicBookDB.GetContext().GetUsersAsync().Result.Where(P => P.ClassId == Global.CurrentUser.ClassId && P.RoleId == 1).ToList();
        }
    }
}