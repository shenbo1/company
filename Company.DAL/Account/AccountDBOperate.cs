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

namespace Company.DAL
{
    public class AccountDBOperate
    {
        const string TableName = "Account";
        const string CompanyTableName = "CustomerCompany";
        const string MemberTableName = "CustomerMember";
        const string ProjectTableName = "ProjectManage";
        const string PersonSettingTableName = "PersonSetting";
        const string HardWareTableName = "HardWare";


        #region 添加
        public static bool AddAccount(CustomerCompany company, CustomerMember member,Account account)
        {
            string companySql = string.Format("insert into {0}([CompanyId],[CompanyName],[Name],[City],[Address],[Status],[Infos],[IsDeleted],[CreateDate],[CreateBy])  " +
                "values(@CompanyId,@CompanyName,@Name,@City,@Address,@Status,@Infos,0,getdate(),@CreateBy);SELECT @@IDENTITY ",
                CompanyTableName);
            string memberSql = string.Format("insert into {0}([CompanyId],[CompanyName],[CusCompanyId],[Name],[ImgUrl],[Mobile],[Email],[Infos],[IsDeleted],[CreateDate],[CreateBy]) " +
                " values(@CompanyId,@CompanyName,@CusCompanyId,@Name,@ImgUrl,@Mobile,@Email,@Infos,0,getdate(),@CreateBy);SELECT @@IDENTITY", MemberTableName);
            string sql = string.Format("insert into {0}([Mobile],[Email],[Password],[IsDeleted],[CreateDate],[CreateBy],[Role],[AccountName],[AccountId])  values" +
                "(@Mobile,@Email,@Password,0,getdate(),@CreateBy,@Role,@AccountName,@AccountId)", TableName);
            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                con.Open();

                var companyId = con.Query<int>(companySql, company).FirstOrDefault();
                member.CusCompanyId = companyId;
                var memberId = con.Query<int>(memberSql, member).FirstOrDefault();
                account.AccountId = memberId;
                return con.Execute(sql, account) > 0;
            }
            return true;
            //string sql = string.Format("insert into {0}([Mobile],[Email],[Password],[IsDeleted],[CreateDate],[CreateBy],[Role],[AccountName])  values(@Mobile,@Email,@Password,0,getdate(),@CreateBy,@Role,@AccountName)", TableName);
            //return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        public static bool AddAccount(CustomerCompany company, CustomerMember member, Account account,ProjectManage projectManage,HardWare hardWare,List<PersonSetting> personSettings)
        {
            string companySql = string.Format("insert into {0}([CompanyId],[CompanyName],[Name],[City],[Address],[Status],[Infos],[IsDeleted],[CreateDate],[CreateBy],[TeamDesc],[CompanyStatus],[PartInfo])  " +
                "values(@CompanyId,@CompanyName,@Name,@City,@Address,@Status,@Infos,0,getdate(),@CreateBy,@TeamDesc,@CompanyStatus,@PartInfo);SELECT @@IDENTITY ",
                CompanyTableName);
            string memberSql = string.Format("insert into {0}([CompanyId],[CompanyName],[CusCompanyId],[Name],[ImgUrl],[Mobile],[Email],[Infos],[IsDeleted],[CreateDate],[CreateBy]) " +
                " values(@CompanyId,@CompanyName,@CusCompanyId,@Name,@ImgUrl,@Mobile,@Email,@Infos,0,getdate(),@CreateBy);SELECT @@IDENTITY", MemberTableName);
            string sql = string.Format("insert into {0}([Mobile],[Email],[Password],[IsDeleted],[CreateDate],[CreateBy],[Role],[AccountName],[AccountId])  values" +
                "(@Mobile,@Email,@Password,0,getdate(),@CreateBy,@Role,@AccountName,@AccountId);SELECT @@IDENTITY", TableName);
            string Projectsql = string.Format("insert into {0}([Name],[FullName],[PayWay],[Money],[Status],[Infos],[CompanyId],[CreateDate],[CreateBy],[IsDeleted]," +
                "[CusMemberId],[CusCompanyId],[Source],[OpereateType],[DevType],[NowStatus],[DevLanguate],[MarketProspect],[Month])" +
                "  values(@Name,@FullName,@PayWay,@Money,@Status,@Infos,@CompanyId,getdate(),@CreateBy,0,@CusMemberId,@CusCompanyId,@Source,@OpereateType,@DevType,@NowStatus,@DevLanguate,@MarketProspect,@Month);SELECT @@IDENTITY", ProjectTableName);
            string HardWareSql = string.Format("insert into {0}([IsDeleted],[CreateDate],[CreateBy],[WorkGuid],[Infos],[ProjectId])  values(0,getdate(),@CreateBy,@WorkGuid,@Infos,@ProjectId)", HardWareTableName);
            string PersonSettingSql = string.Format("insert into {0}([IsDeleted],[CreateDate],[CreateBy],[WorkGuid],[Price],[DepartId],[DepartName],[Infos],[Month],[TotalPrice],[Discount],[ProjectId])  values(0,getdate(),@CreateBy,@WorkGuid,@Price,@DepartId,@DepartName,@Infos,@Month,@TotalPrice,@Discount,@ProjectId)", PersonSettingTableName);

            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                con.Open();

                var companyId = con.Query<int>(companySql, company).FirstOrDefault();
                member.CusCompanyId = companyId;
                var memberId = con.Query<int>(memberSql, member).FirstOrDefault();
                account.AccountId = memberId;
                var accountId = con.Query<int>(sql, account).FirstOrDefault();
                projectManage.CusCompanyId = companyId;
                projectManage.CusMemberId = memberId;
                projectManage.CreateBy = account.AccountName;
                if (personSettings != null && personSettings.Count() > 0) { projectManage.Money = personSettings.Sum(a => a.TotalPrice); }
                var projectId = con.Query<int>(Projectsql, projectManage).FirstOrDefault();
                hardWare.ProjectId = projectId;
                personSettings.ForEach(a => { a.ProjectId = projectId; });
                if (personSettings != null && personSettings.Count() > 0) { con.Execute(PersonSettingSql, personSettings);  }
                return con.Execute(HardWareSql, hardWare) > 0;
            }
            return true;
            //string sql = string.Format("insert into {0}([Mobile],[Email],[Password],[IsDeleted],[CreateDate],[CreateBy],[Role],[AccountName])  values(@Mobile,@Email,@Password,0,getdate(),@CreateBy,@Role,@AccountName)", TableName);
            //return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        public static bool GetAccountByMobile(string mobile, int role) {
            string sql = "select COUNT(0) from Account (nolock) where IsDeleted=0 and Mobile=@mobile and Role=@role;";
            return DBAccess.ExecuteSql<int>(sql, new { mobile = mobile, role = role }) > 0;
        }
        public static bool GetAccountByEmail(string email, int role)
        {
            string sql = "select COUNT(0) from Account (nolock) where IsDeleted=0 and Email=@email and Role=@role;";
            return DBAccess.ExecuteSql<int>(sql, new { email = email, role = role }) > 0;
        }

        public static Account LoginByMobileOrEmail(string name)
        {
            string sql = "select * from Account (nolock) where IsDeleted=0 and ( Email=@name or Mobile=@name);";
            return DBAccess.GetEntityFirstOrDefault<Account>(sql, new { name  = name});
        }

        #region 修改
        public static bool ModifyAccount(Account model)
        {
            string sql = string.Format(@"update {0} set [Mobile]=@Mobile,[Email]=@Email,[AccountName]=@AccountName
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static Account GetModelById(int Id)
        {
            string sql = string.Format(@"select [Mobile],[Email],[Password],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[Role],[Id],[AccountName] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<Account>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static Account GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Mobile],[Email],[Password],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[Role],[Id],[AccountName],[Mobile],[Email],[Password],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[Role],[Id],[AccountName] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<Account>(sql, Name);
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
        public static List<Account> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Mobile],A.[Email],A.[Password],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy],A.[Role],A.[Id],A.[AccountName]";
            pager.WhereStr += " and A.[IsDeleted]=0 and a.CompanyId=@CompanyId";
            param.Add("CompanyId", query.CompanyId.ToString());
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<Account>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion
    }
}