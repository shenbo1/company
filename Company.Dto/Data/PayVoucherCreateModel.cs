using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Dto.Data
{
    public class PayVoucherCreateModel
    {
        /// <summary>
        /// 项目
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        public DateTime BillDate { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public ProjectPayWayEnum PayWay { get; set; }
        /// <summary>   
        /// 支付金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 支付周期
        /// </summary>
        public int PayPeriod { get; set; }
        public int BillType { get; set; }
    }
}
