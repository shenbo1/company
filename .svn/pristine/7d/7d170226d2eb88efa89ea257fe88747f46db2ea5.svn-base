using Company.DAL.Common;
using Company.Dto;
using Company.Dto.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data
{
    public class FinanceConfigDBOperate
    {
        const string TableName = "FinanceConfig";

        #region 添加
        public static bool AddConfig(FinanceConfig model)
        {

            string sql = string.Format("insert into {0}(KeyWord,Value,Remark,Type,CreateDate,CompanyId,IsDeleted,[PID],[Index])  values(@KeyWord,@Value,@Remark,@Type,getdate(),@CompanyId,0,@PID,@Index)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyConfig(FinanceConfig model)
        {
            string sql = string.Format(@"update {0} set [KeyWord]=@KeyWord,[Value]=@Value,[Remark]=@Remark,[Type]=@Type,[CompanyId]=@CompanyId,[IsDeleted]=@IsDeleted,[PId]=@PId,[Index]=@Index
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static FinanceConfig GetModelById(int Id)
        {
            string sql = string.Format(@"select Id,KeyWord,Value,Remark,Type,CreateDate,CompanyId,IsDeleted,[PID],[Index] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<FinanceConfig>(sql, Id);
        }
        #endregion

        #region 获取所有数据
        public static List<FinanceConfig> GetList(string type, int companyId)
        {
            string sql = string.Format(@"select Id,KeyWord,Value,[PID],[Index]  from {0} (nolock) where IsDeleted=0 and Type=@Type and CompanyId=@CompanyId and [IsDeleted]=0 order by [Index]", TableName);
            return DBAccess.GetEntityList<FinanceConfig>(sql, new { Type = type, CompanyId = companyId });
        }
        public static List<FinanceConfig> GetListChild(string type, int companyId) 
        {
            var list = GetList(type, companyId);
            var resList = list.Where(a => a.PId == 0).ToList();
            resList.ConvertAll(
                a => a.ChildList = list.Where(b => b.PId == a.Id).ToList()
            );
            return resList;
        }
        #endregion

        #region 获取分页列表
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagesize">每页显示多少行</param>
        /// <param name="offset">跳过行数</param>
        /// <param name="key">关键词</param>
        /// <param name="totalcount">输出行数</param>
        /// <param name="GroupId">输出行数</param>
        /// <returns></returns>
        public static List<FinanceConfig> GetPagerList(int pagesize, int offset, string name, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = offset, PageSize = pagesize, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[KeyWord],A.[Value],A.[Remark],A.[Type],A.[CreateDate],A.[CompanyId],A.[IsDeleted],a.[PID],A.[Index]";
            if (!string.IsNullOrEmpty(name))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + name + "%");
            }
            var list = PagerDBOperate<FinanceConfig>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        #region  根据部门编号获取配置
        public static List<FinanceConfig> GetConfigByDepartId(int departId)
        {
            string sql = @"  select b.id,b.value,b.pid from FinanceConfigPermission(nolock) a
  left join FinanceConfig(nolock) b
  on a.[FinanceConfigId] = b.id
  where a.DepartId=@DepartId";
            return DBAccess.GetEntityList<FinanceConfig>(sql, new { DepartId = departId });
        }
        #endregion

        #region  根据部门编号获取配置
        /// <summary>
        /// 根据部门编号获取配置
        /// </summary>
        /// <param name="departId">部门编号</param>
        /// <returns></returns>
        public static List<PickerSelect> GetConfigDataByDepartId(int departId) 
        {
            var pickerSelect = new List<PickerSelect>();
            var list = GetConfigByDepartId(departId);
            var resList = list.Where(a => a.PId == 0).ToList();
            resList.ConvertAll(
                a => a.ChildList = list.Where(b => b.PId == a.Id).ToList()
            );
            resList = resList.Where(a => a.ChildList.Count() > 0).ToList();
            pickerSelect = resList.ConvertAll(a => new PickerSelect()
            {
                id = a.Id,
                text = a.Value,
                list = a.ChildList.ConvertAll(b => new PickerSelect() { id = b.Id, text = b.Value })
            });
            return pickerSelect;

        }
        #endregion
    }
}