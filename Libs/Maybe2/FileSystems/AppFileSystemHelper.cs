using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maybe2.FileSystems
{
    public static class AppFileSystemHelper
    {

        public static bool IsDirectoryExists(this IAppFileSystem fs, string path)
        {
            return fs.GetDirectoryInfo(path).IsExists;
        }


        public static bool IsFileExists(this IAppFileSystem fs, string path)
        {
            return fs.GetFileInfo(path).IsExists;
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The path and name of the file to read.</param>
        /// <returns>A string containing all lines of the file, or <code>null</code> if the file doesn't exist.</returns>
        public static string ReadText(this IAppFileSystem fs, string path, Encoding encoding = null)
        {
            var fi = fs.GetFileInfo(path);
            return fi.ReadText(encoding);
        }

        public static void SaveText(this IAppFileSystem fs, string path, string content)
        {
            fs.GetFileInfo(path).SaveText(content);
        }

        public static void AppendText(this IAppFileSystem fs, string path, string content)
        {
            fs.GetFileInfo(path).AppendText(content);
        }

        public static void DeleteFile(this IAppFileSystem fs, string path)
        {
            var fi = fs.GetFileInfo(path);

            if (fi.IsExists)
                fi.Delete();
        }

        public static void DeleteDirectory(this IAppFileSystem fs, string path)
        {
            var fi = fs.GetDirectoryInfo(path);

            if (fi.IsExists)
                fi.Delete();
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

        public static IEnumerable<IAppDirectoryInfo> GetRootToSelfDirs(this IAppFileSystem fs, string filePath)
        {
            var paths = GetRootToSelfPaths(fs, filePath).ToArray();
            var ps = paths.Take(paths.Length - 1);
            var tall = ps.Select(s => fs.GetDirectoryInfo(s));
            return tall;
        }

        public static string ReadText(this IAppFileInfo fi, Encoding encoding = null)
        {
            if (!fi.IsDirectory && fi.IsExists)
            {
                var bytes = fi.ReadBytes();
                return (encoding ?? Encoding.UTF8).GetString(bytes);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void SaveText(this IAppFileInfo fs, string content)
        {
            fs.Save(new AppFileTextSaveCriteria(content));
        }

        public static void AppendText(this IAppFileInfo fs, string content)
        {
            fs.Save(new AppFileTextSaveCriteria(content)
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