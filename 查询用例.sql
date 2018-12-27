use Company
--获得我发起的
select a.Remark,a.Money,a.Status,a.NextOperateId,b.Name NextOperateName,a.CreateDate,a.ConfigId ,c.Value+''+d.Value ConfigName
from AuditMain(nolock) a
left join CompanyUser(nolock) b
on a.NextOperateId = b.Id
left join FinanceConfig(nolock) c 
on a.ConfigId = c.Id
left join FinanceConfig(nolock)d
on c.PId=d.Id
where a.UserId=10000
Order By a.Id desc

--根据用户部门来查询财务类别
select a.*,b.Value,b.PId from FinanceConfigPermission(nolock) a
left join FinanceConfig(nolock) b
on a.FinanceConfigId = b.Id
where a.DepartId=7

--通过MainId 来查询进度
select *from AuditMain(nolock)

select [Id],[UserId],[Step],[Remark],OperateBy,Status,CreateDate,ModifyDate 
from AuditDetail(nolock) 
where AuditMainId='149f6643387a43889972a4efd36f3006'

select b.Id,c.Name+'-'+ b.Name DepartName from CompanyDepartMent(nolock) b
left join CompanyDepartMent(nolock) c
on b.PId=c.Id
where b.Id=3