using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ЛР7_ВПКС.Data;
using ЛР7_ВПКС.models;

namespace praktApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecoverPasswordPage : ContentPage
    {
        public RecoverPasswordPage()
        {
            InitializeComponent();
        }

        private User user;
        private async void ContBut_Clicked(object sender, EventArgs e)
        {
            if(EmailEntry.Placeholder == "Введите новый пароль")
            {
                user.Password = EmailEntry.Text;
                try
                {
                    await ElectronicBookDB.GetContext().SaveUserAsync(user);
                    await DisplayAlert("Изменение пароля", "Пароль успешно изменен", "ОК");
                    await Navigation.PopAsync();
                    return;
                }
                catch(Exception ex)
                {
                    await DisplayAlert("Ошибка", ex.Message, "ОК");
                }
            }
            user = ElectronicBookDB.GetContext().GetUsersAsync().Result.FirstOrDefault(x => x.Email == EmailEntry.Text);
            if(user == null)
            {
                await DisplayAlert("Ошибка", "Пользователя с такой почтой не существует", "ОК");
                return;
            }
            EmailEntry.Text = "";
            EmailEntry.Placeholder = "Введите новый пароль";
        }
    }
}