namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Employees
{
    public class EmployeeDto
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<WorkRecord> WorkRecords { get; set; }
        public EmployeeDto(string employeeId, string employeeName, List<WorkRecord> workRecords)
        {
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            WorkRecords = workRecords;
        }
    }
}
