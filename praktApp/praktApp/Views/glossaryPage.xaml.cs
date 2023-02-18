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
    public partial class GlossaryPage : ContentPage
    {

        public GlossaryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Global.UpdateListWords();
            collectionWordView.ItemsSource = Global.ChooseCategoriesWords;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            collectionWordView.ItemsSource = Global.ChooseCategoriesWords.Where(p => p.Term.ToLower().Contains(SearchTB.Text.ToLower()) || p.CategoryName.ToLower().Contains(SearchTB.Text.ToLower()) || p.Translate.ToLower().Contains(SearchTB.Text.ToLower()));
        }
    }
}