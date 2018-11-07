using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelSearcher
{
    public class CheckModel
    {
        [Description("姓名")]
        public string Name { get; set; }

        [Description("职位")]
        public string Job { get; set; }

        [Description("UserId")]
        public string DingId { get; set; }

        [Description("日期")]
        public DateTime CheckDate { get; set; }

        [Description("上班1打卡时间")]
        public string CheckIn { get; set; }

        [Description("下班1打卡时间")]
        public string CheckOut { get; set; }

        public bool IsAttend { get; set; }
    }

    public class CheckStatisticModel
    {
        [Description("姓名")]
        public string Name { get; set; }

        [Description("职位")]
        public string Job { get; set; }

        [Description("UserId")]
        public string DingId { get; set; }

        public int Days { get; set; }

        public double Hours { get; set; }

        public double Attends { get; set; }
    }

    public class CheckStatisticEquality : IEqualityComparer<CheckStatisticModel>
    {
        public bool Equals(CheckStatisticModel x, CheckStatisticModel y)
        {
            return x.DingId == y.DingId;
        }

        public int GetHashCode(CheckStatisticModel obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
