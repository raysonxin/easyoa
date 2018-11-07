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
    /// ProjectView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectView : UserControl, IPageView
    {
        public string Title { get; set; } = "项目管理";
        public ProjectView()
        {
            InitializeComponent();
            this.Loaded += ProjectView_Loaded;
        }

        public List<ProjectModel> Projects { get; set; }

        private void ProjectView_Loaded(object sender, RoutedEventArgs e)
        {
            GetProjects();
        }

        private async void GetProjects()
        {
            HttpProxy proxy = new HttpProxy(AppCache.Host);

            var ret = await proxy.GetMessage<BatchResult<ProjectModel>>("v1/oa/project");
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
