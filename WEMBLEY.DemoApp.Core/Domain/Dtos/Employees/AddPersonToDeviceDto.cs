namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Employees
{
    public class AddPersonToDeviceDto
    {
        public List<string> EmployeeIds { get; set; }
        public AddPersonToDeviceDto(List<string> employeeIds)
        {
            EmployeeIds = employeeIds;
        }
    }
}
