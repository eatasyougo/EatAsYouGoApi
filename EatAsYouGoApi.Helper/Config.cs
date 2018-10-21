using System;
using System.Configuration;

namespace EatAsYouGoApi.Helper
{
    public class Config
    {
        public static int AuthTokenExpiryInMins
        {
            get
            {
                int authTokenExpiryInMins;
                int.TryParse(ConfigurationManager.AppSettings.Get("AuthTokenExpiryInMins"), out authTokenExpiryInMins);
                return authTokenExpiryInMins;
            }
        }

        public static int RefreshTokenExpiryInMins {
            get
            {
                int refreshTokenExpiryInMins;
                int.TryParse(ConfigurationManager.AppSettings.Get("RefreshTokenExpiryInMins"), out refreshTokenExpiryInMins);
                return refreshTokenExpiryInMins;
            }
        }

        public static string SmtpHost => ConfigurationManager.AppSettings.Get("SmtpHost");
    }
}
