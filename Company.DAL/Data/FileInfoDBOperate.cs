using Company.DAL.Common;
using Company.Dto;
using Company.Dto.Model;
using Company.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data
{
    public class FileInfoDBOperate
    {
        const string TableName = "FileInfo";

        #region 添加
        public static bool AddFileInfo(FileInfo model)
        {
            string sql = string.Format("insert into {0}([Guid],[IsDeleted],[CreateDate],[CreateBy],[FileName],[FilePath],[TypeCode],[TypeId],[FileSize])  values(@Guid,0,getdate(),@CreateBy,@FileName,@FilePath,@TypeCode,@TypeId,@FileSize)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyFileInfo(FileInfo model)
        {
            string sql = string.Format(@"update {0} set [Guid]=@Guid,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,[FileName]=@FileName,[FilePath]=@FilePath,[TypeCode]=@TypeCode,[TypeId]=@TypeId,[FileSize]=@FileSize
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static FileInfo GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[FileName],[FilePath],[TypeCode],[TypeId],[FileSize] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<FileInfo>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static FileInfo GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[FileName],[FilePath],[TypeCode],[TypeId],[FileSize],[Id],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[FileName],[FilePath],[TypeCode],[TypeId],[FileSize] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<FileInfo>(sql, Name);
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
        public static List<FileInfo> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[Guid],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy],A.[FileName],A.[FilePath],A.[TypeCode],A.[TypeId],A.[FileSize]";
            pager.WhereStr += " and A.[IsDeleted]=0 and a.CompanyId=@CompanyId";
            param.Add("CompanyId", query.CompanyId.ToString());
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<FileInfo>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        public static List<FileInfo> GetList(string guid) {
            string sql = string.Format("select * from {0} where TypeId = @Guid and IsDeleted = 0", TableName);
            return DBAccess.GetEntityList<FileInfo>(sql, new { Guid = guid });
        }

        public static bool SetFile(string Guid, string Type, string data)
        {
            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                con.Open();
                var transaction = con.BeginTransaction();
                var dataArray = data.Split(',');
                try
                {
                    string sql = string.Format("update {0} set IsDeleted = 1 where TypeId=@TypeId ;", TableName);
                    con.Execute(sql, new { TypeId = Guid }, transaction);
                    if (dataArray.Length > 0)
                    {
                        string sql2 = string.Format("update {0} set  IsDeleted = 0,TypeCode = @TypeCode,TypeId=@TypeId where Guid in @Guid;", TableName);
                        con.Execute(sql2, new { TypeId = Guid, TypeCode = Type,Guid = dataArray.ToArray() }, transaction);
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return false;
        }
    }
}