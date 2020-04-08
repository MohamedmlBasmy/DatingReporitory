using System;

namespace DatingApp.API.Helper
{
    public static class Extentions
    {
        public static int CalculateAge(this DateTime date){
            var age = DateTime.Now.Year - date.Year;
            if (date.AddYears(age) > DateTime.Today)
            {
                age--;
            }
            return age;
        }
    }
}