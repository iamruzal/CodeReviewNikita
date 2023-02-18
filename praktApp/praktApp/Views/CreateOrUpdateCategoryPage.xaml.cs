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
    public partial class CreateOrUpdateCategoryPage : ContentPage
    {
        private Category CurrentCategory = new Category() { Words = new List<Word>(){ new Word() {Translate = "",Term = ""} } };
        private Word CurrentWord;
        private bool IsChanged = false;
        public CreateOrUpdateCategoryPage()
        {
            InitializeComponent();
            RenameCategory();
            BtnIn.IsVisible = false;
        }

        public CreateOrUpdateCategoryPage(CompleteCategory cat)
        {
            InitializeComponent();

            CurrentCategory = ElectronicBookDB.GetContext().GetCategory(cat.CategoryId);

            collectionWordView.ItemsSource = CurrentCategory.Words;

            if (!cat.CanChange)
            {
                ToolBarItemDelete.IsEnabled = false;
                ToolBarItemRename.IsEnabled = false;
                BtnIn.IsVisible = false;
            }

            contentPage.Title = cat.Category.Name;
        }

        

        protected async override void OnDisappearing()
        {

            if (CurrentCategory == null || !IsChanged)
            {
                return;
            }
            if (CurrentCategory.Words.Count == 0 || string.IsNullOrWhiteSpace(CurrentCategory.Words[0].Term) || string.IsNullOrWhiteSpace(CurrentCategory.Words[0].Translate))
            {
                if(CurrentCategory.CompleteCatList != null)
                {
                    ElectronicBookDB.GetContext().DeleteCategoryAsync(CurrentCategory).Wait();
                    Global.UpdateCompleteCategoriesUser();
                    return;
                }
                return;
            }
            if (CurrentCategory.CompleteCatList != null)
            {
                List<CompleteCategory> completeCategories = ElectronicBookDB.GetContext().GetComplCatAsync().Result.Where(P => P.CategoryId == CurrentCategory.Id).ToList();
                foreach(CompleteCategory completeCategory in completeCategories)
                {
                    completeCategory.IsStuded = false;
                    await ElectronicBookDB.GetContext().SaveComplCatAsync(completeCategory);
                }

                ElectronicBookDB.GetContext().SaveCategoryAsync(CurrentCategory).Wait();
                Global.UpdateCompleteCategoriesUser();
                return;
            }

            SaveCategory();

            //Что бы если пользователь сменил вкладку все норм сохранилось когда он выйдет вернеца
            CurrentCategory = ElectronicBookDB.GetContext().GetCategoriesAsync().Result.Last();
        }

        private void SaveCategory()
        {
            ElectronicBookDB.GetContext().SaveCategoryAsync(CurrentCategory).Wait();
            CurrentCategory = ElectronicBookDB.GetContext().GetCategoriesAsync().Result.Last();

            if(Global.CurrentUser.RoleId == 1)
            {
                CompleteCategory completeCategory = new CompleteCategory() { CanChange = true, User = Global.CurrentUser, Category = CurrentCategory, IsChoose = false, IsStuded = false };
                ElectronicBookDB.GetContext().SaveComplCatAsync(completeCategory).Wait();

                CurrentCategory.CompleteCatList.Add(completeCategory);
                ElectronicBookDB.GetContext().SaveCategoryAsync(CurrentCategory).Wait();

                Global.CurrentUser.CategoriesComlList.Add(completeCategory);
                ElectronicBookDB.GetContext().SaveUserAsync(Global.CurrentUser).Wait();
            }
            else
            {
                foreach(User user in ElectronicBookDB.GetContext().GetUsersAsync().Result.Where(p => p.ClassId == Global.CurrentUser.ClassId))
                {
                    CompleteCategory completeCategory = new CompleteCategory() { CanChange = false, User = user, Category = CurrentCategory, IsChoose = false, IsStuded = false };
                    if (user.Id == Global.CurrentUser.Id)
                        completeCategory.CanChange = true;
                    ElectronicBookDB.GetContext().SaveComplCatAsync(completeCategory).Wait();

                    CurrentCategory.CompleteCatList.Add(completeCategory);


                    user.CategoriesComlList.Add(completeCategory);
                    ElectronicBookDB.GetContext().SaveUserAsync(user).Wait();
                }
                ElectronicBookDB.GetContext().SaveCategoryAsync(CurrentCategory).Wait();
            }
            Global.DeserealizateUser();
        }

        private async void Editor_Completed(object sender, EventArgs e)
        {
            CurrentWord = CurrentCategory.Words.FirstOrDefault(p =>p.Term== (sender as Entry).Text || p.Translate == (sender as Entry).Text);
            if (CurrentWord == null)
                return;
            //Проверка на нулевые поля
            if (string.IsNullOrWhiteSpace(CurrentCategory.Words[CurrentCategory.Words.Count - 1].Term) || string.IsNullOrWhiteSpace(CurrentCategory.Words[CurrentCategory.Words.Count - 1].Translate))
            {
                return;
            }

            //проверка на существование подобных значений уже в категории
            foreach (Word w in CurrentCategory.Words)
            {
                if (w == CurrentWord)
                {
                    continue;
                }
                if (w.Term.ToLower().Trim(' ') == (sender as Entry).Text.ToLower().Trim(' '))
                {
                    await DisplayAlert("Ошибка создания", "Такой термин уже существует", "Ок");

                    return;
                }
                if (w.Translate.ToLower().Trim(' ') == (sender as Entry).Text.ToLower().Trim(' '))
                {
                    await DisplayAlert("Ошибка создания", "Такой перевод уже существует", "Ок");
                    return;
                }
            }

            IsChanged = true;
            ElectronicBookDB.GetContext().SaveWordAsync(CurrentWord).Wait();

            if (BtnIn.IsVisible)
            {
                ElectronicBookDB.GetContext().SaveCategoryAsync(CurrentCategory).Wait();
            }

            //Скролл до создания нового слова
            collectionWordView.ScrollTo(CurrentCategory.Words.Count);
            BtnIn.IsVisible = true;
        }

        private void CreateWordBut_Clicked(object sender, EventArgs e)
        {
            CreateNewWord();
        }

        private void CreateNewWord()
        {
            CurrentCategory.Words.Add(new Word() { Term = "", Translate = "" });

            collectionWordView.ItemsSource = null;
            collectionWordView.ItemsSource = CurrentCategory.Words;

            collectionWordView.ScrollTo(CurrentCategory.Words.Count);
            BtnIn.IsVisible = false;
        }

        private async void BtnDel_Clicked(object sender, EventArgs e)
        {
            CurrentWord = (sender as ImageButton).BindingContext as Word;
            if (await DisplayAlert("Подтвердить действие", "Вы хотите удалить слово?", "Да", "Нет"))
            {
                IsChanged = true;
                CurrentCategory.Words.Remove(CurrentWord);
                ElectronicBookDB.GetContext().DeleteWordAsync(CurrentWord).Wait();
                collectionWordView.ItemsSource = null;
                collectionWordView.ItemsSource = CurrentCategory.Words;
                collectionWordView.ScrollTo(CurrentCategory.Words.Count);
                BtnIn.IsVisible = true;
            }
            if(CurrentCategory.Words.Count == 0)
            {
                BtnIn.IsVisible = true;
            }
        }

        private async void ToolBarItemDelete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Подтвердить действие", "Вы хотите удалить категорию?", "Да", "Нет"))
            {
                if(CurrentCategory.CompleteCatList != null)
                {
                    ElectronicBookDB.GetContext().DeleteCategoryAsync(CurrentCategory).Wait();
                    Global.UpdateCompleteCategoriesUser();
                }
                CurrentCategory = null;
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void RenameCategory()
        {
            string CategoryName = await DisplayPromptAsync("Имя категории", "Введите наименование категории", "Ок", "Отмена");
            if(CategoryName == null)
            {
                await Shell.Current.GoToAsync("..");
            }
            // Проверка на ненулевое имя
            if (string.IsNullOrWhiteSpace(CategoryName) || CategoryName == null)
            {
                await DisplayAlert("Ошибка создания", "Необходимо заполнить поле", "Ок");
                RenameCategory();
                return;
            }

            //Проверка на незанятое имя
            if (ElectronicBookDB.GetContext().GetCategoriesAsync().Result.FirstOrDefault(x => x.Name.ToLower() == CategoryName.ToLower()) != null)
            {
                await DisplayAlert("Ошибка создания", "Имя категории занято", "Ок");
                RenameCategory();
                return;
            }

            CurrentCategory.Name = CategoryName;
            contentPage.Title = CategoryName;

            collectionWordView.ItemsSource = CurrentCategory.Words;
        }



        private void ToolBarItemRename_Clicked(object sender, EventArgs e)
        {
            RenameCategory();
        }
    }
}