using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using praktApp;
using praktApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ЛР7_ВПКС.Data;
using ЛР7_ВПКС.models;

namespace testAnd
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            if(Global.CurrentUser.RoleId == 2)
            {
                TabGlossary.IsVisible = false;
                MenuTab.IsVisible = false;
            }
            else
            {
                ClassTab.IsVisible = false;
                CategoryTab.IsVisible = false;
            }
        }
    }
}