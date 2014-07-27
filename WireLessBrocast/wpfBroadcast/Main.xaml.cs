using System;
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
using wpfBroadcast.Dialog;

namespace wpfBroadcast
{
    /// <summary>
    /// Main.xaml 的互動邏輯
    /// </summary>
    public partial class Main : Window
    {
        wpfBroadcast.BroadcastEntities db;
        public Main()
        {
            InitializeComponent();

          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.accdMenu.SelectAll();

          db  = new BroadcastEntities();
            tblSysLog log = new tblSysLog() { Message = "系統登入", SITE_ID = 0, Type = "S", StartTimeStamp = DateTime.Now ,UserID=App.loginUser.UserID};

            db.tblSysLog.AddObject(log);
            db.SaveChanges();
         //   wpfBroadcast.BroadcastEntities broadcastEntities = new wpfBroadcast.BroadcastEntities();
           
            // 將資料載入 tblSIte。您可以依需要修改這個程式碼。
            System.Windows.Data.CollectionViewSource tblSIteViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tblSIteViewSource")));
            System.Data.Objects.ObjectQuery<wpfBroadcast.tblSIte> tblSIteQuery = this.GettblSIteQuery(App.db);
            tblSIteViewSource.Source = tblSIteQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            wndUserSetting wnd = new Dialog.wndUserSetting();
            wnd.Title = (sender as Button).Content.ToString();
            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.Owner = this;
            wnd.ShowDialog();
            
        }

        

        private void btnSite_Click(object sender, RoutedEventArgs e)
        {
            wndSite wnd = new Dialog.wndSite();
            wnd.Title = (sender as Button).Content.ToString();
            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.Owner = this;
            wnd.ShowDialog();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
        //    wpfBroadcast.BroadcastEntities db = new BroadcastEntities();
            tblSysLog log = new tblSysLog() { Message = "系統登出", SITE_ID = 0, Type = "S", StartTimeStamp = DateTime.Now, UserID = (App.loginUser.UserID)  };

            db.tblSysLog.AddObject(log);
            db.SaveChanges();
        }

        private void btnRecordSound_Click(object sender, RoutedEventArgs e)
        {
            wndRecordSound wnd = new Dialog.wndRecordSound();
            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;
            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            wndSchedule wnd = new Dialog.wndSchedule();
            wnd.Title = (sender as Button).Content.ToString();
            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.Owner = this;
            wnd.ShowDialog();

         
        
        }

        private System.Data.Objects.ObjectQuery<tblSIte> GettblSIteQuery(BroadcastEntities broadcastEntities)
        {
            System.Data.Objects.ObjectQuery<wpfBroadcast.tblSIte> tblSIteQuery = broadcastEntities.tblSIte;
            // 傳回 ObjectQuery。
            return tblSIteQuery;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
           // System.Windows.Data.CollectionViewSource tblSIteViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tblSIteViewSource")));
            lock (App.db)
            {
                tblSIte site = App.db.tblSIte.Where(n => n.SITE_ID == 1).FirstOrDefault();
                if (site == null)
                    return;
                site.AC = !site.AC;
                site.DC = !site.DC;
               
                site.DoorOpen = !site.DoorOpen;
                 
                
                site.Amp = !site.Amp;
                site.Comm = !site.Comm;
                site.Speaker = !site.Speaker;
               
                App.db.SaveChanges();
                if (site.DoorOpen)
                {
                    // VoicePlay(site.SITE_NAME + "箱門開啟");
                    App.VoicePlayAsync(site.SITE_NAME + "箱門開啟");
                }
            }
            
        }

        private void btpReportLogin(object sender, RoutedEventArgs e)
        {
            Dialog.wndLoginReport wnd = new Dialog.wndLoginReport("S");
            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;

           
            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
        }

        private void btnReportEvent(object sender, RoutedEventArgs e)
        {
            wndLoginReport wnd = new Dialog.wndLoginReport("A");
           
            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;


            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
        }

        private void RecordBrocast_Click(object sender, RoutedEventArgs e)
        {
            wndBroadcast wnd = new Dialog.wndBroadcast(false);

            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;


            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
        }

        private void VoiceBrocast_Click(object sender, RoutedEventArgs e)
        {
            wndVoiceBroadcast wnd = new Dialog.wndVoiceBroadcast();

            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;


            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            wndTest wnd = new Dialog.wndTest(false);

            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;


            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
        }

        private void SilentTest_Click(object sender, RoutedEventArgs e)
        {
            wndTest wnd = new Dialog.wndTest(true);

            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;


            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
        }

        private void btnSchTestLog(object sender, RoutedEventArgs e)
        {
            wndTestLogReport wnd = new Dialog.wndTestLogReport();

            wnd.Title = (sender as Button).Content.ToString();
            wnd.Owner = this;


            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
        }
    }
}
