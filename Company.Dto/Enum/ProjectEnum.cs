using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Dto
{
    public enum ProjectPayWayEnum
    {
        [Description("年付")]
        Year = 12,
        [Description("半年付")]
        HalfYear = 6,
        [Description("季付")] 
        Season = 3,
        [Description("月付")]
        Month = 1,
        [Description("未知")]
        UnKnown = 0 
    }
    public enum ProjectStatusEnum
    {
        [Description("进行中")]
        Start,
        [Description("维护中")]
        Repair,
        [Description("已完成")]
        Finished,
        [Description("已结束")]
        End,
        [Description("搁置中")]
        Stop,
        [Description("未开始")]
        NotStart,
    }
    public enum CustomerCompanyStatusEnum {
    
        [Description("合作未开始")]
        NotStart,
        [Description("合作中")]
        Start,
        [Description("合作已结束")]
        End
    }
    public enum OpereateTypeEnum
    {

        [Description("外包")]
        Outer = 1,
        [Description("合伙")]
        Parr = 2,
        [Description("其他")]
        Other = 3
    }
    public enum DevTypeEnum
    {

        [Description("软件")]
        Soft=1,
        [Description("物联网")]
        Wu=2,
        [Description("其他")]
        Other = 3
    }
    public enum CompanyStatusEnum
    {

        [Description("规划中")]
        GUIHUA=1,
        [Description("未注册")]
        WEIZHUCE=2,
        [Description("注册中")]
        ZHUCEZ = 3,
        [Description("已注册")]
        YIZHUCE = 4,
        [Description("成立1-3年")]
        ONE = 5,
        [Description("成立3-5年")]
        THREE = 6,
    }
}
