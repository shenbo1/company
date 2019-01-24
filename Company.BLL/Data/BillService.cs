using Company.DAL.Data;
using Company.Dto;
using Company.Dto.Data;
using Company.Dto.Request;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Data
{
    public class BillService
    {
        /// <summary>
        /// 账单生成逻辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Tuple<Bill,List<BillDetail>> CreateBillDetail(PayVoucherCreateModel model)
        {
            var createBy = CookieOperate.MemberCookie.UserName;
            var bill = new Bill()
            {
                Guid = Guid.NewGuid().ToString(),
                ProjectId = model.ProjectId,
                BillDate = model.BillDate,
                CreateDate = DateTime.Now,
                PayWay = (int)model.PayWay,
                PayDate = model.StartDate,
                PayPeriod = model.PayPeriod,
                PayMoney = model.Money,
                CreateBy = createBy
            };

            var list = new List<BillDetail>();
            var billStartTime = model.BillDate;
            var payStartTime = model.StartDate;
            var payWay = (int)model.PayWay;
            //var billGuid = Guid.NewGuid().ToString();
            for (int i = 0; i < model.PayPeriod; i++)
            {
                var res = new BillDetail()
                {
                    Guid = Guid.NewGuid().ToString(),
                    NeedPayDate = payStartTime,
                    NeedPayMoeny = model.Money,
                    PayStatus = (int)PayVoucherEnum.NotPay,
                    ProjectId = model.ProjectId,
                    BillStartTime = billStartTime,
                    BillGuid = bill.Guid,
                    PayWay = payWay,                 
                    CreateDate = DateTime.Now,  
                    CreateBy = createBy,
                    BillType = model.BillType
                };
                payStartTime = res.NeedPayDate.Value.AddMonths(payWay);
                billStartTime = res.BillStartTime.Value.AddMonths(payWay);
                res.BillEndTime = billStartTime.AddDays(-1);
                res.BillContent = string.Format("账期:{0}-{1}", res.BillStartTime.Value.ToString("yyyy/MM/dd"),
                    res.BillEndTime.Value.ToString("yyyy/MM/dd"));
                list.Add(res);
            }

            return new Tuple<Bill, List<BillDetail>>(bill, list);
        }

        public static bool CreateBill(PayVoucherCreateModel model) {
            var project = ProjectManageDBOperate.GetModelById(model.ProjectId);
            var res = CreateBillDetail(model);

            #region 账单信息填充
            var bill = res.Item1;
            bill.ProjectName = project.Name;
            bill.ProjectId = project.Id;
            bill.CusCompanyId = project.CusCompanyId;
            bill.CusMemberId = project.CusMemberId;
            #endregion

            #region 账单详情
            var billDetail = res.Item2;
            billDetail.ForEach(a => { a.ProjectName = bill.ProjectName; a.CusCompanyId = bill.CusCompanyId; a.CusMemberId = bill.CusMemberId; });
            #endregion

            return BillDBOperate.AddBill(bill, billDetail);
        }
        public static Tuple<bool,string> BillPay(BillPayRequest request)
        {
            var bill = BillDetailDBOperate.GetModelById(request.PayId);
            if (bill == null) { return Tuple.Create<bool, string>(false, "PayId错误"); }
            bill.PayName = request.Fu;
            bill.RelPayMoeny += request.Money;
            bill.PayStatus = bill.RelPayMoeny < bill.NeedPayMoeny ? (int)PayVoucherEnum.PayPart : (int)PayVoucherEnum.PayAll;
            bill.RelPayDate = request.FuTime;
            bill.PayVouchers = request.PingZheng;
            bill.Remark = request.Remark;
            bill.ReceiveName = request.Shou;
            bill.PaySource = request.Way;
            var flag =  BillDetailDBOperate.PayBillDetail(bill);
            return Tuple.Create<bool, string>(flag, flag ? "支付成功" : "支付失败");
        }
    }
}
