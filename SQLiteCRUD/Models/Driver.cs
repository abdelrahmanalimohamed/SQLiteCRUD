﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQLiteCRUD.Models
{
    [Table("Driver")]
    public class Driver
    {
        [Key]
        public int Id { get;  set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }
}
