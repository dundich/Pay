using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maybe2.FileSystems
{
    public enum SaveAction
    {
        /// <summary>
        /// Specifies that the operating system should create a new file. 
        /// If the file already exists, it will be overwritten. 
        /// </summary>
        CreateOrUpdate = 1,
        /// <summary>
        /// If the file already exists then System.IO.IOException exception
        /// </summary>
        CreateNew = 2,
        /// <summary>
        /// Добавить 
        /// </summary>
        Append = 3,
        /// <summary>
        /// Стереть всё
        /// </summary>
        Truncate = 5,
    }


    public interface IAppFileReadCriteria
    {
    }

    public interface IAppFileSaveCriteria
    {
        SaveAction GetAction();
        Byte[] GetData();
    }


    public interface IAppFileSystemInfo
    {
        /// <summary>
        /// True if resource exists in the underlying storage system.
        /// </summary>
        bool IsExists { get; }

        /// <summary>
        /// The length of the file in bytes, or -1 for a directory or non-existing files.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// The path to the file, including the file name. Return null if the file is not directly accessible.
        /// </summary>
        //string PhysicalPath { get; }

        /// <summary>
        /// The name of the file or directory, not including any path.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// When the file was last modified
        /// </summary>
        DateTimeOffset LastModified { get; }

        /// <summary>
        /// True for the case TryGetDirectoryContents has enumerated a sub-directory
        /// </summary>
        bool IsDirectory { get; }

        /// <summary>
        /// Meta Information(Author, Creator ....)
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetAttributes();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task Save(IAppFileSaveCriteria criteria);

        /// <summary>
        /// File or Directory Delete
        /// </summary>        
        Task Delete();
    }


    /// <summary>
    /// Represents a file in the given file provider.
    /// </summary>
    public interface IAppFileInfo : IAppFileSystemInfo
    {
        Task<byte[]> ReadBytes(IAppFileReadCriteria criteria = null);
    }


    public interface IAppDirectoryInfo : IAppFileSystemInfo
    {
    }
}
