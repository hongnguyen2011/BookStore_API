namespace BookStore_API.Models.Dto
{
    public class ChangePasswordRequestDTO
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}