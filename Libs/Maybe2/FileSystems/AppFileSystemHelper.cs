using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maybe2.FileSystems
{
    public static class AppFileSystemHelper
    {

        public static async Task<bool> IsDirectoryExists(this IAppFileSystem fs, string path)
        {
            return (await fs.GetDirectoryInfo(path)).IsExists;
        }


        public static async Task<bool> IsFileExists(this IAppFileSystem fs, string path)
        {
            return (await fs.GetFileInfo(path)).IsExists;
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The path and name of the file to read.</param>
        /// <returns>A string containing all lines of the file, or <code>null</code> if the file doesn't exist.</returns>
        public static async Task<string> ReadText(this IAppFileSystem fs, string path, Encoding encoding = null)
        {
            var fi = await fs.GetFileInfo(path);
            return await fi.ReadText(encoding);
        }

        public static async Task SaveText(this IAppFileSystem fs, string path, string content)
        {
            var fi = await fs.GetFileInfo(path);
            await fi.SaveText(content);
        }

        public static async Task AppendText(this IAppFileSystem fs, string path, string content)
        {
            var fi = await fs.GetFileInfo(path);
            await fi.AppendText(content);
        }

        public static async Task DeleteFile(this IAppFileSystem fs, string path)
        {
            var fi = await fs.GetFileInfo(path);

            if (fi.IsExists)
                await fi.Delete();
        }

        public static async Task DeleteDirectory(this IAppFileSystem fs, string path)
        {
            var fi = await fs.GetDirectoryInfo(path);

            if (fi.IsExists)
                await fi.Delete();
        }


        public static string GetVirtualPath(string paths)
        {
            return GetVirtualPath(Split(paths));
        }

        private static string GetVirtualPath(IEnumerable<string> paths)
        {
            return paths.Where(c => !c.IsNullOrEmpty()).JoinStrings("/");
        }


        static IEnumerable<string> Split(string paths)
        {
            if (paths.IsNullOrEmpty())
                return Enumerable.Empty<string>();

            return paths
                .Split('\\', '/')
                .Where(c => !c.IsNullOrEmpty());
        }

        public static string GetFileName(this IAppFileSystem fs, string filePath)
        {
            return Split(filePath).LastOrDefault();
        }

        public static string GetDirectoryName(this IAppFileSystem fs, string filePath)
        {
            return Split(filePath).Reverse().Skip(1).FirstOrDefault();
        }

        public static IEnumerable<string> GetRootToSelfPaths(this IAppFileSystem fs, string filePath)
        {
            IEnumerable<string> ps = Split(filePath);
            ps = EnumerableExtensions.Concat(string.Empty, ps);
            return ps.Select((c, i) => GetVirtualPath(ps.Take(i + 1)));
        }

        public static Task<IAppDirectoryInfo[]> GetRootToSelfDirs(this IAppFileSystem fs, string filePath)
        {
            var paths = GetRootToSelfPaths(fs, filePath).ToArray();
            var ps = paths.Take(paths.Length - 1);
            var tall = ps.Select(s => fs.GetDirectoryInfo(s));
            return Task.WhenAll(tall);
        }

        public static async Task<string> ReadText(this IAppFileInfo fi, Encoding encoding = null)
        {
            if (!fi.IsDirectory && fi.IsExists)
            {
                var bytes = await fi.ReadBytes();
                return (encoding ?? Encoding.UTF8).GetString(bytes);
            }
            else
            {
                return await Task.FromResult(string.Empty);
            }
        }

        public static async Task SaveText(this IAppFileInfo fs, string content)
        {
            await fs.Save(new AppFileTextSaveCriteria(content));
        }

        public static async Task AppendText(this IAppFileInfo fs, string content)
        {
            await fs.Save(new AppFileTextSaveCriteria(content)
            {
                Action = SaveAction.Append
            });
        }



        class AppFileTextSaveCriteria : IAppFileSaveCriteria
        {

            public AppFileTextSaveCriteria()
            {
            }

            public AppFileTextSaveCriteria(string content)
            {
                Content = () => Encoding.UTF8.GetBytes(content);
            }

            public SaveAction Action = SaveAction.CreateOrUpdate;

            public Func<byte[]> Content;

            public SaveAction GetAction()
            {
                return Action;
            }

            public byte[] GetData()
            {
                return Content();
            }
        }
    }
}