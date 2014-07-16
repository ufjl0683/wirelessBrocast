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
    /// wndTest.xaml 的互動邏輯
    /// </summary>
    public partial class wndTest : Window
    {
        wpfBroadcast.BroadcastEntities db = new BroadcastEntities();
        bool IsSilent;
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        public wndTest(bool IsSilent)
        {

            InitializeComponent();
            this.IsSilent = IsSilent;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
         

            var q = from n in db.tblSIte select new BroadcastBindingData() { SITE_ID = n.SITE_ID, SITE_NAME = n.SITE_NAME };
            // System.Collections.Generic.List<BroadcastBindingData> list = new List<BroadcastBindingData>();
            grdSite.ItemsSource = q.ToArray();

            var q1 = from n in db.tblRecordSound select n;
            tmr.Interval = TimeSpan.FromSeconds(5);
            tmr.Tick += tmr_Tick;
            tmr.Start();
            //cbRecordSound.ItemsSource = q1.ToArray();
            //cbRecordSound.SelectedIndex = 0;
        }

        bool InTmr = false;
        void tmr_Tick(object sender, EventArgs e)
        {
            if (InTmr)
                return;

            InTmr = true;
            try
            {
                foreach (BroadcastBindingData site in grdSite.ItemsSource)
                {
                    if (!site.IsSelected)
                        continue;
                    lock (App.Kenwood)
                    {
                        byte status1, status2;
                        int cnt;
                        App.Kenwood.GetPlayStatus(site.SITE_ID, out status1, out status2, out cnt);
                        BitArray array = new BitArray(new byte[]{status1,status2});
                        site.IsBusy = array.Get((int)StatusIndex.BUSY);
                        site.RepeatCnt = cnt;

                    }
                }
            }
            catch
            {
            }
            finally
            {
                InTmr = false;
            }
            //throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (BroadcastBindingData data in grdSite.ItemsSource)
                data.IsSelected = true;
        }

        void CleadSendFlag()
        {
            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                site.IsSend = false;
                System.Windows.Forms.Application.DoEvents();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            int cnt;
            if (!int.TryParse(txtRepeat.Text, out cnt))
                return;
            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                if (!site.IsSelected)
                    continue;

                lock (App.Kenwood)
               
                    site.IsSend = App.Kenwood.Test(site.SITE_ID, IsSilent,cnt);
                System.Windows.Forms.Application.DoEvents();

            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tmr.Stop();
          
        }
    }
}
