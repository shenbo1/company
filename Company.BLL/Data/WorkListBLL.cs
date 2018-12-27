using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Company.Dto;
using Company.DAL.Data;
using System.Data.SqlClient;
using Company.Util;
using Company.Dto.Data;

namespace Company.BLL.Data
{
    public class WorkListBLL
    {
        private static object company;

        public static bool ImportExcel(DataTable dt, CompanyUser user, int projectId,string status,out string message)
        {
            message = string.Empty;
            try
            {
                //工单名称
                var workName = dt.Columns[1].ColumnName.Trim();
                var project = ProjectManageDBOperate.GetModelById(projectId);
                if (project == null) { message = "项目编号有误,请核实"; return false; }
                var guid = Guid.NewGuid().ToString();
                if (WorkListDBOperate.GetCountByName(workName) > 0) {
                    message = "工单名称已存在,请重新编辑"; return false;
                }
                WorkList work = new WorkList()
                {
                    Name = workName,
                    CreateBy = user.UserName,
                    CreateDate = DateTime.Now,
                    Length = 0,
                    Status = status,
                    CompanyId = user.CompanyId,
                    CompanyName = user.CompanyName,
                    ProjectId = projectId,
                    ProjectName = project.Name,
                    CusCompanyId = project.CusCompanyId,
                    CusMemberId = project.CusMemberId,
                    Guid = guid,
                };
                var list = new List<WorkListDetail>();
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var detail = new WorkListDetail()
                    {
                        Infos = dr[1].ToString().Trim(),
                        RoleCode = dr[0].ToString(), //== "消费者" ? DetailRole.Customer.ToString() : DetailRole.Admin.ToString(),
                        CreateBy = user.UserName,
                        WorkId = guid,
                        Index = i ,
                        Guid = Guid.NewGuid().ToString()
                    };
                    if (string.IsNullOrEmpty(detail.Infos)) { continue; }
                    else if (detail.Infos.Length > 100) {
                        message += (i+2).ToString()+",";
                    }
                    list.Add(detail);
                }
                if (!string.IsNullOrEmpty(message)) {
                    message = "数据第[" + message.Substring(0, message.Length - 1) + "]条内容字数超过100字";
                    return false;

                }
                return WorkListDBOperate.Import(work, list, out message);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        public static bool ModifyDetail(List<WorkListDetail> list,string userId) {
            var insertList = new List<WorkListDetail>();
            var updateList = new List<WorkListDetail>();
            //var i = 0;
            foreach (var item in list)
            {
                //if (item.IsDeleted == 0) { i++; }
                var newModel = new WorkListDetail()
                {
                    WorkId = item.WorkId,
                    Index = item.Index ,
                    Infos = item.Infos,
                    IsDeleted = item.IsDeleted,
                    RoleCode = item.RoleCode,
                    CreateBy = userId,
                    ModifyBy = userId
                };
                if (item.Id != 0) { newModel.Id = item.Id; updateList.Add(newModel); }
                else
                {
                    insertList.Add(newModel);
                }
            }
            return WorkListDBOperate.ModifyDetail(updateList, insertList);
        }

        public static bool AddWorkList(WorkList workList,List<WorkListEdit> list) {
            var detailList = new List<WorkListDetail>();
            var itemList = new List<WorkListItems>();
            var index = 1;
            foreach (var item in list)
            {
                var detail = new WorkListDetail() {
                    WorkId = workList.Guid,
                    Index = index,
                    Infos = item.Name,
                    RoleCode = item.Role,
                    Guid = Guid.NewGuid().ToString(),
                    CreateBy = workList.CreateBy,
                };
                var infos = item.Infos.Split('\n');
                foreach (var item1 in infos)
                {
                    if (string.IsNullOrEmpty(item1)) { continue; }
                    var itemDetail = new WorkListItems()
                    {
                        CompanyId = workList.CompanyId,
                        CompanyName = workList.CompanyName,
                        ProjectId = workList.ProjectId,
                        WorkId = workList.Id,
                        Name = item.Name,
                        Level = 999,
                        Status = ItemStatus.XuQiu.ToString(),
                        Infos = item1,
                        DetailGuid = detail.Guid,
                        CreateBy = workList.CreateBy,
                    };
                    itemList.Add(itemDetail);
                }
                detailList.Add(detail);
                index ++;
            }
            return WorkListDBOperate.InsertWorkList(workList, detailList, itemList);
        }


//        update WorkListDetail set guid = newid()


//update WorkListItems  set DetailGuid =
//(select guid from WorkListDetail b where detailid = b.id)

    }
}
