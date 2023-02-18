using praktApp;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ЛР7_ВПКС.models;

namespace ЛР7_ВПКС.Data
{
    public class ElectronicBookDB
    {
        private static ElectronicBookDB _context;

        public static ElectronicBookDB GetContext()
        {
            if (_context == null)
            {
                _context = new ElectronicBookDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ElectronicBookDB.db3"));
            }
            return _context;
        }

        readonly SQLiteAsyncConnection db;

        public ElectronicBookDB(string connectionString)
        {
            db = new SQLiteAsyncConnection(connectionString);

            db.CreateTableAsync<Category>().Wait();
            db.CreateTableAsync<Class>().Wait();
            db.CreateTableAsync<CompleteCategory>().Wait();
            db.CreateTableAsync<Role>().Wait();
            db.CreateTableAsync<User>().Wait();
            db.CreateTableAsync<Word>().Wait();
        }

        // Методы для категорий

        public Task<List<Category>> GetCategoriesAsync()
        {
            return db.GetAllWithChildrenAsync<Category>();
        }

        public Category GetCategory(int Id)
        {
            return GetCategoriesAsync().Result.FirstOrDefault(p => p.Id == Id);
        }

        public Task SaveCategoryAsync(Category category)
        {
            if (category.Id == 0)
                return db.InsertWithChildrenAsync(category);
            return db.UpdateWithChildrenAsync(category);
        }

        public Task<int> DeleteCategoryAsync(Category category)
        {
            db.DeleteAllAsync(category.Words).Wait();

            foreach (CompleteCategory completeCategory in category.CompleteCatList)
            {
                DeleteComplCatAsync(completeCategory).Wait();
                Global.CurrentUser.CategoriesComlList.Remove(completeCategory);
            }
            SaveUserAsync(Global.CurrentUser);

            return db.DeleteAsync(category);
        }

        // Методы для классов

        public Task<List<Class>> GetClassesAsync()
        {
            return db.GetAllWithChildrenAsync<Class>();
        }

        public Class GetClass(int Id)
        {
            return  GetClassesAsync().Result.FirstOrDefault(p => p.Id == Id);
        }

        public Task SaveClassAsync(Class classs)
        {
            if (classs.Id == 0)
                return db.InsertWithChildrenAsync(classs);
            return db.UpdateWithChildrenAsync(classs);
        }

        public Task<int> DeleteClassAsync(Class classs)
        {
            return db.DeleteAsync(classs);
        }

        // Методы для ролей

        public Task<List<Role>> GetRolesAsync()
        {
            return db.GetAllWithChildrenAsync<Role>();
        }

        public Role GetRole(int Id)
        {
            return GetRolesAsync().Result.FirstOrDefault(p => p.Id == Id);
        }

        public Task SaveRoleAsync(Role role)
        {
            if (role.Id == 0)
                return db.InsertWithChildrenAsync(role);
            return db.UpdateWithChildrenAsync(role);
        }

        public Task<int> DeleteRoleAsync(Class role)
        {
            return db.DeleteAsync(role);
        }

        // Методы для пользователей

        public Task<List<User>> GetUsersAsync()
        {
            return db.GetAllWithChildrenAsync<User>();
        }

        public User GetUser(int Id)
        {
            return GetUsersAsync().Result.FirstOrDefault(p => p.Id == Id);
        }

        public Task SaveUserAsync(User user)
        {
            if (user.Id == 0)
                return db.InsertWithChildrenAsync(user);
            return db.UpdateWithChildrenAsync(user);
        }

        public Task<int> DeleteUserAsync(User user)
        {
            return db.DeleteAsync(user);
        }

        // Методы для слов

        public Task<List<Word>> GetWordsAsync()
        {
            return db.GetAllWithChildrenAsync<Word>();
        }

        public Word GetWord(int Id)
        {
            return GetWordsAsync().Result.FirstOrDefault(p => p.Id == Id);
        }

        public Task SaveWordAsync(Word word)
        {
            if (word.Id == 0)
                return db.InsertWithChildrenAsync(word);
            return db.UpdateWithChildrenAsync(word);
        }

        public Task<int> DeleteWordAsync(Word word)
        {
            return db.DeleteAsync(word);
        }

        // Методы для выбранных категорий

        public Task<List<CompleteCategory>> GetComplCatAsync()
        {
            return db.GetAllWithChildrenAsync<CompleteCategory>();
        }

        public CompleteCategory GetComplCatAsync(int Id)
        {
            return GetComplCatAsync().Result.FirstOrDefault(p => p.Id == Id);
        }

        public Task SaveComplCatAsync(CompleteCategory completeCategory)
        {
            if (completeCategory.Id == 0)
                return db.InsertWithChildrenAsync(completeCategory);
            return db.UpdateWithChildrenAsync(completeCategory);
        }

        public Task<int> DeleteComplCatAsync(CompleteCategory completeCategory)
        {
            return db.DeleteAsync(completeCategory);
        }
    }
}
