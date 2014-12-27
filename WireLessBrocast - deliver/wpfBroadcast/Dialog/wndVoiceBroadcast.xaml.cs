using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WirelessBrocast;

namespace wpfBroadcast.Dialog
{
    /// <summary>
    /// wndVoiceBroadcast.xaml 的互動邏輯
    /// </summary>
    public partial class wndVoiceBroadcast : Window
    {
        wpfBroadcast.BroadcastEntities db = new BroadcastEntities();
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        bool IsEmergency;
        public wndVoiceBroadcast(bool IsEmergency)
        {
            InitializeComponent();
            this.IsEmergency = IsEmergency;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            var q = from n in db.tblSIte select new BroadcastBindingData() { SITE_ID = n.SITE_ID, SITE_NAME = n.SITE_NAME };
            // System.Collections.Generic.List<BroadcastBindingData> list = new List<BroadcastBindingData>();
            grdSite.ItemsSource = q.ToArray();

          //  var q1 = from n in db.tblRecordSound select n;
           

            tmr.Interval = TimeSpan.FromSeconds(3);
            tmr.Tick += tmr_Tick;
        //   tmr.Start();

            if (IsEmergency)
               Dispatcher.Invoke(()=> Button_Click(this.btnBegin, null));

        }

        bool InTmr = false;
        bool IsPause = false;
        void tmr_Tick(object sender, EventArgs e)
        {

            if (InTmr)
                return;
            InTmr = true;
            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                
                    byte status1,status2;
                    int cnt;
                    if (IsPause)
                        return;
                    try
                    {
                        lock (App.Kenwood)
                                       App.Kenwood.GetPlayStatus(site.SITE_ID, out status1, out status2, out cnt);
                        BitArray array = new BitArray(new byte[] { status1, status2 });
                        site.IsBusy = array.Get((int)StatusIndex.BUSY);
                    }
                    catch (Exception ex)
                    {
                    }

               
              //  site.IsSend = App.Kenwood.VoiceBroadcast(site.SITE_ID, true);

            }

             
            //throw new NotImplementedException();
        }
        bool StopVoiceFlag = false;
        bool InProcess = false;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            ClearSendFlag();
            (sender as Button).IsEnabled = false;
            StopVoiceFlag = false ;
            InProcess = true;

            await Task.Run(() =>
                {
                    lock (App.Kenwood)
                    {

                        if (!IsEmergency)
                        {
                            foreach (BroadcastBindingData site in grdSite.ItemsSource)
                            {


                                site.IsSend = App.Kenwood.VoiceBroadcast(site.SITE_ID, true);


                            }
                        }
                        else
                        App.Kenwood.VoiceBroadcast(0, true);
                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("您可以開始廣播");
                        });



                        while (!StopVoiceFlag)
                        {


                            System.Threading.Thread.Sleep(100);
                        }

                    }
                });

            (sender as Button).IsEnabled = true;
            if(IsEmergency)
                App.AddOperationlog("緊急廣播");
            else
            App.AddOperationlog("口語廣播");

        }
        void ClearSendFlag()
        {

            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                site.IsSend = false;
                 
            }
        }

        async void StopVoiceBroadcast()
        {
            StopVoiceFlag = true;

         await Task.Run(()=>
            {
                lock (App.Kenwood)
                {
                    foreach (BroadcastBindingData site in grdSite.ItemsSource)
                    {

                   
                            site.IsSend = App.Kenwood.VoiceBroadcast(site.SITE_ID, false);
                  
                    }
                }
                

            });


            
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            ClearSendFlag();
            (sender as Button).IsEnabled = false;
            StopVoiceBroadcast();

            (sender as Button).IsEnabled = true;
              InProcess = false;
         
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (InProcess)
            {
                MessageBox.Show("請先停止口語廣播");
                e.Cancel = true;
            }

            IsPause = true;
            tmr.Stop();
             
            
        }
    }
}
