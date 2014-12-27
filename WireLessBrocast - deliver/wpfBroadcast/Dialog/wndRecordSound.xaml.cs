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

namespace wpfBroadcast.Dialog
{
    /// <summary>
    /// wndRecordSound.xaml 的互動邏輯
    /// </summary>
    public partial class wndRecordSound : Window
    {
        wpfBroadcast.BroadcastEntities broadcastEntities;
        public wndRecordSound()
        {
            InitializeComponent();
        }

        private System.Data.Objects.ObjectSet <tblRecordSound> GettblRecordSoundQuery(BroadcastEntities broadcastEntities)
        {
            System.Data.Objects.ObjectSet<wpfBroadcast.tblRecordSound> tblRecordSoundQuery = broadcastEntities.tblRecordSound;
            // 傳回 ObjectQuery。
            return tblRecordSoundQuery;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            broadcastEntities = new wpfBroadcast.BroadcastEntities();
            // 將資料載入 tblRecordSound。您可以依需要修改這個程式碼。
            System.Windows.Data.CollectionViewSource tblRecordSoundViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tblRecordSoundViewSource")));
            System.Data.Objects.ObjectSet<wpfBroadcast.tblRecordSound> tblRecordSoundQuery = this.GettblRecordSoundQuery(broadcastEntities);
            tblRecordSoundViewSource.Source = tblRecordSoundQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.broadcastEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "," + ex.StackTrace);

            }
            this.Close();

        }
    }
}
