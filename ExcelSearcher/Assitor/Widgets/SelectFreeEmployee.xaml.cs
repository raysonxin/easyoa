using Assitor.Models;
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
    /// SelectFreeEmployee.xaml 的交互逻辑
    /// </summary>
    public partial class SelectFreeEmployee : Window, INotifyPropertyChanged
    {
        public SelectFreeEmployee(EventHandler handler, int projId)
        {
            InitializeComponent();
            selectCallback = handler;
            ProjId = projId;
            this.DataContext = this;
            this.Loaded += SelectFreeEmployee_Loaded;
        }

        public EventHandler selectCallback;
        private int ProjId;

        private List<EmployeeModel> employees;
        public List<EmployeeModel> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                RaisePropertyChanged("Employees");
            }
        }

        private EmployeeModel current;
        public EmployeeModel Current
        {
            get { return current; }
            set
            {
                current = value;
                RaisePropertyChanged("Current");
            }
        }
        HttpProxy proxy = new HttpProxy(AppCache.Host);


        private async void SelectFreeEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            dpStart.SelectedDate = DateTime.Now;
            var ret = await proxy.GetMessage<BatchResult<EmployeeModel>>("v1/oa/employee/free");
            if (ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = ret.Content.Data;
                if (result != null && result.Count > 0)
                {
                    Employees = result;
                }
                else
                {
                    MessageBox.Show("当前无空闲人员。");
                }
            }
        }

        private async void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (Current == null)
            {
                MessageBox.Show("请选择人员！");
                return;
            }

            ProjectStaffModel model = new ProjectStaffModel
            {
                DingId = Current.DingId,
                ProjId = ProjId,
                StartDate = (DateTime)dpStart.SelectedDate,
            };

            var ret = await proxy.PostMessage<OneResult<ProjectStaffModel>>("v1/oa/projstaff/assign", model);
            if (ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                selectCallback(true, null);
                this.Close();
            }
            else
            {
                MessageBox.Show("添加失败！");
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
    }
}
