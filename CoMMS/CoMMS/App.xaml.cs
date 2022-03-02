using System;
using System.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoMMS
{
    public partial class App : Application
    {
        public static bool IsUnexpected = true;

        public App()
        {
            InitializeComponent();
            try
            {
                string isFirst = Application.Current.Properties["DB_IP"].ToString();
                Provider prov = new Provider();
                DataSet ds = prov.LoadCode();
                CodeService.SensorCodeTable = ds.Tables[0];
            }
            catch (Exception)
            {
                IsUnexpected = false;
            }
        }

        protected override void OnStart()
        {
            if (IsUnexpected)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new OptionPage(false));
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
