namespace khiemnguyen.WebApi.Models
{
    public class ResetPassowrdDTO
    {
        public int Userid { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
