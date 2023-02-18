using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ЛР7_ВПКС.Data;
using ЛР7_ВПКС.models;

namespace praktApp
{
    public static class Global
    {
        public static User CurrentUser;
        /// <summary>
        /// Список выбранных пользователем категорий
        /// </summary>
        public static List<CompleteCategory> completeCategoriesUser;

        public static List<Word> ChooseCategoriesWords;

        /// <summary>
        /// Путь к айди текущего пользователя
        /// </summary>
        private static string CurUserPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CurrentUser.xml");
        /// <summary>
        /// Авторизовавшийся пользователь
        /// </summary>
        /// <param name="user"></param>
        public static void UpdateCompleteCategoriesUser()
        {
            List<CompleteCategory> completeCategories = ElectronicBookDB.GetContext().GetComplCatAsync().Result;
            completeCategoriesUser = completeCategories.Where(x => CurrentUser.CategoriesComlList.FirstOrDefault(y => y.Id == x.Id) != null).ToList();
        }

        public static void SerializateUser(User user)
        {
            CurrentUser = user;
            File.WriteAllText(CurUserPath, JsonConvert.SerializeObject(user.Id));
            UpdateCompleteCategoriesUser();
        }

        public static void DeserealizateUser()
        {
            int id = JsonConvert.DeserializeObject<int>(File.ReadAllText(CurUserPath));
            CurrentUser = ElectronicBookDB.GetContext().GetUsersAsync().Result.FirstOrDefault(i => i.Id == id);
            completeCategoriesUser = ElectronicBookDB.GetContext().GetComplCatAsync().Result.Where(x => CurrentUser.CategoriesComlList.Select(p => p.Id).Contains(x.Id)).ToList();
        }

        public static void DeleteFileUserId()
        {
            File.Delete(CurUserPath);
        }

        public static bool IsFileWithUserExist()
        {
            return File.Exists(CurUserPath);
        }

        public static void UpdateListWords()
        {
            ChooseCategoriesWords = new List<Word>();
            List<Category> categories = ElectronicBookDB.GetContext().GetCategoriesAsync().Result.Where(x => completeCategoriesUser.FirstOrDefault(y => y.CategoryId == x.Id && y.IsChoose) != null).ToList();
            foreach (Category category in categories)
            {
                ChooseCategoriesWords.AddRange(category.Words);
            }
        }
    }
}
