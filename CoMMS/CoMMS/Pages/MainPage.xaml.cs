using CoMMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoMMS
{
    public partial class MainPage : ContentPage
    {
        private bool IsRunning = true;
        private ObservableCollection<Equip> _scanned { get; set; }
        public MainPage()
        {
            InitializeComponent();
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Equiplist.IsEnabled = true;
            tbOption.IsEnabled = true;

            IsRunning = true;

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                if (IsRunning)
                {
                    indicator.IsRunning = true;
                    _scanned = new ObservableCollection<Equip>();
                    Provider prov = new Provider();
                    DataTable dt = prov.EquipList_R10();
                    if (dt != null)
                    {
                        int cnt = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            bool status = true;
                            string statusString = "가동";
                            string equipColor = "#3c9ee7";
                            if (dt.Rows[i]["collection_value"].ToString() == "0")
                            {
                                status = false;
                                statusString = "비가동";
                                equipColor = "#f16257";
                                cnt++;
                            }

                            _scanned.Add(new Equip()
                            {
                                EquipCode = dt.Rows[i]["resource_code"].ToString(),
                                EquipName = CodeService.SensorNameReturn(dt.Rows[i]["resource_code"].ToString()),
                                Status = status,
                                EquipColor = equipColor,
                                StatusString = statusString,
                                Value = dt.Rows[i]["collection_value"].ToString(),
                            });
                        }
                        lblCount.Text = cnt.ToString();
                    }
                    // Do something
                    Equiplist.ItemsSource = _scanned;
                    indicator.IsRunning = false;
                    return true; // True = Repeat again, False = Stop the timer
                }
                else
                {
                    return false;
                }

            });
        }
        protected override void OnDisappearing()
        {
            IsRunning = false;
            base.OnDisappearing();
        }
        #region 버튼 메소드
        private async void tbOption_Clicked(object sender, EventArgs e)
        {
            tbOption.IsEnabled = false;
            await Navigation.PushAsync(new OptionPage(true));
        }
        #endregion


    }
}
