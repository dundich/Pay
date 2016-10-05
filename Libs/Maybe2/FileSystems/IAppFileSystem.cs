using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maybe2.FileSystems
{

    public interface IAppFileSystem
    {
        /// <summary>
        /// Root Directory - virtual path
        /// </summary>
        string RootPath { get; }


        ///// <summary>
        /////  ('/d1/d2/', '\file.txt') => d1/d2/file.txt
        ///// </summary>
        ///// <param name="pats"></param>
        ///// <returns></returns>
        //string GetVirtualPath(params string[] pats);

        /// <summary>
        /// FileInfo /dfdf/dfdf/
        /// </summary>
        /// <param name="path">virtual path</param>        
        Task<IAppFileInfo> GetFileInfo(string path);

        /// <summary>
        /// DirectoryInfo
        /// </summary>
        Task<IAppDirectoryInfo> GetDirectoryInfo(string path = null);

        /// <summary>
        /// files by dir
        /// </summary>
        /// <param name="path"></param>
        /// <param name="matcher"></param>
        /// <returns></returns>
        Task<IEnumerable<IAppFileInfo>> ListFiles(string path, params string[] matcher);

        /// <summary>
        /// sub dirs
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<IEnumerable<IAppDirectoryInfo>> ListDirectories(string path);
    }
}
