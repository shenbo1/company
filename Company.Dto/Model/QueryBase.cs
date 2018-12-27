using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Dto.Model
{
    public class QueryBase
    {
        public int TypeId { get; set; }
        public int CompanyId { get; set; }
        public int CusMemberId { get; set; }
        public string KeyWord { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string Order{ get; set; }
    }
    public class WorkListQuery : QueryBase {
        public int ProjectId { get; set; }
        public string Status { get; set; }
    }
    public class WorkListItemQuery : QueryBase
    {
        public int WorkId { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
    }
}
