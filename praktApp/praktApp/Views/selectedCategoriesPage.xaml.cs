using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using testAnd;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ЛР7_ВПКС.Data;
using ЛР7_ВПКС.models;

namespace praktApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedCategoriesPage : ContentPage
    {
        public int idRole;
        public SelectedCategoriesPage()
        {
            InitializeComponent();
            idRole = Global.CurrentUser.RoleId;
        }
        protected override void OnAppearing()
        {
            collectionCategoryView.SelectedItem = null;
            collectionCategoryView.ItemsSource = Global.completeCategoriesUser;
            base.OnAppearing();
        }
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as CheckBox).BindingContext is CompleteCategory CurrentCompleteCategory)
                ElectronicBookDB.GetContext().SaveComplCatAsync(CurrentCompleteCategory);
        }
       

        private async void CreateNewCategoryButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateOrUpdateCategoryPage());
        }

        private async void collectionCategoryView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (collectionCategoryView.SelectedItem != null)
                await Navigation.PushAsync(new CreateOrUpdateCategoryPage((CompleteCategory)e.CurrentSelection.FirstOrDefault()));
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            collectionCategoryView.ItemsSource = Global.completeCategoriesUser.Where(p => p.Category.Name.ToLower().Contains(SearchTB.Text.ToLower()));
        }
    }
}