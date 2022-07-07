using KjellDemo.Core.Domain;

namespace KjellDemo.Core.DataInterface
{
    public interface IDeskBookingRepository
    {
        void Save(DeskBooking deskBooking);
    }
}
