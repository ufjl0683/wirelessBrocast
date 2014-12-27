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
    /// wndBroadcast.xaml 的互動邏輯
    /// </summary>
    /// 
    
    public partial class wndBroadcast : Window
    {
        wpfBroadcast.BroadcastEntities db = new BroadcastEntities();
        System.Threading.Thread thTask;
        bool IsEmergency;
        bool IsPause=false;
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        public wndBroadcast(bool IsEmergency)
        {
            InitializeComponent();
            this.IsEmergency = IsEmergency;
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private  void Window_Loaded(object sender, RoutedEventArgs e)
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
            if(IsEmergency)
              Button_Click(this.btnEmergency, null);
        }

        bool InTmr = false;
        async void tmr_Tick(object sender, EventArgs e)
        {
            if (IsPause)
            {
                tmr.Stop();
                tmr.IsEnabled = false;
                return;
            }
            if (InTmr)
                return;

            InTmr = true;
            try
            {
              await  Task.Run(()=>
                {
                foreach (BroadcastBindingData site in grdSite.ItemsSource)
                {
                    if (IsPause)
                    {
                        InTmr = false;
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


        void CleadSendFlag()
        {
            foreach (BroadcastBindingData site in grdSite.ItemsSource)
            {
                site.IsSend = false;
                site.RepeatCnt = 0;
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
            tmr.Stop();
            tmr.IsEnabled = true;
            (sender as Button).IsEnabled = false;
            int pindex=(this.cbRecordSound.SelectedItem as tblRecordSound).PlayIndex;
            StopTask = false;
            await Task.Run(() => PlayTask(Playcnt,pindex) );
             
             
            (sender as Button).IsEnabled = true;
            if(IsEmergency)
                App.AddOperationlog("緊急廣播");
            else
            {
                App.AddOperationlog((cbRecordSound.SelectedItem as tblRecordSound).Name + "X" + txtCount.Text);
                 foreach (BroadcastBindingData data in grdSite.ItemsSource)
                 {

                     if (data.IsSelected)
                         App.AddOperationlog(data.SITE_ID, (cbRecordSound.SelectedItem as tblRecordSound).Name + "X" + txtCount.Text);
                 }
             }
            tmr.Start();
        }

        bool StopTask;
        bool InPlayTask;
        async  Task PlayTask( int  playcnt,int PlayIndex)
        {
          // int playcnt=System.Convert.ToInt32(args )
            //if (App.Kenwood == null)
            //    App.Kenwood = new KenWood(0, App.ComPort, true);
            try
            {
                if (InPlayTask)
                    return;

                InPlayTask = true;

                foreach (BroadcastBindingData data in grdSite.ItemsSource)
                {
                    if (IsPause)
                        return;

                    if (StopTask)
                        return;
                    System.Windows.Forms.Application.DoEvents();
                    if (!data.IsSelected)
                        continue;
                    data.IsSend = false;
                    data.RepeatCnt = 0;
                    lock (App.Kenwood)
                    {

                        data.IsSend =
                        App.Kenwood.Play(data.SITE_ID, PlayIndex - 1,
                          playcnt);


                    }



                }
                bool finish = true;
                do
                {

                    finish = true;
                    foreach (BroadcastBindingData data in grdSite.ItemsSource)
                    {
                        byte status1, status2;
                        int cnt;
                        if (IsPause)
                        {
                            return;
                        }
                        if (StopTask)
                        {
                            return;
                        }
                        if (!data.IsSelected)
                            continue;
                        lock (App.Kenwood)
                            App.Kenwood.GetPlayStatus(data.SITE_ID, out status1, out status2, out cnt);


                        BitArray array = new BitArray(new byte[] { status1, status2 });
                        data.IsBusy = array.Get((int)StatusIndex.BUSY);


                        data.RepeatCnt = cnt;
                        finish &= (!data.IsBusy || !data.IsSend);
                        System.Windows.Forms.Application.DoEvents();
                    }
                } while (!finish);



            }
            catch
            {
                ;
            }
            finally
            {

                //MessageBox.Show("finished");
                InPlayTask = false;
            }
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

        private async void Monitor_Click(object sender, RoutedEventArgs e)
        {
             BroadcastBindingData data = (sender as Button).DataContext as BroadcastBindingData;
             data.CanEcho = false;
             await Task.Run(async () =>
             {
                 tmr.Stop();
                     lock (App.Kenwood)
                     {

                      
                         bool success = App.Kenwood.Echo(data.SITE_ID);

                           
                        System.Threading.Thread.Sleep(10 * 1000);
                      
                          
                    }
                    
                   //  await Task.Delay(1000 * 10);
                     tmr.Start();
                 });
             data.CanEcho = true;
           
        }

        bool isForceLeave = false;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isForceLeave &&   MessageBox.Show("確定要離開", "Brocast", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel  )
            {
                e.Cancel = true;
                return;
            }

            busyIndicator.Content ="廣播任務結束中!";
            busyIndicator.IsBusy = true;
            this.StopTask = true;
            IsPause = true;
            tmr.Stop();
            tmr.IsEnabled = false;
            
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
        
            busyIndicator.IsBusy = false;
        }

        private  void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StopTask = true;
            IsPause = true;
            (sender as Button).IsEnabled = false;
            lock (App.Kenwood)
                App.Kenwood.Abort(0);// all brocast

            //foreach (BroadcastBindingData site in grdSite.ItemsSource)
            //{
            //    if (!site.IsSelected)
            //        continue;

            //    await Task.Run(() =>
            //    {
            //        lock (App.Kenwood)

            //            site.IsSend = App.Kenwood.Abort(site.SITE_ID);
            //    });


            //}

            wndVoiceBroadcast wnd = new Dialog.wndVoiceBroadcast(true);

            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;


            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            
            wnd.ShowDialog();
            (sender as Button).IsEnabled = true;
            IsPause = false;
            isForceLeave = true;
            this.StopTask = true;
            IsPause = true;
            tmr.Stop();
           
            this.Close();
            
        }
       
    }
}
