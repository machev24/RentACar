namespace RentACar.Data.Models
{
    public class CarRequestViewModel
    {
        public IEnumerable<CarListingViewModel> Cars { get; set; }
        public IEnumerable<RequestListingViewModel> Requests { get; set; }
    }
}
