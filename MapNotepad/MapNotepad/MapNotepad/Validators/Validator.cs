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

        public static bool LongitudeValidator(double Longitude)
        {
            var isValid = false;

            if (Longitude > 180)
            {
                isValid = false;
            }
            else if (Longitude < 0)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        public static bool LatitudeValidator(double Latitude)
        {
            var isValid = false;

            if (Latitude > 90)
            {
                isValid = false;
            }
            else if (Latitude < -90)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        public static bool PinNameValidator(string pinName)
        {
            var isValid = false;

            if(string.IsNullOrWhiteSpace(pinName))
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
