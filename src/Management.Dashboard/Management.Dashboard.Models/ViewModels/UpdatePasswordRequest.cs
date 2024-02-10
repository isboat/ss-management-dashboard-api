namespace Management.Dashboard.Models.ViewModels
{
    public class UpdatePasswordRequest
    {
        public string? CurrentPasswd { get; set; }

        public string? NewPassword { get; set; }
    }
}
