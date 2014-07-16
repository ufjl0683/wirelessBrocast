using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public wndVoiceBroadcast()
        {
            InitializeComponent();
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

        }

        void tmr_Tick(object sender, EventArgs e)
        {

            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                
                    byte status1,status2;
                    int cnt;
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            ClearSendFlag();
            (sender as Button).IsEnabled = false;
            StopVoiceFlag = false ;
            InProcess = true;
            lock (App.Kenwood)
            {
                foreach (BroadcastBindingData site in grdSite.ItemsSource)
                {


                    site.IsSend = App.Kenwood.VoiceBroadcast(site.SITE_ID, true);

                    System.Windows.Forms.Application.DoEvents();
                }

                MessageBox.Show("您可以開始廣播");
                while (!StopVoiceFlag)
                {

                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
                
            }

            (sender as Button).IsEnabled = true;
            
          

        }
        void ClearSendFlag()
        {

            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                site.IsSend = false;
                System.Windows.Forms.Application.DoEvents();
            }
        }

        void StopVoiceBroadcast()
        {
            StopVoiceFlag = true;

            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {

                lock (App.Kenwood)
                {
                    site.IsSend = App.Kenwood.VoiceBroadcast(site.SITE_ID, false);
                }
                System.Windows.Forms.Application.DoEvents();
            }


            
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
        }
    }
}
