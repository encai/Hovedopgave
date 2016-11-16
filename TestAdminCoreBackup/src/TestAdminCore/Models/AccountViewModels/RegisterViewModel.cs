﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestAdminCore.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "FirstName")]

        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
   
        
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Address")]


        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone")]


        public string Phone { get; set; }

        [Required]
        [Display(Name = "Zipcode")]


        public string Zipcode { get; set; }

        [Required]
        [Display(Name = "MyProperty")]


        public string Myproperty { get; set; }

        [Required]
        [Display(Name = "City")]


        public string City { get; set; }
    }
}
