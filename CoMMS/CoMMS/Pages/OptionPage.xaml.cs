
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
            pkAlarm.Items.Add("적용");
            pkAlarm.Items.Add("미적용");

            ResourceManager rm = new ResourceManager("CoMMS.Resource1", Assembly.GetExecutingAssembly());

            if (!unexpected)
            {
                DisplayAlert("알림", "데이터베이스가 검색되지 않습니다.\n관리자에게 문의하세요.", "확인");
                // 세팅 불러오기
            }

            txtDB_IP.Text = Application.Current.Properties.ContainsKey("DB_IP") ? Application.Current.Properties["DB_IP"].ToString() : rm.GetString("DB_IP");
            txtDB_PORT.Text = Application.Current.Properties.ContainsKey("DB_PORT") ? Application.Current.Properties["DB_PORT"].ToString() : rm.GetString("DB_PORT");
            txtDBName.Text = Application.Current.Properties.ContainsKey("DB_NAME") ? Application.Current.Properties["DB_NAME"].ToString() : rm.GetString("DB_NAME");
            txtDB_ID.Text = Application.Current.Properties.ContainsKey("DB_ID") ? Application.Current.Properties["DB_ID"].ToString() : rm.GetString("DB_ID");
            txtDB_PW.Text = Application.Current.Properties.ContainsKey("DB_PW") ? Application.Current.Properties["DB_PW"].ToString() : rm.GetString("DB_PW");
            txtMstCode.Text = Application.Current.Properties.ContainsKey("Resorce_Mst_Code") ? Application.Current.Properties["Resorce_Mst_Code"].ToString() : rm.GetString("Resorce_Mst_Code");
            txtCondition.Text = Application.Current.Properties.ContainsKey("Condition") ? Application.Current.Properties["Condition"].ToString() : rm.GetString("Condition");
            string value = Application.Current.Properties.ContainsKey("Alarm") ? Application.Current.Properties["Alarm"].ToString() : rm.GetString("Alarm");

            for (int i = 0; i < pkAlarm.Items.Count; i++)
            {
                if (pkAlarm.Items[i] == value)
                {
                    pkAlarm.SelectedIndex = i;
                }
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
            Application.Current.Properties["Condition"] = txtCondition.Text == ""? "0": txtCondition.Text;
            Application.Current.Properties["Alarm"] = pkAlarm.SelectedItem.ToString();
            await Application.Current.SavePropertiesAsync();
            await DisplayAlert("완료", "저장이 완료되었습니다. 어플리케이션을 재실행해주세요.", "확인");
            if (Navigation.ModalStack.Count > 0)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                INativeHelper nativeHelper = null;
                nativeHelper = DependencyService.Get<INativeHelper>();
                if (nativeHelper != null)
                {
                    nativeHelper.CloseApp();
                }
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("알람", "백그라운드 작동 시 비가동 설비가 생성 시 알람이 진행됩니다.", "확인");
        }
    }
}
