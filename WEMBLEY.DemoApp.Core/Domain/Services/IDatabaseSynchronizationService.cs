namespace WEMBLEY.DemoApp.Core.Domain.Services
{
    public interface IDatabaseSynchronizationService
    {
        Task SynchronizeReferencesData();
        Task SynchronizeStationsData();
        Task SynchronizeHomeData();
        Task SynchronizeEmployeesData();
    }
}
