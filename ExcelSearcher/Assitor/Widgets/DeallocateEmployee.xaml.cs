using Assitor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Assitor.Widgets
{
    /// <summary>
    /// DeallocateEmployee.xaml 的交互逻辑
    /// </summary>
    public partial class DeallocateEmployee : Window, INotifyPropertyChanged
    {
        public DeallocateEmployee(Models.ProjectStaffModel staff)
        {
            InitializeComponent();
            Staff = staff;
            dpStop.SelectedDate = DateTime.Now;
            this.DataContext = this;
        }

        HttpProxy proxy = new HttpProxy(AppCache.Host);

        private Models.ProjectStaffModel staff;
        public Models.ProjectStaffModel Staff
        {
            get { return staff; }
            set
            {
                staff = value;
                RaisePropertyChanged("Staff");
            }
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性改变事件
        /// </summary>
        /// <param name="name"></param>
        private void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        private async void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var ret = await proxy.RemoveMessage<OneResult<int>>("v1/oa/projstaff/" + Staff.Id + "/" + ((DateTime)dpStop.SelectedDate).ToString("yyyy-MM-dd"), null);//, new { id = Staff.Id, stop = (DateTime)dpStop.SelectedDate });
            if (ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("移除人员成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("移除人员失败！");
            }
        }
    }
}
