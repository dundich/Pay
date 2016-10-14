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

        public static string DefaultFileName = "Settings.txt";

        public SettingsProvider(params IAppFileInfo[] file)
        {
            this.fi = file;
        }

        public IDictionary<string, string> LoadSettings()
        {
            var r = fi
                .Where(f => f.IsExists)
                .Reverse()
                .Select(f =>
                {
                    var text = f.ReadText();
                    return text.GetLines()
                        .Select(c => new { c = c, i = c.IndexOf(':') })
                        .Where(c => c.i > 0)
                        .Select(c => c.c.TrySubstring(0, c.i).Pack()
                            .PairWith(c.c.TrySubstring(c.i + 1).Pack()))
                        .Where(c => IsValid(ref c));
                })
                .Aggregate((c1, c2) =>
                    c1.Concat(c2.Where(c => !c1.Any(k => k.Key == c.Key)))
                )
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

        public static ISettingsProvider CreateProvider(string dir = null, string filename = null, bool isWebHost = true)
        {
            var fs = isWebHost
                ? new AppWebFileSystem()
                : new AppWinFileSystem();

            var dirs = fs.GetRootToSelfPaths(dir).ToArray();

            filename = filename.PackToNull() ?? DefaultFileName;

            var files = dirs
                .Select(d => d.EnsureTrailingSlash() + filename)
                .Select(f => fs.GetFileInfo(f))
                .ToArray();

            return new SettingsProvider(files);
        }
    }


    public static class SettingsProviderHelper
    {
        public static ISettingsProvider SaveSettings(this ISettingsProvider sett, params KeyValuePair<string, string>[] vals)
        {
            var d = vals.Where(c => !c.Key.IsNullOrWhiteSpace()).ToDictionary();
            sett.SaveSettings(d);
            return sett;
        }
    }

}