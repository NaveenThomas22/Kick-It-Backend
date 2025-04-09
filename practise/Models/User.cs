using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;

namespace practise.Models
{
    public class User
    {
        public Guid UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User"; 

        public bool IsBlocked { get; set; } = false;

        public List<Order> Orders { get; set; } = new List<Order>();


        //Navigation propertys
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }

    }
}