using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ЛР7_ВПКС.models;
using testAnd;
using ЛР7_ВПКС.Data;
using System.IO;
using Newtonsoft.Json;

namespace praktApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutorizationPage : ContentPage
    {
        public AutorizationPage()
        {
            InitializeComponent();
        }

        private void ButEnter_Clicked(object sender, EventArgs e)
        {
            User user = ElectronicBookDB.GetContext().GetUsersAsync().Result.FirstOrDefault(x => x.Email == EmailEntry.Text && x.Password == PasswordEntry.Text);
            if (user == null)
            {
                DisplayAlert("Ошибка", "Неправильный логин или пароль!", "Ок");
                EmailEntry.Text = "";
                PasswordEntry.Text = "";
                return;
            }
            Global.SerializateUser(user);
            Application.Current.MainPage = new AppShell();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RecoverPasswordPage());
        }
    }
}