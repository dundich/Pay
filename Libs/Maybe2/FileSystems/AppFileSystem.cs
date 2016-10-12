using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;

namespace Maybe2.FileSystems
{

    public class AppFileSystemConfig
    {
        /// <summary>
        /// virtual path to context
        /// </summary>
        public string RootPath = "App_Data";
    }

    public class AppWinFileSystem : IAppFileSystem
    {

        readonly AppFileSystemConfig config;

        /// <summary>
        /// virtual relative to Physical Root Path
        /// </summary>
        protected Func<string> GetPhysicalRootPath;

        protected Func<string> GetRootPath;


        public string RootPath => GetRootPath();


        public AppWinFileSystem(AppFileSystemConfig config = null)
        {
            this.config = config ?? new AppFileSystemConfig();

            GetRootPath = FuncUtils.One(() =>
                AppFileSystemHelper.GetVirtualPath(this.config.RootPath));

            GetPhysicalRootPath = FuncUtils.One(() =>
            {
                var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var relPaths = RootPath.Replace('/', Path.DirectorySeparatorChar);
                return Path.Combine(rootPath, relPaths);
            });
        }

        protected virtual string CombineToPhysicalPath(string path)
        {
            var rel = AppFileSystemHelper.GetVirtualPath(path).Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(GetPhysicalRootPath(), rel);
        }

        protected virtual IAppFileInfo CreateFileInfo(string path)
        {
            var physicalPath = CombineToPhysicalPath(path);
            return new AppFileInfoImpl(new FileInfo(physicalPath));
        }

        protected virtual IAppDirectoryInfo CreateDirectoryInfo(string path)
        {
            var physicalPath = CombineToPhysicalPath(path);
            return new AppDirInfoImpl(new DirectoryInfo(physicalPath));
        }

        public IAppFileInfo GetFileInfo(string path)
        {
            return CreateFileInfo(path);
        }

        public IAppDirectoryInfo GetDirectoryInfo(string path = null)
        {
            return CreateDirectoryInfo(path);
        }

        public IEnumerable<IAppFileInfo> ListFiles(string path, params string[] matcher)
        {
            var physicalPath = CombineToPhysicalPath(path);
            var files = Directory.GetFiles(physicalPath, "*", SearchOption.TopDirectoryOnly);
            return files.Select(f => CreateFileInfo(f));
        }

        public IEnumerable<IAppDirectoryInfo> ListDirectories(string path)
        {
            var physicalPath = CombineToPhysicalPath(path);
            var dirs = Directory.GetDirectories(physicalPath, "*", SearchOption.TopDirectoryOnly);
            return dirs.Select(f => CreateDirectoryInfo(f));
        }
    }


    public class AppWebFileSystem : AppWinFileSystem
    {
        public AppWebFileSystem(AppFileSystemConfig config = null)
            : base(config)
        {
            GetPhysicalRootPath = FuncUtils.One(() =>
           {
               var path = "~/" + RootPath.Split('/', '\\').JoinStrings("/");
               return HostingEnvironment.MapPath(path);
           });
        }
    }
}
