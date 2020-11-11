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

        public const string WEATHER_URL = "http://api.openweathermap.org/data/2.5/forecast?lat=";

        public const string WEATHER_APIKEY = "8e1cef07d8affb9f4bf5bff5652eb06c";

        public const string WEATHER_ICONS = "http://openweathermap.org/img/wn/";
    }
}
