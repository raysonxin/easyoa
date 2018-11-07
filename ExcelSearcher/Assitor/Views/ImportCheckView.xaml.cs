using Assitor.Models;
using Assitor.Utils;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
    /// ImportCheckView.xaml 的交互逻辑
    /// </summary>
    public partial class ImportCheckView : UserControl, IPageView
    {
        public string Title { get; set; } = "考勤数据导入";

        public ImportCheckView()
        {
            InitializeComponent();
        }

        List<CheckModel> dataSource = new List<CheckModel>();
        List<CheckModel> checkList = new List<CheckModel>();

        private void btnBrowserFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files|*.xls;*.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.txtFilePath.Text = openFileDialog.FileName;
            }

            dataSource.Clear();
            using (var stream = File.Open(txtFilePath.Text, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                    dtFormat.ShortDatePattern = "yy/MM/dd";
                    foreach (DataTable item in dataSet.Tables)
                    {
                        foreach (DataRow row in item.Rows)
                        {
                            var job = new CheckModel
                            {
                                Name = row[0].ToString(),
                                Job = row[1].ToString(),
                                DingId = row[2].ToString(),
                                CheckDate = Convert.ToInt32(Convert.ToDateTime(row[3].ToString().Substring(0, 8), dtFormat).ToString("yyyyMMdd")),
                                CheckIn = row[4].ToString().Length > 6 ? ParseTime(row[4].ToString()) : row[4].ToString(),
                                CheckOut = row[5].ToString().Length > 6 ? ParseTime(row[5].ToString()) : row[5].ToString(),
                            };
                            dataSource.Add(job);
                        }
                    }
                }
            }

            checkList = dataSource;
            dgResult.ItemsSource = checkList;
        }

        private string ParseTime(string time)
        {
            if (time.Contains(" "))
            {
                var parts = time.Split(' ');
                var shorts = parts[1].Split(':');
                return shorts[0] + ":" + shorts[1];
            }
            var t = time.Split(':');
            return t[0] + ":" + t[1];
        }


        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            HttpProxy proxy = new HttpProxy(AppCache.Host);

            var ret = await proxy.PostMessage<BatchResult<CheckModel>>("v1/oa/checktime/ones", checkList);

            if (ret.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("数据导入成功！");
            }
            else
            {
                MessageBox.Show("数据导入错误！" + ret.Error, "操作提示");
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            checkList = dataSource.Where(f => f.CheckDate >= Convert.ToInt32(dpStart.SelectedDate?.ToString("yyyyMMdd")) && f.CheckDate <= Convert.ToInt32(dpStop.SelectedDate?.ToString("yyyyMMdd"))).ToList();
            dgResult.ItemsSource = checkList;
        }
    }
}
