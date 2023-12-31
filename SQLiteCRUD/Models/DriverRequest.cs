﻿using System.ComponentModel.DataAnnotations;

namespace SQLiteCRUD.Models
{
    public class DriverRequest
    {
        [StringLength(50)]
        public string firstName { get; set; }

        [StringLength(50)]
        public string lastName { get; set; }

        [EmailAddress]
        public string email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number. Please enter a 10-digit phone number.")]
        public string phoneNumber { get; set; }
    }

}
