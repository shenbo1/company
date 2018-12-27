using Company.DAL.Data;
using Company.Dto;
using Company.Dto.Data;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Data
{
    public class FinanceReportBLL
    {
        #region 将Excel中的格式转换为数据库的格式
        public static DataTable ConventImportDt(DataTable dt, CompanyUser admin)
        {
            DataTable newDt = new DataTable();

            #region 反射获得列名
            var objType = typeof(FinanceReport);
            PropertyInfo[] properties = objType.GetProperties();
            string[] outColName = new string[] { "Id", "ModifyDate" };
            foreach (var item in properties)
            {
                var name = item.Name;
                if (outColName.Contains(name)) { continue; }
                newDt.Columns.Add(new DataColumn() { ColumnName = item.Name });
            }
            #endregion

            //财务类别
            var list = FinanceConfigDBOperate.GetList("Report", admin.CompanyId);

            #region  数据处理
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = newDt.NewRow();
                string Object = dt.Rows[i][1].ToString();//科目
                var objectName = list.FirstOrDefault(a => a.Value == Object);
                if (objectName == null) { continue; }
                dr[0] = objectName.Id;//报表编号类型
                dr[1] = Object;//报表类型名称
                dr[2] = dt.Rows[i][2].ToString();//科目
                decimal money = CommonMethod.GetDecimal(dt.Rows[i][3]);//支出
                int type = 2;//1收入 2支出
                if (money == 0)
                {//收入
                    type = 1;
                    money = CommonMethod.GetDecimal(dt.Rows[i][4]);
                }
                dr[3] = money.ToString();
                dr[4] = type.ToString();
                dr[5] = dt.Rows[i][7];//备注
                dr[6] = admin.UserName;
                dr[7] = CommonMethod.GetDateTime(dt.Rows[i][0]);
                dr[8] = dt.Rows[i][5];//
                dr[9] = dt.Rows[i][6];
                dr[10] = admin.CompanyId;
                dr[11] = DateTime.Now.ToString();
                dr[12] = "0";
                newDt.Rows.Add(dr);
            }
            #endregion

            return newDt;
        }
        #endregion

        #region 导入报表
        /// <summary>
        /// 导入报表
        /// </summary>
        /// <param name="dt">用户所传Excel</param>
        /// <param name="admin">用户</param>
        /// <returns></returns>
        public static bool ImportExcel(DataTable dt, CompanyUser admin)
        {
            var flag = false;
            dt = ConventImportDt(dt, admin);
            flag = FinanceReportDBOperate.ImportExcel(dt);
            return flag;
        }
        #endregion

        #region 报表导出数据
        public static List<ExportFinanceModel> GetExportList(Dto.Model.QueryBase model) 
        {
            var list = FinanceReportDBOperate.GetList(model);
            var ExPortList = new List<ExportFinanceModel>();
            ExPortList = list.ConvertAll(a => ConventToExPortFinance(a));
            return ExPortList;
        }
        private static ExportFinanceModel ConventToExPortFinance(FinanceReport model) {
            ExportFinanceModel newModel = new ExportFinanceModel() 
            {
                Abstract = model.Abstract,
                Operater = model.Operater,
                Remark = model.Remark,
                ReportTypeName = model.ReportTypeName,
                ShouRu = model.Type==1 ? model.Blance.ToString() : "0",
                ZhiChu = model.Type==1 ? "0" : model.Blance.ToString(),
                TimeFormat = model.DateTime.ToShortDateString()
            };
            return newModel;
        }
        #endregion

    }
}
