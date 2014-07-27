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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WirelessBrocast;

namespace wpfBroadcast
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class wndLogin : Window
    {
     //   public wpfBroadcast.BroadcastEntities db = new BroadcastEntities();
      //  System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
         
        public wndLogin()
        {
            InitializeComponent();
          //  App.Kenwood = new KenWood(0, App.ComPort, true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            wpfBroadcast.BroadcastEntities db = new BroadcastEntities();

            tblUser usr = db.tblUser.Where(n => n.UserID == txtAccount.Text.Trim() && n.Password == txtPwd.Password.Trim()).FirstOrDefault();
#if !DEBUG
            if (usr == null)
            {
                MessageBox.Show("帳號或密碼錯誤!");
                return;
            }
#endif
            txtAccount.Text = txtPwd.Password = "";
           // App.Current.Properties["loginUser"] = usr;
            if (usr == null)
                usr = new tblUser() { UserID = "", GroupID = 1, Password = "", UserName = "" };
           App.loginUser = usr;
            this.Hide();
            new Main().ShowDialog();
            this.Show();
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
            
        }

        void tmr_Tick(object sender, EventArgs e)
        {
           
                 
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(App.tmr!=null)
            App.tmr.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            Environment.Exit(0);
        }

             
        }
    }

