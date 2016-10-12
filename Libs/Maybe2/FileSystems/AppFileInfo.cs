using System;
using System.IO;

namespace Maybe2.FileSystems
{

    abstract class AppFileSystemInfo : IAppFileSystemInfo
    {
        public abstract bool IsExists { get; }
        public abstract bool IsDirectory { get; }
        public abstract DateTimeOffset LastModified { get; }
        public abstract long Length { get; }
        public abstract string Name { get; }
        public abstract string PhysicalPath { get; }
        public abstract void Delete();
        public abstract void Save(IAppFileSaveCriteria criteria);
    }



    class AppFileInfoImpl : AppFileSystemInfo, IAppFileInfo
    {
        private readonly FileInfo _info;

        public AppFileInfoImpl(FileInfo info)
        {
            _info = info;
        }

        public override bool IsExists => _info.Exists;

        public override long Length => _info.Length;

        public override string PhysicalPath => _info.FullName;

        public override string Name => _info.Name;

        public override DateTimeOffset LastModified => _info.LastWriteTimeUtc;

        public override bool IsDirectory => false;

        //public override Task<Dictionary<string, string>> GetAttributes()
        //{
        //    return Task.Run<Dictionary<string, string>>(() => new Dictionary<string, string>
        //    {
        //    });
        //}

        public byte[] ReadBytes(IAppFileReadCriteria criteria = null)
        {
            return File.ReadAllBytes(PhysicalPath);
        }

        public override void Save(IAppFileSaveCriteria criteria)
        {
            //Check Dir
            MakeDirectory();

            var action = criteria.GetAction();
            var bytes = criteria.GetData();
            switch (action)
            {
                case SaveAction.CreateNew:
                    using (var fs = new FileStream(PhysicalPath, FileMode.CreateNew, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    break;

                case SaveAction.CreateOrUpdate:
                    File.WriteAllBytes(PhysicalPath, bytes);
                    break;

                case SaveAction.Append:
                    using (var stream = new FileStream(PhysicalPath, FileMode.Append))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    break;

                case SaveAction.Truncate:
                    File.WriteAllText(PhysicalPath, string.Empty);
                    break;
            }
        }

        private void MakeDirectory()
        {
            _info.Directory.Create();
        }

        public override void Delete()
        {
            File.Delete(PhysicalPath);
        }
    }


    class AppDirInfoImpl : AppFileSystemInfo, IAppDirectoryInfo
    {
        private readonly DirectoryInfo _info;

        public AppDirInfoImpl(DirectoryInfo info)
        {
            _info = info;
        }

        public override bool IsExists => _info.Exists;

        public override long Length => -1;

        public override string PhysicalPath => _info.FullName;

        public override string Name => _info.Name;

        public override DateTimeOffset LastModified => _info.LastWriteTimeUtc;

        public override bool IsDirectory => true;

        public override void Save(IAppFileSaveCriteria criteria)
        {
            var action = criteria.GetAction();
            var bytes = criteria.GetData();
            switch (action)
            {
                case SaveAction.CreateNew:
                case SaveAction.CreateOrUpdate:
                case SaveAction.Append:
                    Directory.CreateDirectory(PhysicalPath);
                    break;

                case SaveAction.Truncate:
                    //File.Read
                    //File.WriteAllText(PhysicalPath, string.Empty);
                    break;
            }

        }

        public override void Delete()
        {
            Directory.Delete(PhysicalPath);
        }
    }
}
