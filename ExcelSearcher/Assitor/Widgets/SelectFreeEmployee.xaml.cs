using Assitor.Models;
using Assitor.Utils;
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

namespace Assitor.Widgets
{
    /// <summary>
    /// SelectFreeEmployee.xaml 的交互逻辑
    /// </summary>
    public partial class SelectFreeEmployee : Window
    {
        public SelectFreeEmployee(EventHandler handler)
        {
            InitializeComponent();
            selectCallback = handler;
            this.Loaded += SelectFreeEmployee_Loaded;
        }

        public EventHandler selectCallback;

        HttpProxy proxy = new HttpProxy(AppCache.Host);


        private async void SelectFreeEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            var ret = await proxy.GetMessage<BatchResult<EmployeeModel>>("v1/oa/employee/free");
            if (ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = ret.Content.Data;
                if (result != null && result.Count > 0)
                {
                    dgResult.ItemsSource = result;
                }
                else
                {
                    MessageBox.Show("当前无空闲人员。");
                }
            }
        }
    }
}
