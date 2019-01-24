using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Dto
{
    public enum WorkListStatusEnum {
        [Description("工单未处理")]
        NotStart,
        [Description("处理工单中")]
        Start,
        [Description("工单完成")]
        End
    }
    public enum DetailRole
    {
        [Description("消费者")]
        Customer,
        [Description("管理员")]
        Admin
    }
    public enum ItemLevel {
        [Description("正常")]
        Normal,
        [Description("高级")]
        High
    }
    public enum ItemType {
        [Description("前端开发")]
        QianDuan,
        [Description("需求整理")]
        XuQiu,
        [Description("设计")]
        UI,
        [Description("后端开发")]
        HouDuan,
        [Description("功能优化迭代")]
        DieDai,
        [Description("Bug与测试")]
        Bug,
        [Description("其他问题")]
        Others, 
    }
    public enum ItemStatus
    {
        [Description("需求整理中")]
        XuQiu,
        [Description("已分配")]
        HasPei,
        [Description("处理中")]
        InHand,
        [Description("测试中")]
        Test,
        [Description("已逾期")]
        HasYu,
        [Description("已完成")]
        End,
        [Description("逾期完成")]
        YuEnd,

    }
    public enum WorkListDetailStatusEnum
    {
        [Description("确认需求中")]
        XuQiuIng,
        [Description("需求已确认")]
        XuQiuOk,
        [Description("UI设计中")]
        UIIng,
        [Description("UI设计完成")]
        UIOk,
        [Description("前端开发中")]
        QianIng,
        [Description("前端开发完成")]
        QianOk,
        [Description("后台开发中")]
        HouIng,
        [Description("后台开发完成")]
        HouOk,
        [Description("测试中")]
        TestIng,
        [Description("测试完成")]
        TestOk,
        [Description("客户确认中")]
        KeHuIng,
        [Description("客户已确认")]
        KeHuOk,
        [Description("已上线")]
        End
    }
    public enum PayVoucherEnum
    {
        [Description("未支付")]
        NotPay = 1 ,
        [Description("部分支付")]
        PayPart = 2 ,
        [Description("支付完成")]
        PayAll = 3 
    }
}
