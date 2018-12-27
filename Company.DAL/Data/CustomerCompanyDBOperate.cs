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
    public class CustomerCompanyDBOperate
    {
        const string TableName = "CustomerCompany";

        #region 添加
        public static bool AddCustomerCompany(CustomerCompany model)
        {

            string sql = string.Format("insert into {0}([CompanyId],[CompanyName],[Name],[City],[Address],[Status],[Infos],[IsDeleted],[CreateDate],[CreateBy],[Source])  values(@CompanyId,@CompanyName,@Name,@City,@Address,@Status,@Infos,0,getdate(),@CreateBy,@Source)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyCustomerCompany(CustomerCompany model)
        {
            string sql = string.Format(@"update {0} set [CompanyId]=@CompanyId,[CompanyName]=@CompanyName,[Name]=@Name,[City]=@City,[Address]=@Address,[Status]=@Status,[Infos]=@Infos,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,
[BankName]=@BankName,[BankNo]=@BankNo,[Tax]=@Tax,[ContactName]=@ContactName,[ContactMobile]=@ContactMobile,[License]=@License
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static CustomerCompany GetModelById(int Id)
        {
            string sql = string.Format(@"select * from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<CustomerCompany>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static CustomerCompany GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[CompanyId],[CompanyName],[Name],[City],[Address],[Status],[Infos],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[Id],[CompanyId],[CompanyName],[Name],[City],[Address],[Status],[Infos],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<CustomerCompany>(sql, Name);
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
        public static List<CustomerCompany> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]", Direction = Direction.ASC };
            pager.Columns = @"A.[Id],A.[Name],A.[City],A.[Address],A.[Status],A.[Infos],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy]";
            pager.WhereStr += " and A.[IsDeleted]=0 and a.[CompanyId]=@CompanyId";

            param.Add("CompanyId", query.CompanyId);
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<CustomerCompany>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        public static CustomerCompany GetCompanyByMember(int id) {
            string sql = @"select * from CustomerCompany(nolock) where id = (
select CusCompanyId from CustomerMember (nolock)
where id=@Id
)";
            return DBAccess.GetEntityById<CustomerCompany>(sql, id);
        }
    }
}