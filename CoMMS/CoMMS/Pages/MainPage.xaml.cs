using CoMMS.Models;
using CoMMS.Service;
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
        INotificationManager notificationManager;
        private bool IsRunning = true;
        int previousCnt = 0;
        private ObservableCollection<Equip> _scanned { get; set; }
        public MainPage()
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };

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
                    int cnt = 0;

                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            bool status = true;
                            string statusString = "가동";
                            string equipColor = "#3c9ee7";
                            double condition = 0;
                            if (Application.Current.Properties.ContainsKey("Condition"))
                                condition = Convert.ToDouble(Application.Current.Properties["Condition"].ToString());
                            double value = dt.Rows[i]["collection_value"].ToString() == "" ? Convert.ToDouble(0) : Convert.ToDouble(dt.Rows[i]["collection_value"].ToString());

                            if (value <= condition)
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
                                Value = value.ToString(),
                            });
                        }
                        lblCount.Text = cnt.ToString();
                    }
                    if (cnt > 0)
                    {
                        if (previousCnt != cnt)
                        {
                            string title = $"비가동 설비 발생";
                            string message = $"{cnt}대의 설비가 비가동 상태가 발생했습니다.";
                            notificationManager.SendNotification(title, message);
                            previousCnt = cnt;
                        }
                    }
                  
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
            if (Application.Current.Properties.ContainsKey("Alarm").ToString() == "미적용")
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

        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
            });
        }
    }
}
