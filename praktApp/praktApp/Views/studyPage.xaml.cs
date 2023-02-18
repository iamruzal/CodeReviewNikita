using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace praktApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudyPage : ContentPage
    {
        public StudyPage()
        {
            InitializeComponent();
        }

        private async void VipolnitZauch_Tapped(object sender, EventArgs e)
        {
            if (Global.completeCategoriesUser.FirstOrDefault(x => x.IsChoose == true) != null)
            {
                await Navigation.PushAsync(new WordStudyPage());
            }
            else
            {
                await DisplayAlert("Ошибка", "Изучаемые категории не выбраны!!!", "Ок");
            }
        }

        private async void ChooseCategory_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectedCategoriesPage());
        }
    }
}