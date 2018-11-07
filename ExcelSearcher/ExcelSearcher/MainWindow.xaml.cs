using System;
using System.Collections.Generic;
using System.Data;
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
using ExcelDataReader;
//using 
namespace ExcelSearcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<JobModel> interestJobs;
        public List<JobModel> InterestJobs
        {
            get { return interestJobs; }
            set
            {
                interestJobs = value;
            }
        }

        public List<EnrollModel> EnrollStatistic { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEnrollFile.Text))
                return;
            LoadInterestExcel();
            LoadEnrollStatistic();
            CompareData();
        }

        private void LoadInterestExcel()
        {
            InterestJobs = new List<JobModel>();
            using (var stream = File.Open("my.xls", FileMode.Open, FileAccess.Read))
            {

                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    foreach (DataTable item in result.Tables)
                    {
                        foreach (DataRow row in item.Rows)
                        {
                            var job = new JobModel
                            {
                                BmCode = row[JobModel.BmCode_REF].ToString(),
                                BmLevel = row[JobModel.BmLevel_REF].ToString(),
                                BmName = row[JobModel.BmName_REF].ToString(),
                                BmType = row[JobModel.BmType_REF].ToString(),
                                Degree = row[JobModel.Degree_REF].ToString(),
                                Education = row[JobModel.Education_REF].ToString(),
                                ExamType = row[JobModel.ExamType_REF].ToString(),
                                InterviewScale = row[JobModel.InterviewScale_REF].ToString(),
                                IsNeedMajor = row[JobModel.IsNeedMajor_REF].ToString(),
                                Job = row[JobModel.Job_REF].ToString(),
                                JobBrief = row[JobModel.JobBrief_REF].ToString(),
                                JobCode = row[JobModel.JobCode_REF].ToString(),
                                JobFenbu = row[JobModel.JobFenbu_REF].ToString(),
                                JobType = row[JobModel.JobType_REF].ToString(),
                                LuohuPlace = row[JobModel.LuohuPlace_REF].ToString(),
                                Major = row[JobModel.Major_REF].ToString(),
                                Mark = row[JobModel.Mark_REF].ToString(),
                                NeedNumber = Convert.ToInt32(row[JobModel.NeedNumber_REF].ToString()),
                                PoliticalStatus = row[JobModel.PoliticalStatus_REF].ToString(),
                                UpperBm = row[JobModel.UpperBm_REF].ToString(),
                                WorkPlace = row[JobModel.WorkPlace_REF].ToString(),
                                WorkProject = row[JobModel.WorkProject_REF].ToString(),
                                WorkYears = row[JobModel.WorkYears_REF].ToString(),
                            };
                            InterestJobs.Add(job);
                        }
                        //System.Diagnostics.Debug.WriteLine(item.TableName);
                        //foreach (DataColumn column in item.Columns)
                        //{
                        //    System.Diagnostics.Debug.WriteLine(column.ColumnName);
                        //}
                    }

                }
            }
        }

        private void LoadEnrollStatistic()
        {
            EnrollStatistic = new List<EnrollModel>();
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


                    foreach (DataTable item in result.Tables)
                    {
                        foreach (DataRow row in item.Rows)
                        {
                            var job = new EnrollModel
                            {
                                BmSiju = row[EnrollModel.BmSiju_REF].ToString(),
                                BmCode = row[EnrollModel.BmCode_REF].ToString(),
                                BmName = row[EnrollModel.BmName_REF].ToString(),
                                Job = row[EnrollModel.Job_REF].ToString(),
                                JobCode = row[EnrollModel.JobCode_REF].ToString(),
                                NeedNumber = Convert.ToInt32(row[EnrollModel.NeedNumber_REF].ToString()),
                                PassNumber = Convert.ToInt32(row[EnrollModel.PassNumber_REF].ToString()),
                            };
                            EnrollStatistic.Add(job);
                        }
                    }
                }
            }
        }

        private void CompareData()
        {
            if (InterestJobs.Count <= 0 || EnrollStatistic.Count <= 0)
                return;

            foreach (var item in InterestJobs)
            {
                var enroll = EnrollStatistic.SingleOrDefault(f => f.JobCode == item.JobCode && f.BmCode == item.BmCode);
                if (enroll != null)
                {
                    item.竞争 = (double)enroll.PassNumber / enroll.NeedNumber;
                    item.报名人数 = enroll.PassNumber.ToString();
                    item.招聘人数 = enroll.NeedNumber.ToString();
                }
                else
                {
                    item.竞争 = 0;// (double)enroll.PassNumber / enroll.NeedNumber;
                    item.报名人数 = "无匹配";//enroll.PassNumber.ToString();
                    item.招聘人数 = "无匹配";//enroll.NeedNumber.ToString();
                }

            }
            InterestJobs = InterestJobs.OrderByDescending(f => f.竞争).ToList();
            dgResult.ItemsSource = InterestJobs;
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xls)|*.xls",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.txtEnrollFile.Text = openFileDialog.FileName;
            }
        }
    }
}
