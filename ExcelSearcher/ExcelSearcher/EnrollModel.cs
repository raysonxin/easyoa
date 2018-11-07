using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelSearcher
{
    public class EnrollModel
    {
        //部门名称、部门代码、用人司局、招考职位、职位代码、招考人数、资格审查通过人数
        public const string BmName_REF = "部门名称";
        public string BmName { get; set; }


        public const string BmCode_REF = "部门代码";
        public string BmCode { get; set; }

        public const string BmSiju_REF = "用人司局";
        public string BmSiju { get; set; }

        public const string Job_REF = "招考职位";
        public string Job { get; set; }

        public const string JobCode_REF = "职位代码";
        public string JobCode { get; set; }

        public const string NeedNumber_REF = "招考人数";
        public int NeedNumber { get; set; }

        public const string PassNumber_REF = "资格审查通过人数";
        public int PassNumber { get; set; }
    }
}
