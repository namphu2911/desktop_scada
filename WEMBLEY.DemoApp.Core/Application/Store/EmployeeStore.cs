using System.Collections.ObjectModel;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Employees;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class EmployeeStore
    {
        public List<EmployeeDto> Employees { get; private set; }
        public ObservableCollection<string> EmployeeIds { get; private set; }
        public ObservableCollection<string> EmployeeNames { get; private set; }
        public EmployeeStore()
        {
            Employees = new List<EmployeeDto>();
            EmployeeIds = new ObservableCollection<string>();
            EmployeeNames = new ObservableCollection<string>();
        }

        public void SetEmployee(IEnumerable<EmployeeDto> employees)
        {
            Employees = employees.ToList();
            EmployeeIds = new ObservableCollection<string>(Employees.Select(i => i.EmployeeId).OrderBy(s => s));
            EmployeeNames = new ObservableCollection<string>(Employees.Select(i => i.EmployeeName).OrderBy(s => s));
        }
    }
}
