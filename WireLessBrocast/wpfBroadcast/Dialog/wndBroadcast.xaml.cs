﻿using System;
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
    /// wndBroadcast.xaml 的互動邏輯
    /// </summary>
    /// 
    
    public partial class wndBroadcast : Window
    {
        wpfBroadcast.BroadcastEntities db = new BroadcastEntities();
        System.Threading.Thread thTask;
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        public wndBroadcast()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var q = from n in db.tblSIte select new BroadcastBindingData() { SITE_ID=n.SITE_ID,SITE_NAME=n.SITE_NAME };
           // System.Collections.Generic.List<BroadcastBindingData> list = new List<BroadcastBindingData>();
            grdSite.ItemsSource = q.ToArray();

            var q1 = from n in db.tblRecordSound select n;

            cbRecordSound.ItemsSource = q1.ToArray();
            cbRecordSound.SelectedIndex = 0;
            tmr.Interval = TimeSpan.FromSeconds(5);
            tmr.Tick += tmr_Tick;
            tmr.Start();
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
                        BitArray array = new BitArray(new byte[] { status1, status2 });
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


        void CleadSendFlag()
        {
            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                site.IsSend = false;
                System.Windows.Forms.Application.DoEvents();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (BroadcastBindingData data in grdSite.ItemsSource)
                data.IsSelected = true;
        }

        private async  void Button_Click_1(object sender, RoutedEventArgs e)
        {

            int Playcnt;
         //   grdSite.CommitEdit( );
            if (!int.TryParse(txtCount.Text, out Playcnt))
            {
                MessageBox.Show("播放次數格式錯誤!");
                return;
            }

        //    grdSite.SelectedIndex = -1;
          //  Playcnt=System.Convert.ToInt32(txtCount.Text);
            (sender as Button).IsEnabled = false;
              await  PlayTask(Playcnt);
             
            (sender as Button).IsEnabled = true;
        }

        bool StopTask;
        bool InPlayTask;
        async  Task PlayTask( int  playcnt)
        {
          // int playcnt=System.Convert.ToInt32(args )
            if (InPlayTask)
                return;

            InPlayTask = true;
                  
                    foreach (BroadcastBindingData data in grdSite.ItemsSource)
                    {

                        if (StopTask)
                            return;
                        if (!data.IsSelected)
                            continue;
                        data.IsSend = false;
                        data.RepeatCnt = 0;
                        lock(App.Kenwood)
                        data.IsSend = App.Kenwood.Play(data.SITE_ID, (cbRecordSound.SelectedItem as tblRecordSound).PlayIndex - 1,
                         playcnt);



                    }
                //    bool finish = true;
                //    do
                //    {

                //        finish = true;
                //        foreach (BroadcastBindingData data in grdSite.ItemsSource)
                //        {
                //            byte status1, status2;
                //            int cnt;

                //            if (StopTask)
                //                return;
                //            if (!data.IsSelected)
                //                continue;
                //            lock (App.Kenwood)
                //            App.Kenwood.GetPlayStatus(data.SITE_ID, out status1, out status2, out cnt);
                           

                //            BitArray array = new BitArray(new byte[] { status1, status2 });
                //            data.IsBusy = array.Get((int)StatusIndex.BUSY);
                             
                                
                //            data.RepeatCnt = cnt;
                //            finish &= (!data.IsBusy || !data.IsSend);
                //            System.Windows.Forms.Application.DoEvents();
                //        }
                //    } while (!finish);

                       
                        
               
                //MessageBox.Show("finished");
                InPlayTask = false;
        }

        private void grdSite_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //DataGrid grid = (DataGrid)sender;
            //grid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((sender as CheckBox).DataContext as BroadcastBindingData).IsSelected = (sender as CheckBox).IsChecked ?? false; ;
        }

        private void Monitor_Click(object sender, RoutedEventArgs e)
        {
             BroadcastBindingData data = (sender as Button).DataContext as BroadcastBindingData;

             lock (App.Kenwood)
             {
                 data.CanEcho = false;
                 System.Windows.Forms.Application.DoEvents();
              bool success =  App.Kenwood.Echo(data.SITE_ID);
              data.CanEcho = true;
              System.Windows.Forms.Application.DoEvents();
              System.Threading.Thread.Sleep(10 * 1000);
             }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("確定要離開", "Brocast", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            tmr.Stop();
            if (InPlayTask)
            {

                foreach (BroadcastBindingData data in grdSite.ItemsSource)
                {

                    if (data.IsSelected)
                    {
                        lock(App.Kenwood)
                        App.Kenwood.Abort(data.SITE_ID);
                    }
                }
            }
            this.StopTask = true;
        }
       
    }
}