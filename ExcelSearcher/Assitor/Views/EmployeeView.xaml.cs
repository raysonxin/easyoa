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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assitor.Views
{
    /// <summary>
    /// EmployeeView.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeView : UserControl, IPageView
    {
        public string Title { get; set; } = "人员管理";

        public EmployeeView()
        {
            InitializeComponent();
            this.Loaded += ProjectView_Loaded;
        }

        public List<EmployeeModel> Projects { get; set; }

        private void ProjectView_Loaded(object sender, RoutedEventArgs e)
        {
            GetProjects();
        }

        private async void GetProjects()
        {
            HttpProxy proxy = new HttpProxy(AppCache.Host);

            var ret = await proxy.GetMessage<BatchResult<EmployeeModel>>("v1/oa/employee");
            if (ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = ret.Content.Data;
                if (result != null && result.Count > 0)
                {
                    Projects = result;
                    dgResult.ItemsSource = Projects;
                }
            }
        }
    }
}
