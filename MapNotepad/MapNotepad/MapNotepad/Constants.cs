using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotepad
{
    public static class Constants
    {
        public const string DATABASE_NAME = "Repository.db";

        public const string ACTION = "Action";

        public const string EMAIL = "Email";

        public const string EMAIL_REGEX = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" +
                                           "@"+
                                          @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

        public const string PASSWORD_REGEX = @"(?=^[^\s]{4,20}$)(?=.*\d)(?=.*[a-zA-Z])";
    }
}
