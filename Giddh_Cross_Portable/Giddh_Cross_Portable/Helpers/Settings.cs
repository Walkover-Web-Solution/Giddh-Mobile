// Helpers/Settings.cs
using Giddh_Cross_Portable.Model;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Giddh_Cross_Portable.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string AuthKey = "auth_key";
        private static string AuthKeyValue = string.Empty;

        private const string userUniqueNameKey = "user_obj";
        private static string userUniqueNameValue = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
            }
        }

        public static string AuthKeySettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(AuthKey, AuthKeyValue);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(AuthKey, value);
            }
        }

        public static string UserObjSetting
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(userUniqueNameKey, userUniqueNameValue);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(userUniqueNameKey, value);
            }
        }

    }
}