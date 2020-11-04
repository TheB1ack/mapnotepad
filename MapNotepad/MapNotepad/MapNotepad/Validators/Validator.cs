using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MapNotepad.Validators
{
    public class Validator
    {
        public static bool EmailValidator(string email)
        {
            var isValid = false;
            var isMatch = Regex.IsMatch(email, Constants.EMAIL_REGEX);

            if (isMatch)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        public static bool PasswordValidator(string password)
        {
            var isValid = false;

            var isMatch = Regex.IsMatch(password, Constants.PASSWORD_REGEX);

            if (isMatch)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        public static bool UserNameValidator(string userName)
        {
            var isValid = false;

            if (userName.Length < 2)
            {
                isValid = false;
            }
            else if(userName.Length > 20)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

    }
}
