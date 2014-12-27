using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
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
using wpfBroadcast.report;

namespace wpfBroadcast
{
    /// <summary>
    /// wndReportViewer.xaml 的互動邏輯
    /// </summary>
    public partial class wndReportViewer : Window
    {
        public wndReportViewer(object ds,ReportClass rpt)
        {
            InitializeComponent();
            ReportClass report =rpt;
            report.SetDataSource(ds);
            this.reportViewer.ViewerCore.ReportSource = report;

        }
    }
}
