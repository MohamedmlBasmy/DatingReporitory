using DatingApp.API.DTOs;
using DatingApp.API.Validations;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DatingApp.Tests
{
    [TestFixture]
    public class UsersTests
    {
        private UserDTOValidations validator;
        
        [SetUp]
        public void Setup()
        {
            validator = new UserDTOValidations();
        }

        [Test]
        public void WhenCustomerNameNull_ShouldHaveError()
        {
            var user = new UserDTO()
            {
                Password = "",
                KnownAs = "Mohamed"

            };
            var result =  validator.TestValidate(user);
            
            validator.ShouldHaveChildValidator(m => m.Username, null);
        }
    }
}