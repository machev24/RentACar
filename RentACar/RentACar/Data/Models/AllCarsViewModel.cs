namespace RentACar.Data.Models
{
    public class AllCarsViewModel
    {
        public IEnumerable<CarListingViewModel> Cars { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }
    }
}
