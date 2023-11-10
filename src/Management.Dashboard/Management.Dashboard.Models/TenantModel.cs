namespace Management.Dashboard.Models
{
    public class TenantModel
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public string? Telephone { get; set; }

        public string? Country { get; set; }

        public string? Postcode { get; set; }

        public string? Note { get; set; }

        public int TvAppsLimit { get; set; } = 3;

        public DateTime Created { get; set; }
    }
}
