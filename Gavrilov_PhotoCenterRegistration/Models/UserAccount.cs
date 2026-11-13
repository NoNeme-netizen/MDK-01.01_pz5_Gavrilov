namespace Gavrilov_PhotoCenterRegistration.Models
{
    internal class UserAccount
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
    }
}