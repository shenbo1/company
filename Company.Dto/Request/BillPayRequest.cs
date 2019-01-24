using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Dto.Request
{
    public class BillPayRequest
    {
        public decimal Money { get; set; }
        public string Way { get; set; }
        public string Fu { get; set; }
        public DateTime FuTime { get; set; }
        public string PingZheng { get; set; }
        public string Shou { get; set; }
        public string Remark { get; set; }
        public int PayId { get; set; }
    }
}
