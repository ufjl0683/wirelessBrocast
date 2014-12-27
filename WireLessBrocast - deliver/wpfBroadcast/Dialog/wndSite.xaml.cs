using System;
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

namespace wpfBroadcast.Dialog
{
    /// <summary>
    /// wndSite.xaml 的互動邏輯
    /// </summary>
    public partial class wndSite : Window
    {
        BroadcastDataSet ds=new BroadcastDataSet();
        public wndSite()
        {
            InitializeComponent();
        }

             wpfBroadcast.BroadcastDataSetTableAdapters.tblSIteTableAdapter broadcastDataSettblSIteTableAdapter ;
             wpfBroadcast.BroadcastDataSet broadcastDataSet;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

              broadcastDataSet = ((wpfBroadcast.BroadcastDataSet)(this.FindResource("broadcastDataSet")));
            // 將資料載入資料表 tblSIte。您可以依需要修改這個程式碼。
        broadcastDataSettblSIteTableAdapter=   new wpfBroadcast.BroadcastDataSetTableAdapters.tblSIteTableAdapter();
            broadcastDataSettblSIteTableAdapter.Fill(broadcastDataSet.tblSIte);
            System.Windows.Data.CollectionViewSource tblSIteViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tblSIteViewSource")));
            tblSIteViewSource.View.MoveCurrentToFirst();
            broadcastDataSet.tblSIte.TableNewRow += tblSIte_TableNewRow;
        }

        void tblSIte_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
           // throw new NotImplementedException();
            wpfBroadcast.BroadcastDataSet.tblSIteRow row = e.Row as wpfBroadcast.BroadcastDataSet.tblSIteRow;
            row.AC = false; ;
            row.Amp = false;
            row.Comm = false;
            row.DC = false;
            row.DoorOpen = false;
            row.Speaker = false;
            

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            broadcastDataSettblSIteTableAdapter.Update(this.broadcastDataSet);
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void btnOk_Copy_Click(object sender, RoutedEventArgs e)
        {
            new wndReportViewer(this.broadcastDataSet,new report.rptSite()).ShowDialog();
        }
    }
}
