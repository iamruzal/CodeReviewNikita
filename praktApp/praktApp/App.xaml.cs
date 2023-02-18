using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using testAnd;
using Xamarin.Forms;
using Xamarin.Essentials;
using praktApp.Views;
using ЛР7_ВПКС.models;
using praktApp.Data;
using Newtonsoft.Json;
using ЛР7_ВПКС.Data;

namespace praktApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        protected override void OnStart()
        {
            InitializateDatabase.InitializateDB();

            if (Global.IsFileWithUserExist())
            {
                Global.DeserealizateUser();
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new AutorizationPage());
            }
        }
    }
}
