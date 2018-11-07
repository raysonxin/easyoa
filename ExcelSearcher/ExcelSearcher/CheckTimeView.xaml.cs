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
using System.Windows.Shapes;

namespace ExcelSearcher
{
    /// <summary>
    /// CheckTimeView.xaml 的交互逻辑
    /// </summary>
    public partial class CheckTimeView : Window
    {
        public CheckTimeView()
        {
            InitializeComponent();
        }

        List<CheckModel> checkList = new List<CheckModel>();

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files|*.xls;*.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.txtEnrollFile.Text = openFileDialog.FileName;
            }
        }

        private void btnReader_Click(object sender, RoutedEventArgs e)
        {
            checkList.Clear();
            using (var stream = File.Open(txtEnrollFile.Text, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });


                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

                    dtFormat.ShortDatePattern = "yy/MM/dd";

                    foreach (DataTable item in result.Tables)
                    {
                        foreach (DataRow row in item.Rows)
                        {
                            var job = new CheckModel
                            {
                                // BmSiju = row[EnrollModel.BmSiju_REF].ToString(),
                                Name = row[0].ToString(),
                                Job = row[1].ToString(),
                                DingId = row[2].ToString(),
                                CheckDate = Convert.ToDateTime(row[3].ToString().Substring(0, 8), dtFormat),
                                CheckIn = row[4].ToString(),
                                CheckOut = row[5].ToString()
                            };
                            checkList.Add(job);
                        }
                    }
                }
            }


        }

        private async void btnUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employees = checkList.Select(f => new { f.Name, f.Job, f.DingId }).Distinct();
            HttpProxy proxy = new HttpProxy("http://192.168.192.134:9999/");
            foreach (var item in employees)
            {
                var ret = await proxy.PostMessage<string>("v1/oa/employee", item);
            }

        }

        private void btnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();

            dtFormat.ShortTimePattern = "HH:mm:ss";
            Random rand = new Random();
            foreach (var item in checkList)
            {
                var date = item.CheckDate.ToString("yyyy-MM-dd");

                if (!string.IsNullOrEmpty(item.CheckIn) || !string.IsNullOrEmpty(item.CheckOut))
                {
                    item.IsAttend = true;

                    if (!string.IsNullOrEmpty(item.CheckIn))
                    {
                        var inStr = item.CheckIn.Length > 5 ? item.CheckIn.Substring(item.CheckIn.Length - 8, 5) : item.CheckIn;
                        var checkIn = Convert.ToDateTime(string.Format("{0} {1}:00", date, inStr));
                        var checkInNormal = Convert.ToDateTime(string.Format("{0} 08:30:00", date));

                        item.CheckIn = checkIn > checkInNormal ? "08:2" + rand.Next(0, 9) : inStr;
                    }
                    else
                    {
                        item.CheckIn = "08:24";
                    }

                    if (!string.IsNullOrEmpty(item.CheckOut))
                    {
                        var outStr = item.CheckOut.Length > 5 ? item.CheckOut.Substring(item.CheckOut.Length - 8, 5) : item.CheckOut;
                        var checkOut = Convert.ToDateTime(string.Format("{0} {1}:00", date, outStr));
                        var checkOutNormal = Convert.ToDateTime(string.Format("{0} 18:30:00", date));

                        item.CheckOut = checkOut < checkOutNormal ? "18:3" + rand.Next(0, 9) : outStr;
                    }
                    else
                    {
                        item.CheckOut = "18:35";
                    }
                }
            }

            var employees = checkList.Select(f => new CheckStatisticModel { Name = f.Name, Job = f.Job, DingId = f.DingId }).Distinct(new CheckStatisticEquality()).ToList();
            foreach (var item in employees)
            {
                var current = checkList.Where(f => f.DingId == item.DingId).ToList();
                item.Days = current.Where(f => f.IsAttend).Count();

                item.Hours = current.Where(f => f.IsAttend).Sum(f => Convert.ToDateTime(f.CheckOut + ":00", dtFormat).Subtract(Convert.ToDateTime("18:00:00", dtFormat)).TotalHours);

                item.Attends = (double)item.Days + item.Hours / 8;
            }

            dgResult.ItemsSource = employees;
        }
    }
}
