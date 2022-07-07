namespace KjellDemo.Core.Domain
{
    public class DeskBookingResult : DeskBookingBase
    {
        public int? DeskBookingId;

        public DeskBookingResultCode Code { get; set; }
    }
}