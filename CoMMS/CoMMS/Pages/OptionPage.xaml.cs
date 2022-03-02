
using CoMMS.ViewModels;
using System;

using System.Reflection;
using System.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoMMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionPage : ContentPage
    {
        public OptionPage()
        { }
        public OptionPage(bool unexpected)
        {
            InitializeComponent();
            var vm = new OptionViewModel();
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var width = mainDisplayInfo.Width;
            vm.Width = width / 4;
            BindingContext = vm;
            if (!unexpected)
            {
                DisplayAlert("알림", "데이터베이스가 검색되지 않습니다.\n관리자에게 문의하세요.", "확인");
                // 세팅 불러오기
                ResourceManager rm = new ResourceManager("CoMMS.Resource1", Assembly.GetExecutingAssembly());

                //DB 세팅 불러오기
                txtDB_IP.Text = rm.GetString("DB_IP");
                txtDB_PORT.Text = rm.GetString("DB_PORT");
                txtDBName.Text = rm.GetString("DB_NAME");
                txtDB_ID.Text = rm.GetString("DB_ID");
                txtDB_PW.Text = rm.GetString("DB_PW");
                txtMstCode.Text = rm.GetString("Resorce_Mst_Code");
            }
            else
            {
                //DB 세팅 불러오기
                txtDB_IP.Text = Application.Current.Properties["DB_IP"].ToString();
                txtDB_PORT.Text = Application.Current.Properties["DB_PORT"].ToString();
                txtDBName.Text = Application.Current.Properties["DB_NAME"].ToString();
                txtDB_ID.Text = Application.Current.Properties["DB_ID"].ToString();
                txtDB_PW.Text = Application.Current.Properties["DB_PW"].ToString();
                txtMstCode.Text = Application.Current.Properties["Resorce_Mst_Code"].ToString();
            }



        }

        private async void SaveOption(object sender, EventArgs e)
        {
            btn_Save.IsEnabled = false;
            Application.Current.Properties["DB_IP"] = txtDB_IP.Text;
            Application.Current.Properties["DB_PORT"] = txtDB_PORT.Text;
            Application.Current.Properties["DB_NAME"] = txtDBName.Text;
            Application.Current.Properties["DB_ID"] = txtDB_ID.Text;
            Application.Current.Properties["DB_PW"] = txtDB_PW.Text;
            Application.Current.Properties["Resorce_Mst_Code"] = txtMstCode.Text;
            await Application.Current.SavePropertiesAsync();

            await DisplayAlert("완료", "저장이 완료되었습니다. 어플리케이션을 완전히 종료 후 재실행해주세요.", "확인");
            if (Navigation.ModalStack.Count > 0)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }

    }
}
