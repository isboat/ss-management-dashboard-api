﻿namespace Management.Dashboard.Models
{
    public class UserModel : IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public UserRoles? Role { get; set; }

        public DateTime Created { get; set; }
    }

    public enum UserRoles
    {
        //[Display(Name = "Editor")]
        Editor,

        //[Display(Name = "Admin")]
        Admin
    }
}
