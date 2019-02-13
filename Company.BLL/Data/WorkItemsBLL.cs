using Company.DAL.Data;
using Company.Dto;
using Company.Dto.Data;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Data
{
    public class WorkItemsBLL
    {
        public static bool ImportExcel(DataTable dt, CompanyUser user) {
            try
            {
                using (SqlBulkCopy sqlBC = new SqlBulkCopy(ConfigSetting.DataConnection))
                {
                    //一次批量的插入的数据量
                    sqlBC.BatchSize = dt.Rows.Count;
                    //超时之前操作完成所允许的秒数，如果超时则事务不会提交 ，数据将回滚，所有已复制的行都会从目标表中移除
                    sqlBC.BulkCopyTimeout = 1800;
                    //设置要批量写入的表
                    sqlBC.DestinationTableName = "WorkListItems";


                    DataTable newDt = new DataTable();

                    #region  赋值
                    var objType = typeof(WorkListItems);
                    PropertyInfo[] properties = objType.GetProperties();
                    string[] outColName = new string[] { "Id", "ModifyDate", "CanEdit", "ModifyBy", "LevelName", "LevelClass", "StatusName", "StatusClass", "WorkName", "ProjectName" };
                    foreach (var item in properties)
                    {
                        var name = item.Name;
                        if (outColName.Contains(name)) { continue; }
                        newDt.Columns.Add(new DataColumn() { ColumnName = item.Name });
                        sqlBC.ColumnMappings.Add(item.Name, item.Name);
                    }
                    var itemType = CommonMethod.GetEnumItems<ItemType>();
                    var itemStatus = CommonMethod.GetEnumItems<ItemStatus>();
                    var userName = new List<string>();
                    var workId = new List<int>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var type = dt.Rows[i][6].ToString();
                        var status = dt.Rows[i][7].ToString();
                        KeyValuePair<string, string>?  typeTxt = itemType.FirstOrDefault(a => a.Value == type);
                        KeyValuePair<string, string>? statusTxt = itemStatus.FirstOrDefault(a => a.Value == status);
                        if (!typeTxt.HasValue 
                            || string.IsNullOrWhiteSpace(typeTxt.Value.Key)
                            || !statusTxt.HasValue
                            || string.IsNullOrWhiteSpace(statusTxt.Value.Key))
                        { continue; }
                        DataRow dr = newDt.NewRow();
                        dr["CompanyId"] = user.CompanyId.ToString();
                        dr["CompanyName"] = user.CompanyName.ToString();
                        dr["ProjectId"] = "";
                        dr["WorkId"] = dt.Rows[i][0];
                        dr["Name"] = dt.Rows[i][1];
                        dr["Infos"] = dt.Rows[i][2];
                        dr["UserName"] = dt.Rows[i][3].ToString() + ",";
                        dr["Level"] = dt.Rows[i][4].ToString();
                        dr["DayCount"] = dt.Rows[i][5].ToString();
                        dr["StartDate"] =DateTime.Now;
                        dr["EndDate"] = DateTime.Now.AddDays(CommonMethod.GetInt( dr["DayCount"]));

                        dr["Type"] = typeTxt.Value.Key;
                        dr["Status"] = statusTxt.Value.Key;


                        dr["IsDeleted"] = "0";
                        dr["CreateDate"] = DateTime.Now.ToString();
                        dr["CreateBy"] = user.UserName;
                        newDt.Rows.Add(dr);
                        workId.Add(CommonMethod.GetInt(dt.Rows[i][0]));
                        userName.AddRange(dr["UserName"].ToString().Split(','));
                    }
                    #endregion

                    var companyList = CompanyUserDBOperate.GetUserByNames(userName.ToArray());
                    var workList = WorkListDBOperate.GetListByIds(workId.ToArray());

                    for (int i = 0; i < newDt.Rows.Count; i++)
                    {
                        var usrName = newDt.Rows[i]["UserName"].ToString();
                        var usrs = usrName.Split(',');
                        var ids = string.Empty;
                        foreach (var item in usrs)
                        {
                            if (string.IsNullOrWhiteSpace(item)) { continue; }
                            ids += companyList.FirstOrDefault(a => a.Name == item).UserName + ",";
                        }
                        var wId = CommonMethod.GetInt(newDt.Rows[i]["WorkId"]);
                        newDt.Rows[i]["UserId"] = ids;
                        newDt.Rows[i]["ProjectId"] = workList.FirstOrDefault(a => a.Id == wId).ProjectId;
                    }

                    //批量写入
                    if (newDt.Rows.Count != 0 && newDt != null)
                    {
                        sqlBC.WriteToServer(newDt);
                        newDt.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                LogBussiness.AddLog("WorkItemsImport", "", ex.Message, LogLevel.Error);
                return false;
            }
            return true;
        }

        public static bool ImportExcel(DataTable dt, CompanyUser user, out string message) {
            message = string.Empty;
            try
            {
                var workName = dt.Columns[1].ColumnName.Trim();
                var work = WorkListDBOperate.GetModelByName(workName);
                if (work == null) { message = "工单不存在,请先导入或创建工单"; return false; }
                var addList = new List<WorkListItems>();
                var detailName = new List<string>();
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var detail = dr[1].ToString();
                    if (!detailName.Contains(detail)) detailName.Add(detail);
                    var col = dr.ItemArray;
                    //从第二列开始算
                    for (int j = 2; j < col.Length; j++)
                    {
                        var info = col[j].ToString();
                        if (string.IsNullOrEmpty(info)) { continue; }
                        var model = new WorkListItems()
                        {
                            CompanyId = work.CompanyId,
                            CompanyName = work.CompanyName,
                            ProjectId = work.ProjectId,
                            WorkId = work.Id,
                            Name = detail,  
                            Level = 999,
                            Status = ItemStatus.XuQiu.ToString(),
                            DayCount = 0,
                            CreateBy = user.UserName,
                            CreateDate = DateTime.Now,
                            IsDeleted = 0,
                            Infos = info,
                        };
                        addList.Add(model);
                    }
                }
                var detailList = WorkListDetailDBOperate.GetModelByNames(detailName.ToArray(), work.Guid);
                var flag = true;
                var notExistList = new List<string>();
                addList.ForEach(a => {
                    var myDetail = detailList.FirstOrDefault(b => b.Infos == a.Name);
                    if (myDetail == null) { flag = false; notExistList.Add(a.Name); return; }
                    a.DetailId = myDetail.Id;
                    a.DetailGuid = myDetail.Guid;
                });
                if (!flag) {
                    message = string.Format("[{0}]工单内容不存在,无法导入", string.Join(",", notExistList.Distinct()));
                    return false;
                }
                return WorkListItemsDBOperate.AddWorkListItems(addList);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }

        public static bool WorkListInfoEdit(List<WorkListEdit> list,WorkList work) {
            var operate = CookieOperate.MemberCookie;
            var workDetail = new List<WorkListDetail>();
            var workItem = new List<WorkListItems>();
            var index = 1;

            foreach (var item in list)
            {
                var workDetailModel = new WorkListDetail()
                {
                    WorkId = work.Guid,
                    Index = index++,
                    Infos = item.Name,
                    RoleCode = item.Role,
                    IsDeleted = 0,
                    CreateBy = operate.UserName,
                    Guid = Guid.NewGuid().ToString()
                };
                workDetail.Add(workDetailModel);
                var infos = item.Infos.Split('\n');
                foreach (var item1 in infos)
                {
                    var workItemModel = new WorkListItems()
                    {
                        CompanyId = operate.CompanyId,
                        CompanyName = operate.CompanyName,
                        ProjectId = work.ProjectId,
                        WorkId = work.Id,
                        Name = item.Name,
                        Level = 999,
                        Status = ItemStatus.HasPei.ToString(),
                        UserId = string.Format("{0},", operate.UserName),
                        UserName = string.Format("{0},", operate.Name),
                        DayCount = 0,
                        Infos = item1,
                        DetailGuid = workDetailModel.Guid,
                        CreateBy = operate.UserName,
                        Type = ItemType.HouDuan.ToString()
                    };
                    workItem.Add(workItemModel);
                }
            }
            
            return WorkListItemsDBOperate.AddWorkListItems(workDetail, workItem);
        }
    }
}

