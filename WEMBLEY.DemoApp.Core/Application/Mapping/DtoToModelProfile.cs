using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Employees;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ShiftReports;

namespace WEMBLEY.DemoApp.Core.Application.Mapping
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<ParameterDto, LineInitialSettingEntry>();
            CreateMap<ShiftReportDto, ShiftReportEntry>();
            CreateMap<EmployeeDto, PersonViewModel>();
        }
    }
}
