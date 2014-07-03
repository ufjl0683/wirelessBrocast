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

namespace wpfBroadcast
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class wndLogin : Window
    {
        public wndLogin()
        {
            InitializeComponent();
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
            (App.Current as App).loginUser = usr;
            this.Hide();
            new Main().ShowDialog();
            this.Show();
        }
    }
}
