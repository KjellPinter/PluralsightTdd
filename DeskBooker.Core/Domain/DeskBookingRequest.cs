using DeskBooker.Core.Processor;

namespace DeskBooker.Core.Domain
{
    public class DeskBookingRequest : DeskBookingBase
    {
        private IDeskBookingRequestProcessor _deskBookingRequestProcessor;

        public DeskBookingRequest()
        {
        }

        public DeskBookingRequest(IDeskBookingRequestProcessor deskBookingRequestProcessor)
        {
            _deskBookingRequestProcessor = deskBookingRequestProcessor;
        }
    }
}