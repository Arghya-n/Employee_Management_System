﻿using System.ComponentModel.DataAnnotations;

namespace EmpTaskAPI.DTOModels
{
    public class UserLogin
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
