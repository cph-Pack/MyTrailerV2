namespace MyTrailer_Frontend.Data
{
    public class RentalRequest
    {
        public int Trailernumber { get; set; }
        public string Email {  get; set; }
        public bool HasInsurance { get; set; }
        public RentalType Rentaltype { get; set; }

        public RentalRequest(int trailerNumber, string email, bool hasInsurance, RentalType rentaltype)
        {
            this.Trailernumber = trailerNumber;
            this.Email = email;
            this.HasInsurance = hasInsurance;
            this.Rentaltype = rentaltype;

        }
    }
}
