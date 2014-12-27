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
    /// wndUserSetting.xaml 的互動邏輯
    /// </summary>
    public partial class wndUserSetting : Window
    {
        public wndUserSetting()
        {
            InitializeComponent();
        }

        private System.Data.Objects.ObjectQuery<tblUser> GettblUserQuery(BroadcastEntities broadcastEntities)
        {


            System.Data.Objects.ObjectQuery<wpfBroadcast.tblUser> tblUserQuery = broadcastEntities.tblUser;
            // 若要明確載入資料，您必須加入類似下面的 Include 方法:
            // tblUserQuery = tblUserQuery.Include("tblUser.tblUserGroup").
            // 如需詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=157380
            // 傳回 ObjectQuery。
            return tblUserQuery;
        }
        wpfBroadcast.BroadcastEntities broadcastEntities;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            broadcastEntities = new wpfBroadcast.BroadcastEntities();
            // 將資料載入 tblUser。您可以依需要修改這個程式碼。
            System.Windows.Data.CollectionViewSource tblUserViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tblUserViewSource")));
            System.Data.Objects.ObjectQuery<wpfBroadcast.tblUser> tblUserQuery = this.GettblUserQuery(broadcastEntities);
            tblUserViewSource.Source = tblUserQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);

            tblUserViewSource.View.CollectionChanged += View_CollectionChanged;
          
        }

        void View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (tblUser usr in e.NewItems)
                {
                    usr.GroupID = 1;
                }
            }
            //throw new NotImplementedException();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // System.Windows.Data.CollectionViewSource tblUserViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tblUserViewSource")));
            //(tblUserViewSource.View as BindingListCollectionView).CommitEdit();
            //(tblUserViewSource.View as BindingListCollectionView).CommitNew();
            //(tblUserViewSource.View as BindingListCollectionView).Refresh();
            
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            broadcastEntities.SaveChanges();
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
