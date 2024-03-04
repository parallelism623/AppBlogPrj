
using AutoMapper;
using Core.Domain.Identity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.RequestData
{
    public class CreateUserRequest
    {
        public required string FirstName { get; set; }  
        public required string LastName { get; set;}
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        
        public required string ConfirmPassword { get; set;}
        public string? PhoneNumber { get; set; }
        public DateTime? Dob { get; set; }
        public string? Avatar { get; set; }
        public bool IsActive { get; set; }
        public double RoyaltyAmountPerPost { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<CreateUserRequest, AppUser>();
            }
        }
    }

    public class CreateUserRequestValidation : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidation()
        {
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Confirm password must match password");
        }
    }
   
}
