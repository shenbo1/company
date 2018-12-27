using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Dto
{
    public enum CompanySexEnum
    {
        [Description("男")]
        Male = 1,
        [Description("女")]
        Female = 2,
        [Description("保密")]
        Secret = 3
    }
    public enum HuKouEnum
    {
        [Description("城镇户口")]
        City = 1,
        [Description("非城镇户口")]
        NotCity = 2
    }
    public enum PoliticalEnum {
        [Description("中共党员")]
        Party =1,
        [Description("共青团员")]
        League = 2,
        [Description("群众")]
        Normal = 3,
        [Description("其他")]
        Others = 4
    }
    public enum EducationEnum {
        [Description("博士")]
        Doctor = 1,
        [Description("研究生")]
        Graduate = 2,
        [Description("本科")]
        Benke = 3,
        [Description("大专")]
        DaZhuan = 4,
        [Description("高中")]
        High = 5,
        [Description("初中")]
        Middle = 6,
        [Description("小学")]
        Primary = 7,
        [Description("其他")]
        Others = 8,
    }
}
