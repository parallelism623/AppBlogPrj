using AutoMapper;
using Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.RequestData
{
    public class CreateAccountRequest
    {
        public required string UserName { get; set; }   
        public required string Password { get; set; }
        public required string Email { get; set; }
        [Compare("Password", ErrorMessage = "ConfirmPassword does not match with password")]
        public required string ConfirmPassword { get;set; }
        public required string FirstName { get; set; }   
        public required string LastName { get; set;}

        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile() 
            {
                CreateMap<CreateAccountRequest, AppUser>();
            }
        }
    }
}
