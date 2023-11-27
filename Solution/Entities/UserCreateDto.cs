﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Login should be provided.")]
        [MaxLength(50)]
        public string Login { get; set; }
        
        public string Password { get; set; }
        [Required(ErrorMessage = "First Name should be provided.")]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name should be provided.")]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email should be provided.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number should be provided.")]
        public string PhoneNumber { get; set; }
        public List<CardForUserCreationDto> Cards { get; set; }
    }
}
