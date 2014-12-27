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
    /// wndTest.xaml 的互動邏輯
    /// </summary>
    public partial class wndTest : Window
    {
        wpfBroadcast.BroadcastEntities db = new BroadcastEntities();
        bool IsSilent;
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        bool IsPause = false;
        public wndTest(bool IsSilent)
        {

            InitializeComponent();
            this.IsSilent = IsSilent;

            if (IsSilent)
                txtTitle.Text = "靜音測試";
            else
                txtTitle.Text = "一般測試";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
         

            var q = from n in db.tblSIte select new BroadcastBindingData() { SITE_ID = n.SITE_ID, SITE_NAME = n.SITE_NAME };
            // System.Collections.Generic.List<BroadcastBindingData> list = new List<BroadcastBindingData>();
            grdSite.ItemsSource = q.ToArray();

            var q1 = from n in db.tblRecordSound select n;
            tmr.Interval = TimeSpan.FromSeconds(5);
            tmr.Tick += tmr_Tick;
          
            //cbRecordSound.ItemsSource = q1.ToArray();
            //cbRecordSound.SelectedIndex = 0;
        }

        bool InTmr = false;
        void tmr_Tick(object sender, EventArgs e)
        {
            if (IsPause)
                return;
            if (InTmr)
                return;

            InTmr = true;
            try
            {

                Task.Run(() =>
                    {
                        foreach (BroadcastBindingData site in grdSite.ItemsSource)
                        {
                            if (IsPause)
                            {
                                return;
                            }
                            if (!site.IsSelected)
                                continue;
                            lock (App.Kenwood)
                            {
                                byte status1, status2;
                                int cnt;
                                App.Kenwood.GetPlayStatus(site.SITE_ID, out status1, out status2, out cnt);
                                BitArray array = new BitArray(new byte[] { status1, status2 });
                                site.IsBusy = array.Get((int)StatusIndex.BUSY);
                                site.RepeatCnt = cnt;

                            }
                        }
                    });
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
                site.RepeatCnt = 0;
            }
        }

        //void CleadSendFlag()
        //{
        //    foreach (BroadcastBindingData site in grdSite.ItemsSource)
        //    {
        //        site.IsSend = false;
        //        System.Windows.Forms.Application.DoEvents();
        //    }
        //}
        bool IsStopTask = false;
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            int cnt;
            if (!int.TryParse(txtRepeat.Text, out cnt))
                return;
            CleadSendFlag();
            (sender as Button).IsEnabled = false;
            IsStopTask = false;
             await Task.Run(() =>
                {
                    lock (App.Kenwood)
                    {
            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                System.Windows.Forms.Application.DoEvents();
                if (IsStopTask)
                    return;
                if (!site.IsSelected)
                    continue;


               
                  
                
                site.IsSend = App.Kenwood.Test(site.SITE_ID, IsSilent,IsSilent? 2:cnt);

               
                

            }
                }

                });
            (sender as Button).IsEnabled = true;
            App.AddOperationlog(IsSilent?"靜音測試":"語音測試");
            tmr.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsPause = true;
            IsStopTask = true;
            tmr.Stop();
            
          
        }

        private  async  void Button_Click_2(object sender, RoutedEventArgs e)
        {
            IsPause = true;
            IsStopTask = true;
            (sender as Button).IsEnabled = false;
            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                if (!site.IsSelected)
                    continue;

                await Task.Run(() =>
                {
                    lock (App.Kenwood)

                        site.IsSend = App.Kenwood.Abort(site.SITE_ID);
                });
                

            }

            wndVoiceBroadcast wnd = new Dialog.wndVoiceBroadcast(true);

            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;


            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
            (sender as Button).IsEnabled = true;
            IsPause = false;
            tmr.Stop();
           
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((sender as CheckBox).DataContext as BroadcastBindingData).IsSelected = (sender as CheckBox).IsChecked ?? false; ;
        }
    }
}
