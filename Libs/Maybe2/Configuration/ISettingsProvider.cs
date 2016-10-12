using Maybe2.FileSystems;
using System.Collections.Generic;
using System.Linq;

namespace Maybe2.Configuration
{
    public interface ISettingsProvider
    {
        IDictionary<string, string> LoadSettings();
        void SaveSettings(IDictionary<string, string> settings);
    }



    public class SettingsProvider : ISettingsProvider
    {
        readonly IAppFileInfo[] fi;

        public SettingsProvider(params IAppFileInfo[] file)
        {
            this.fi = file;
        }

        public IDictionary<string, string> LoadSettings()
        {
            var r = fi
                .Reverse()
                .Select(f =>
                {
                    var text = f.ReadText();
                    return text.GetLines()
                        .Select(c => new { c = c, i = c.IndexOf(':') })
                        .Where(c => c.i > 0)
                        .Select(c => c.c.TrySubstring(0, c.i).Pack().PairWith(c.c.TrySubstring(c.i + 1).Pack()))
                        .Where(c => IsValid(ref c));
                })
                .Aggregate((c1, c2) => c1.Concat(c2.Except(c1)))
                .ToDictionary(c => c.Key, c => c.Value);

            return r;
        }

        public void SaveSettings(IDictionary<string, string> settings)
        {
            var oldsettings = LoadSettings();

            var lines = oldsettings
                .Overload(settings)
                .Where(c => IsValid(ref c))
                .Select(c => c.Key.Trim() + " : " + c.Value.Trim())
                .JoinStrings(StringExtensions.NewLine);

            fi.Last().SaveText(lines);
        }

        private bool IsValid(ref KeyValuePair<string, string> c)
        {
            return !c.Key.IsNullOrWhiteSpace() && !c.Value.IsNullOrWhiteSpace();
        }

        public static ISettingsProvider CreateWebSettings()
        {
            return new SettingsProvider(new AppWebFileSystem().GetFileInfo("Settings.txt"));
        }
    }


    //public interface IShellConfig
    //{
    //    /// <summary>
    //    /// load configuration
    //    /// </summary>        
    //    IDictionary<string, string> GetSettings();
    //    /// <summary>
    //    /// reset cache for reload
    //    /// </summary>
    //    void Reset();
    //}

}