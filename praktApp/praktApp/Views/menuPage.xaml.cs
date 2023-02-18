using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testAnd;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ЛР7_ВПКС.Data;

namespace praktApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            StackUserform.BindingContext = Global.CurrentUser;
        }

        protected override void OnAppearing()
        {
            Global.CurrentUser = ElectronicBookDB.GetContext().GetUser(Global.CurrentUser.Id);
            StackUserform.BindingContext = Global.CurrentUser;
            base.OnAppearing();
        }

        private void ExitBut_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new AutorizationPage());
        }
    }
}