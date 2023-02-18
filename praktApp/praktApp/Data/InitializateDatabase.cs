using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ЛР7_ВПКС.Data;
using ЛР7_ВПКС.models;

namespace praktApp.Data
{
    class InitializateDatabase
    {
        public static async void  InitializateDB()
        {
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ElectronicBookDB.db3")))
            {
                ElectronicBookDB DB = ElectronicBookDB.GetContext();

                //Добавление ролей
                await DB.SaveRoleAsync(new Role() { Name = "Ученик", Users = new List<User>() });
                await DB.SaveRoleAsync(new Role() { Name = "Учитель", Users = new List<User>() });


                //Добавление классов
                await DB.SaveClassAsync(new Class() { Name = "4a", Users = new List<User>() });
                await DB.SaveClassAsync(new Class() { Name = "5б", Users = new List<User>() });

                //Инициализация слов и категорий
                List<Word> words1 = new List<Word>() {
                    new Word() { Term = "a storage medium", Translate = "Носитель ЗУ" },
                    new Word() { Term = "distinguishing characteristics", Translate = "Отличительные свойства" },
                    new Word() { Term = "in terms of", Translate = "с точки зрения" },
                    new Word() { Term = "automated", Translate = "автоматизированные" },
                    new Word() { Term = "primarily", Translate = "главным образом" }
                };

                foreach (Word word in words1)
                {
                    await DB.SaveWordAsync(word);
                }

                List<Word> words2 = new List<Word>() {
                    new Word() { Term = "cover", Translate = "охватывать" },
                    new Word() { Term = "spurred", Translate = "побудить" },
                    new Word() { Term = "file transfer", Translate = "пересылка файлов" },
                    new Word() { Term = "file sharing", Translate = "Совместное пользование файлом" },
                    new Word() { Term = "web browsing", Translate = "просмотр информационной сети" }
                };

                foreach (Word word in words2)
                {
                    await DB.SaveWordAsync(word);
                }

                Category category1 = new Category() { Name = "The Internet 1" };
                Category category2 = new Category() { Name = "The Internet 2" };

                await DB.SaveCategoryAsync(category1);
                await DB.SaveCategoryAsync(category2);

                category1.Words = words1;
                category2.Words = words2;

                await DB.SaveCategoryAsync(category1);
                await DB.SaveCategoryAsync(category2);

                User user1 = new User() { Name = "Никита", Fullname = "Кравченко", Papaname = "Алексеевич", Email = "nikita@mail.ru", Password = "123" };
                await DB.SaveUserAsync(user1);

                Class @class = DB.GetClass(1);
                Role role =  DB.GetRole(1);
                @class.Users.Add(user1);
                role.Users.Add(user1);
                await DB.SaveClassAsync(@class);
                await DB.SaveRoleAsync(role);

                User user3 = new User() { Name = "Диляра", Fullname = "Сайфутдинова", Papaname = "Искандеровна", Email = "disaif@mail.ru", Password = "123" };
                await DB.SaveUserAsync(user3);

                @class.Users.Add(user3);
                role.Users.Add(user3);
                await DB.SaveClassAsync(@class);
                await DB.SaveRoleAsync(role);

                User user2 = new User() { Name = "Алексей", Fullname = "Еркашов", Papaname = "Сергеевич", Email = "alex@mail.ru", Password = "12345" };
                await DB.SaveUserAsync(user2);

                Role role2 =  DB.GetRole(2);
                @class.Users.Add(user2);
                role2.Users.Add(user2);
                await DB.SaveClassAsync(@class);
                await DB.SaveRoleAsync(role2);

                User user4 = new User() { Name = "Рамиль", Fullname = "Мухтаров", Papaname = "Расимович", Email = "ram@mail.ru", Password = "123" };
                await DB.SaveUserAsync(user4);

                @class = DB.GetClass(2);
                role = DB.GetRole(1);

                @class.Users.Add(user4);
                role.Users.Add(user4);
                await DB.SaveClassAsync(@class);
                await DB.SaveRoleAsync(role);

                CompleteCategory completeCategory1 = new CompleteCategory() {Category = category1, User = user1, IsChoose = false, IsStuded = false, CanChange = false };
                CompleteCategory completeCategory2 = new CompleteCategory() {Category = category2, User = user1, IsChoose = false, IsStuded = false, CanChange = false };

                CompleteCategory completeCategory3 = new CompleteCategory() { Category = category1, User = user3, IsChoose = false, IsStuded = false, CanChange = false };
                CompleteCategory completeCategory4 = new CompleteCategory() { Category = category2, User = user3, IsChoose = false, IsStuded = false, CanChange = false };

                CompleteCategory completeCategory5 = new CompleteCategory() { Category = category1, User = user2, IsChoose = false, IsStuded = false, CanChange = false };
                CompleteCategory completeCategory6 = new CompleteCategory() { Category = category2, User = user2, IsChoose = false, IsStuded = false, CanChange = false };

                await DB.SaveComplCatAsync(completeCategory1);
                await DB.SaveComplCatAsync(completeCategory2);
                await DB.SaveComplCatAsync(completeCategory3);
                await DB.SaveComplCatAsync(completeCategory4);
                await DB.SaveComplCatAsync(completeCategory5);
                await DB.SaveComplCatAsync(completeCategory6);

                user1.CategoriesComlList = new List<CompleteCategory>() { completeCategory1, completeCategory2 };
                user2.CategoriesComlList = new List<CompleteCategory>() { completeCategory5, completeCategory6 };
                user3.CategoriesComlList = new List<CompleteCategory>() { completeCategory3, completeCategory4 };

                await DB.SaveUserAsync(user1);
                await DB.SaveUserAsync(user3);
                await DB.SaveUserAsync(user2);
            }
            else { }
        }
    }
}
