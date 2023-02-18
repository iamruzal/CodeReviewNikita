using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ЛР7_ВПКС.Data;
using ЛР7_ВПКС.models;

namespace praktApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WordStudyPage : ContentPage
    {

        private List<Category> categories;
        private List<CompleteCategory> completeCategoriesForStudy;
        private int NumberCurrentCategory = 0;
        private int NumberCurrentWordInCategory = 0;
        private bool[] isError;
        private int Counterror;
        private double step;

        public WordStudyPage()
        {
            InitializeComponent();

            //Отключение тулбара
            Shell.SetTabBarIsVisible(this, false);

            completeCategoriesForStudy = Global.completeCategoriesUser.Where(x => x.IsChoose == true).ToList();

            categories = ElectronicBookDB.GetContext().GetCategoriesAsync().Result.Where(x => completeCategoriesForStudy.Select(y => y.CategoryId).Contains(x.Id)).ToList();
            
            // Айди категорий выполненных с ошибками
            isError = new bool[categories.Count];
            LabelTerm.Text = categories[0].Words[0].Translate;

            // Высчитываем шаг прогресс бара
            int countWord = 0;
            foreach(Category category in categories)
            {
                countWord += category.Words.Count;
            }
            step = 1.0 / countWord;
            EntTB.Focus();
        }

        private async void EntTB_Completed(object sender, EventArgs e)
        {

            if (EntTB.Text.ToLower().Trim(' ') == categories[NumberCurrentCategory].Words[NumberCurrentWordInCategory].Term.ToLower().Trim(' '))
            {
                //Верно
            }
            else
            {
                if(!await DisplayAlert("Ошибка", "Верный термин " + categories[NumberCurrentCategory].Words[NumberCurrentWordInCategory].Term, "Я ответил верно", "Далее"))
                {
                    isError[NumberCurrentCategory] = true;
                    Counterror++;
                }
            }

            Progress.Progress += step;

            //Если было последнее слово в категории
            if (NumberCurrentWordInCategory == (categories[NumberCurrentCategory].Words.Count - 1))
            {
                //Если последняя категория
                if(NumberCurrentCategory == (categories.Count - 1))
                {
                    int i = 0;
                    foreach(Category cat in categories)
                    {
                        if (isError[i] == false)
                        {
                            CompleteCategory completeCategory = completeCategoriesForStudy.FirstOrDefault(x => x.CategoryId == cat.Id);
                            completeCategory.IsStuded = true;
                            await ElectronicBookDB.GetContext().SaveComplCatAsync(completeCategory);
                        }
                        i++;
                    }
                    await DisplayAlert("Обучение завершено", $"Категорий без ошибок { isError.Where(p => p == false).ToArray().Count()}.\nВсего ошибок:{Counterror}", "Ок");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
                NumberCurrentWordInCategory = -1;
                NumberCurrentCategory++;
            }
            NumberCurrentWordInCategory++;
            LabelTerm.Text = categories[NumberCurrentCategory].Words[NumberCurrentWordInCategory].Translate;
            EntTB.Text = "";
            EntTB.Focus();
        }
    }
}