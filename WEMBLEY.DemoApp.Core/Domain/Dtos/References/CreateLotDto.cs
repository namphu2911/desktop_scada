namespace WEMBLEY.DemoApp.Core.Domain.Dtos.References
{
    public class CreateLotDto
    {
        public string LotCode { get; set; }
        public int LotSize { get; set; }
        public CreateLotDto(string lotCode, int lotSize)
        {
            LotCode = lotCode;
            LotSize = lotSize;
        }
    }
}
