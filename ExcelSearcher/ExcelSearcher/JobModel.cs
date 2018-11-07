using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelSearcher
{
    public class JobModel
    {
        public double 竞争 { get; set;}

        public string 报名人数 { get; set; }

        public string 招聘人数 { get; set; }

        public const string BmCode_REF = "部门代码";
        //部门代码
        public string BmCode { get; set; }

        public const string BmName_REF = "部门名称";
        //部门名称
        public string BmName { get; set; }

        public const string UpperBm_REF = "用人司局";
        //用人司局
        public string UpperBm { get; set; }

        public const string BmType_REF = "机构性质";
        //机构性质
        public string BmType { get; set; }

        public const string Job_REF = "招考职位";
        //招考职位
        public string Job { get; set; }

        public const string JobType_REF = "职位属性";
        //职位属性
        public string JobType { get; set; }

        public const string JobFenbu_REF = "职位分布";
        //职位分布
        public string JobFenbu { get; set; }

        public const string JobBrief_REF = "职位简介";
        //职位简介
        public string JobBrief { get; set; }

        public const string JobCode_REF = "职位代码";
        //职位代码
        public string JobCode { get; set; }

        public const string BmLevel_REF = "机构层级";
        //机构层级
        public string BmLevel { get; set; }

        public const string ExamType_REF = "考试类别";
        //考试类别
        public string ExamType { get; set; }

        public const string NeedNumber_REF = "招考人数";
        //招考人数
        public int NeedNumber { get; set; }

        public const string Major_REF = "专业";
        //专业
        public string Major { get; set; }

        public const string Education_REF = "学历";
        //学历
        public string Education { get; set; }

        public const string Degree_REF = "学位";
        //学位
        public string Degree { get; set; }

        public const string PoliticalStatus_REF = "政治面貌";
        //政治面貌
        public string PoliticalStatus { get; set; }

        public const string WorkYears_REF = "基层工作最低年限";
        //基层工作最低年限
        public string WorkYears { get; set; }

        public const string WorkProject_REF = "服务基层项目工作经历";
        //服务基层项目工作经历
        public string WorkProject { get; set; }

        public const string IsNeedMajor_REF = "是否在面试阶段组织专业能力测试";
        //是否在面试阶段组织专业能力测试
        public string IsNeedMajor { get; set; }

        public const string InterviewScale_REF = "面试人员比例";
        //面试人员比例
        public string InterviewScale { get; set; }

        public const string WorkPlace_REF = "工作地点";
        //工作地点
        public string WorkPlace { get; set; }

        public const string LuohuPlace_REF = "落户地点";
        //落户地点
        public string LuohuPlace { get; set; }

        public const string Mark_REF = "备注";
        //备注
        public string Mark { get; set; }

       
    }
}
