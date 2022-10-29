namespace MinuteTaker
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNbr { get; set; }

        public string FullName()
        {
            return FirstName.Trim() + " " + LastName.Trim();
        }

        public void CleanPerson()
        {
            FirstName = FirstName.Trim();
            LastName = LastName.Trim();
            EmailAddress = EmailAddress.Trim();
            PhoneNbr = PhoneNbr.Trim();
        }
    }
}
