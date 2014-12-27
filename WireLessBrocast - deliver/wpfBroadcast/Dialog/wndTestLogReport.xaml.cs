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
    /// wndLoginReport.xaml 的互動邏輯
    /// </summary>
    public partial class wndTestLogReport : Window
    {

        string reporttype;
        public wndTestLogReport( )
        {
            InitializeComponent();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.repotyViewier.ViewerCore.ReportSource = null;
            wpfBroadcast.BroadcastDataSetTableAdapters.tblTestLogTableAdapter adp = new BroadcastDataSetTableAdapters.tblTestLogTableAdapter();
            wpfBroadcast.BroadcastDataSet ds = new BroadcastDataSet();
           adp.FillByRange(ds.tblTestLog,(DateTime)dtpBeginDate.SelectedDate, ((DateTime)dtpEndDate.SelectedDate).AddDays(1) );
            report.rpSchTestLog rpt;
            if (repotyViewier.ViewerCore.ReportSource == null)
            {
                rpt = new report.rpSchTestLog();
             
                rpt.SetDataSource(ds);
                this.repotyViewier.ViewerCore.ReportSource = rpt;
            }

            else
            {
                rpt = this.repotyViewier.ViewerCore.ReportSource as report.rpSchTestLog;
                rpt.SetDataSource(ds);
                this.repotyViewier.ViewerCore.RefreshReport();
            }
          
          
        
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.dtpEndDate.SelectedDate = (e.Source as DatePicker).SelectedDate;
           

          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.dtpBeginDate.SelectedDate = System.DateTime.Now.Date;

        }
    }
}
