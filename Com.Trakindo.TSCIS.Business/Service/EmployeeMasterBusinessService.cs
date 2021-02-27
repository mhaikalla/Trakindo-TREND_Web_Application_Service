using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class EmployeeMasterBusinessService
    {
        private readonly MasterEmployeeContext _dbMstEmployee = Factory.Create<MasterEmployeeContext>("MasterEmployee", ClassType.clsTypeDataContext);
        public bool CheckEmployee(int id)
        {
            return _dbMstEmployee.EmployeeMaster.Count(e => e.Employee_Id == id) > 0;
        }
        public EmployeeMaster GetDetail(int id)
        {
            EmployeeMaster result = _dbMstEmployee.EmployeeMaster
                .FirstOrDefault(i => i.Employee_Id == id);

            return result;
        }
       
        public EmployeeMaster GetDetailbyUserName(string id)
        {
            EmployeeMaster result = _dbMstEmployee.EmployeeMaster
                .FirstOrDefault(i => i.Employee_xupj.Contains(id));

            return result;
        }
       
    }
}
