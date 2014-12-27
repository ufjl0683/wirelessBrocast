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
    /// wndSchedule.xaml 的互動邏輯
    /// </summary>
    public partial class wndSchedule : Window
    {
        wpfBroadcast.BroadcastEntities broadcastEntities;
        public wndSchedule()
        {
            InitializeComponent();
        }

        //private System.Data.Entity.Infrastructure.Db<tblRecordSound> GettblRecordSoundQuery(BroadcastEntities broadcastEntities)
        //{
        //    System.Data.Objects.ObjectSet<wpfBroadcast.tblRecordSound> tblRecordSoundQuery = broadcastEntities.tblRecordSound;
        //    // 更新查詢以將 tblSchedule 資料包含在 tblRecordSound 中。您可以依需要修改這個程式碼。
        //    tblRecordSoundQuery = tblRecordSoundQuery.Include("tblSchedule");
        //    // 傳回 ObjectQuery。
        //    return tblRecordSoundQuery;
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            broadcastEntities = new wpfBroadcast.BroadcastEntities();
            // 將資料載入 tblRecordSound。您可以依需要修改這個程式碼。
            //System.Windows.Data.CollectionViewSource tblRecordSoundViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tblRecordSoundViewSource")));
            //System.Data.Objects.ObjectSet<wpfBroadcast.tblRecordSound> tblRecordSoundQuery = this.GettblRecordSoundQuery(broadcastEntities);
            //tblRecordSoundViewSource.Source = tblRecordSoundQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
            // 將資料載入 tblSchedule。您可以依需要修改這個程式碼。
            System.Windows.Data.CollectionViewSource tblScheduleViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tblScheduleViewSource")));
            System.Data.Objects.ObjectSet<wpfBroadcast.tblSchedule> tblScheduleQuery = this.GettblScheduleQuery(broadcastEntities);
            tblScheduleViewSource.Source = tblScheduleQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private System.Data.Objects.ObjectSet<tblSchedule> GettblScheduleQuery(BroadcastEntities broadcastEntities)
        {
            System.Data.Objects.ObjectSet<wpfBroadcast.tblSchedule> tblScheduleQuery = broadcastEntities.tblSchedule;
            //// 若要明確載入資料，您必須加入類似下面的 Include 方法:
            //// tblScheduleQuery = tblScheduleQuery.Include("tblSchedule.tblRecordSound").
            //// 如需詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=157380
            //// 傳回 ObjectQuery。
            return tblScheduleQuery;
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
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOk_Copy_Click(object sender, RoutedEventArgs e)
        {
            BroadcastDataSet ds = new BroadcastDataSet();
            BroadcastDataSetTableAdapters.tblRecordSoundTableAdapter adpRecordSound = new BroadcastDataSetTableAdapters.tblRecordSoundTableAdapter();
            adpRecordSound.Fill(ds.tblRecordSound);
            BroadcastDataSetTableAdapters.tblScheduleTableAdapter adpsch = new BroadcastDataSetTableAdapters.tblScheduleTableAdapter();
            
            adpsch.Fill(ds.tblSchedule);

            this.broadcastEntities.SaveChanges();
            
            new wndReportViewer(ds,new report.rptSchedule()).ShowDialog();
        }
    }
}
