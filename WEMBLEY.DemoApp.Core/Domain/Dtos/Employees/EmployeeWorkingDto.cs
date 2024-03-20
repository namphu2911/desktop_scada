namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Employees
{
    public class EmployeeWorkingDto
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public EmployeeWorkingDto(string employeeId, string employeeName)
        {
            EmployeeId = employeeId;
            EmployeeName = employeeName;
        }
    }
}
