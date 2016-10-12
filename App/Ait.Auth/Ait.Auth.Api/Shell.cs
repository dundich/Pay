﻿using Maybe2;
using Maybe2.Configuration;

namespace Ait.Auth.Api
{
    class Shell : ShellSettings
    {
        const string DB_KEY = "Db";

        public static Shell Default => new Shell();

        public Shell() : base(SettingsProvider.CreateWebSettings())
        {
        }

        /// <summary>
        /// Коннекшен считываем из настроек App_Data/Settings.txt
        /// </summary>
        public static string GetConnectionString()
        {
            return Default.GetSettings().GetOrDefault(DB_KEY) ?? DB_KEY;
        }
    }
}