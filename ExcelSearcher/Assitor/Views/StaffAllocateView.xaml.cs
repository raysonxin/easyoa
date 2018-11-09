using Assitor.Models;
using Assitor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// StaffAllocateView.xaml 的交互逻辑
    /// </summary>
    public partial class StaffAllocateView : UserControl, IPageView, INotifyPropertyChanged
    {
        public StaffAllocateView()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += StaffAllocateView_Loaded;
        }
        HttpProxy proxy = new HttpProxy(AppCache.Host);

        private ObservableCollection<ProjectModel> projcets;
        public ObservableCollection<ProjectModel> Projects
        {
            get { return projcets; }
            set
            {
                projcets = value;
                RaisePropertyChanged("Projects");
            }
        }

        private ObservableCollection<ProjectStaffModel> projcetStaffes;
        public ObservableCollection<ProjectStaffModel> ProjectStaffes
        {
            get { return projcetStaffes; }
            set
            {
                projcetStaffes = value;
                RaisePropertyChanged("ProjectStaffes");
            }
        }

        private ProjectModel currentProject;
        public ProjectModel CurrentProject
        {
            get { return currentProject; }
            set
            {
                currentProject = value;
                RaisePropertyChanged("CurrentProject");
                GetProjectStaffes();
            }
        }

        private ProjectStaffModel currentProjStaff;
        public ProjectStaffModel CurrentProjStaff
        {
            get { return currentProjStaff; }
            set
            {
                currentProjStaff = value;
                RaisePropertyChanged("CurrentProjStaff");
            }
        }

        private void StaffAllocateView_Loaded(object sender, RoutedEventArgs e)
        {
            GetProjects();
        }

        public string Title { get; set; } = "项目人员分配";

        private async void GetProjects()
        {
            var ret = await proxy.GetMessage<BatchResult<ProjectModel>>("v1/oa/project");
            if (ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = ret.Content.Data;
                if (result != null && result.Count > 0)
                {
                    Projects = new ObservableCollection<ProjectModel>(result);
                    CurrentProject = Projects[0];
                }
            }
        }

        private async void GetProjectStaffes()
        {
            if (CurrentProject == null)
                return;

            var ret = await proxy.GetMessage<BatchResult<ProjectStaffModel>>("v1/oa/projstaff/project?proj=" + CurrentProject.Id);
            if (ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = ret.Content.Data;
                if (result != null && result.Count > 0)
                {
                    ProjectStaffes = new ObservableCollection<ProjectStaffModel>(result);
                    CurrentProjStaff = ProjectStaffes[0];
                }
                else
                {
                    ProjectStaffes = null;
                    CurrentProjStaff = null;
                }
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

        private void btnSelectAdd_Click(object sender, RoutedEventArgs e)
        {

            if (CurrentProject == null)
            {
                MessageBox.Show("请选择项目！");
                return;
            }

            var win = new Widgets.SelectFreeEmployee((s, args) =>
            {
                if (s != null)
                {
                    GetProjectStaffes();
                }
            }, CurrentProject.Id);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProjStaff == null)
            {
                MessageBox.Show("请选择需要移除的人员！");
                return;
            }

            Widgets.DeallocateEmployee win = new Widgets.DeallocateEmployee(CurrentProjStaff);
            win.ShowDialog();
            GetProjectStaffes();
        }
    }
}
