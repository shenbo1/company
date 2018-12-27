using Company.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Util;
using Company.DAL.Data;

namespace Company.BLL.Data
{
    public class AuditBussiness
    {
        #region 添加审核
        /// <summary>
        /// 添加审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddAudit(AuditMain model,out string msg ) 
        {
            msg = "";
            try
            {
                var auditStepList = AuditStepDBOperate.GetAuditStepByDepartId(model.DepartId);//获取当前人部门的审核步骤
                if (auditStepList.Count() == 0) { msg = "StepNull"; return false; }
                var auditDetail = auditStepList.ConvertAll(a => new AuditDetail()
                {
                    AuditMainId = model.MainId,
                    OperateBy = a.UserId,
                    Status = (int)AuditStatus.NotCheck,
                    Step = a.Step,
                    UserId = model.UserId,
                    CompanyId = model.CompanyId
                }).ToList();//插入详情

                auditDetail[0].Status = (int)AuditStatus.NextCheck;
                model.Status = (int)AuditStatus.Pass;
                model.NowCount = 0;
                model.TotalCount = auditDetail.Count();
                model.NextOperateId = auditDetail[0].OperateBy;

                #region 默认插入发起审核记录
                auditDetail.Insert(0, new AuditDetail() {
                    AuditMainId = model.MainId,
                    Step = 0,
                    ModifyDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CompanyId = model.CompanyId,
                    Status = (int)AuditStatus.Pass,
                    UserId = model.UserId,
                    OperateBy = model.UserId,
                    Remark = "发起审核"
                });
                #endregion
                model.DetailList = auditDetail;

                var flag = AuditMainDBOperate.AuditMainAdd(model);
                return flag;
            }
            catch (Exception ex)
            {
                LogBussiness.AddLog("AddAudit", CommonMethod.ToJson(model), ex.Message, Util.LogLevel.Error);
            }
            return false;
        }
        #endregion

        #region 根据编号获取
        public static AuditMain GetAuditMainById(int id,out string message) 
        {
            message = "";
            AuditMain main = new AuditMain();
            try
            {
                main = AuditMainDBOperate.GetAuditMainById(id);
                if (main == null) { message = "MainNull"; return null; }
                var list = AuditMainDBOperate.GetAuditDetailList(main.MainId);
                var memeberList = CompanyUserDBOperate.GetUserByIds(list.Select(a => a.OperateBy).ToArray());
                list.ForEach(a => {
                    var model = memeberList.FirstOrDefault(b => b.Id == a.OperateBy);
                    if (model == null) { return; }
                    a.OperateName = model.Name;
                    a.OperateHeadUrl = model.WeChatHeadUrl;
                });
                main.DetailList = list;
            }
            catch (Exception ex)
            {
                LogBussiness.AddLog("GetAuditMainById", id + "", ex.Message, Util.LogLevel.Error);
            }
            return main;
        }
        #endregion
    }
}
