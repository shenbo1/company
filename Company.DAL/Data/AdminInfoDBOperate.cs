using Company.Dto;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Company.Dto.Model;
using Company.DAL.Common;

namespace Company.DAL.Data
{
    public class AdminInfoDBOperate
    {
        const string TableName = "AdminInfo";

        #region 登录
        public static AdminInfo Login(string loginId, string password) 
        {
            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                string sql = @"SELECT *
               FROM [AdminInfo] Where UserName=@UserName and PassWord=@PassWord";
                var model = con.Query<AdminInfo>(sql, new { UserName = loginId, PassWord = password }).FirstOrDefault();
                return model;
            }
        }
        #endregion

        #region 用户名是否重复
        public static bool CheckUserName(string UserName) 
        {
            string sql = string.Format(@"select Count(0) from {0} (nolock) where UserName=@UserName", TableName);
            return DBAccess.ExecuteSql<int>(sql, new { UserName = UserName }) > 0;
        }
        #endregion

        #region 添加
        public static bool AddAdminInfo(AdminInfo model)
        {
            model.Ip = CommonMethod.GetClientIP;
            string sql = string.Format("insert into {0}(UserName,PassWord,Name,CreateDate,IsDeleted,Ip,CompanyId,Mobile,Canceer,RoleId)  values(@UserName,@PassWord,@Name,getdate(),0,@Ip,@CompanyId,@Mobile,@Canceer,@RoleId)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyAdminInfo(AdminInfo model)
        {
            string sql = string.Format(@"update {0} set [UserName]=@UserName,[Name]=@Name,[CompanyId]=@CompanyId,[Mobile]=@Mobile,[Canceer]=@Canceer,[RoleId]=@RoleId
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static AdminInfo GetModelById(int Id)
        {
            string sql = string.Format(@"select Id,UserName,PassWord,Name,CreateDate,IsDeleted,Ip,CompanyId,Mobile,Canceer,RoleId from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<AdminInfo>(sql, Id);
        }
        #endregion

        #region 获取所有数据
        public static List<AdminInfo> GetList()
        {
            string sql = string.Format(@"select Id,UserName,PassWord,Name,CreateDate,IsDeleted,Ip,CompanyId,Mobile,Canceer,RoleId from {0} (nolock)", TableName);
            return DBAccess.GetEntityList<AdminInfo>(sql, new { });
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
        public static List<AdminInfo> GetPagerList(int pagesize, int offset, string name, out int totalcount,int companyId)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = offset, PageSize = pagesize, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[UserName],A.[Name],A.[CreateDate],A.[IsDeleted],A.[CompanyId],A.[Mobile],A.[Canceer],A.[RoleId],B.[RoleName],c.[CompanyName]+'-'+c.[CompanyFullName] CompanyName";
            pager.JoinSql += " left join [RoleInfo] B on A.RoleId = B.Id";
            pager.JoinSql += " left join [CompanyInfo] C on A.CompanyId = C.Id";
            pager.Direction = Direction.ASC;
            if (!string.IsNullOrEmpty(name))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + name + "%");
            }
            if (companyId != 0) {
                pager.WhereStr += " and A.[CompanyId]=@CompanyId";
                param.Add("CompanyId", companyId);
            }
            var list = PagerDBOperate<AdminInfo>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion
        
    }
}
