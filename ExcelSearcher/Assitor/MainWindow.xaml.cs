using Assitor.Views;
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

namespace Assitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var view = new Views.ImportCheckView();
            ChangeView(view);
        }

        private void btnOpenView_Click(object sender, RoutedEventArgs e)
        {
            UserControl view = new UserControl();
            switch (((Button)sender).Tag.ToString())
            {
                case "ImportCheck":
                    view = new Views.ImportCheckView();
                    break;
                case "Project":
                    view = new ProjectView();
                    break;
                case "Employee":
                    view = new EmployeeView();
                    break;
                case "StaffAllocate":
                    view = new StaffAllocateView();
                    break;
                case "Monthly":
                    view = new MonthlyStatisticView();
                    break;
                default:
                    break;
            }
            ChangeView(view);
        }

        private void ChangeView(UserControl view)
        {
            if (view is IPageView)
            {
                txtTitle.Text = (view as IPageView).Title;
            }
            mainContainer.Child = view;
        }
    }
}
