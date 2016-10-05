using Maybe2.FileSystems;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maybe2.Configuration
{
    public interface IShellSettings
    {
        /// <summary>
        /// Retrieves all shell settings stored.
        /// </summary>
        /// <returns>All shell settings.</returns>
        Task<IDictionary<string, string>> LoadSettings();

        /// <summary>
        /// Persists shell settings to the storage.
        /// </summary>
        /// <param name="settings">The shell settings to store.</param>
        Task SaveSettings(IDictionary<string, string> settings);
    }



    public class ShellSettingsFile : IShellSettings
    {
        readonly IAppFileInfo fi;

        public ShellSettingsFile(IAppFileInfo file)
        {
            this.fi = file;
        }

        public async Task<IDictionary<string, string>> LoadSettings()
        {
            var text = await fi.ReadText();

            Dictionary<string, string> dict = new Dictionary<string, string> { };

            text.GetLines()
                .Select(c => c.Split(':'))
                .Where(c => c.Length >= 2)
                .Select(c => c[0].PairWith(c[1]))
                .Where(c => IsValid(ref c))
                .ForEach(c => dict[c.Key.Trim()] = c.Value.Trim());

            return dict;
        }

        public async Task SaveSettings(IDictionary<string, string> settings)
        {
            var oldsettings = await LoadSettings();

            var lines = oldsettings
                .Overload(settings)
                .Where(c => IsValid(ref c))
                .Select(c => c.Key.Trim() + " : " + c.Value.Trim())
                .JoinStrings(StringExtensions.NewLine);

            await fi.SaveText(lines);
        }

        private static bool IsValid(ref KeyValuePair<string, string> c)
        {
            return !c.Key.IsNullOrWhiteSpace() && !c.Value.IsNullOrWhiteSpace();
        }
    }



    public class ShellSettingsFileProvider
    {
        protected readonly IAppFileSystem fileSystem;

        public ShellSettingsFileProvider(IAppFileSystem fs)
        {
            fileSystem = fs;
        }
    }


    public class ShellFileSettingsProviderEx
    {
        protected readonly IAppFileSystem fileSystem;

        public readonly string settingsFile;

        public ShellFileSettingsProviderEx(IAppFileSystem fs)
        {
            fileSystem = fs;
        }

        public async Task<IDictionary<string, string>> GetSettings(string file = "Settings.txt")
        {
            IAppFileInfo fi = await GetFile(file);
            return await ReadFile(fi);
        }

        protected async Task<IAppFileInfo> GetFile(string file)
        {
            return await fileSystem.GetFileInfo(file);
        }

        protected virtual async Task<Dictionary<string, string>> ReadFile(IAppFileInfo fi)
        {
            var text = await fi.ReadText();

            Dictionary<string, string> dict = new Dictionary<string, string> { };

            text.GetLines()
                .Select(c => new { c = c, i = c.IndexOf(':') })
                .Where(c => c.i > 0)
                .Select(c => c.c.TrySubstring(0, c.i).PairWith(c.c.TrySubstring(c.i + 1)))
                .Where(c => IsValid(ref c))
                .ForEach(c => dict[c.Key.Trim()] = c.Value.Trim());

            return dict;
        }


        public async Task SaveSettings(IDictionary<string, string> settings, string file = "Settings.txt")
        {
            IAppFileInfo fi = await GetFile(file);
            await SaveFile(fi, settings);
        }

        protected async Task SaveFile(IAppFileInfo fi, IDictionary<string, string> settings)
        {
            var oldsettings = await ReadFile(fi);

            var lines = oldsettings
                .Overload(settings)
                .Where(c => !c.Value.IsNullOrWhiteSpace())
                .Select(c => c.Key.Trim() + " : " + c.Value.Trim())
                .JoinStrings(StringExtensions.NewLine);

            await fi.SaveText(lines);
        }

        private static bool IsValid(ref KeyValuePair<string, string> c)
        {
            return !c.Key.IsNullOrWhiteSpace() && !c.Value.IsNullOrWhiteSpace();
        }
    }


    public class ShellFileSettings : IShellSettings
    {

        readonly ShellFileSettingsProviderEx fp;
        readonly string filesetts;

        public static IShellSettings CreateWebShellSettings(string filesetts = "Settings.txt")
        {
            return new ShellFileSettings(new ShellFileSettingsProviderEx(new AppWebFileSystem()), filesetts);
        }

        public ShellFileSettings(ShellFileSettingsProviderEx fp, string filesetts = "Settings.txt")
        {
            this.fp = fp;
            this.filesetts = filesetts;
        }


        public async Task<IDictionary<string, string>> LoadSettings()
        {
            var t1 = fp.GetSettings();
            var t2 = fp.GetSettings(filesetts);

            return (await t1).Overload(await t2);
        }

        public async Task SaveSettings(IDictionary<string, string> settings)
        {
            await fp.SaveSettings(settings, filesetts);
        }
    }
}
