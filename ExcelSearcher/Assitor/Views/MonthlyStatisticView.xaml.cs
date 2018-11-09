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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assitor.Views
{
    /// <summary>
    /// MonthlyStatisticView.xaml 的交互逻辑
    /// </summary>
    public partial class MonthlyStatisticView : UserControl, IPageView, INotifyPropertyChanged
    {
        public string Title { get; set; } = "项目人力投入月统计";
        public MonthlyStatisticView()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += MonthlyStatisticView_Loaded;
        }



        HttpProxy proxy = new HttpProxy(AppCache.Host);

        private List<ProjectStatModel> projectStats;
        public List<ProjectStatModel> ProjectStats
        {
            get { return projectStats; }
            set
            {
                projectStats = value;
                RaisePropertyChanged("ProjectStats");
            }
        }

       

        private void MonthlyStatisticView_Loaded(object sender, RoutedEventArgs e)
        {
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

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var result = await proxy.GetMessage<BatchResult<ProjectStatModel>>("v1/oa/report/project/monthly", new { year = 2018, month = 10 });
            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                ProjectStats = result.Content.Data;
                if (ProjectStats!=null&& ProjectStats.Count > 0)
                {

                }
            }

        }
    }
}
