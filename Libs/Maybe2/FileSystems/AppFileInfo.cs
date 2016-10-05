using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        public abstract Task Delete();
        public abstract Task<Dictionary<string, string>> GetAttributes();
        public abstract Task Save(IAppFileSaveCriteria criteria);
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

        public override Task<Dictionary<string, string>> GetAttributes()
        {
            return Task.Run<Dictionary<string, string>>(() => new Dictionary<string, string>
            {
            });
        }

        public Task<byte[]> ReadBytes(IAppFileReadCriteria criteria = null)
        {
            return Task.Run(() => File.ReadAllBytes(PhysicalPath));
        }

        public override Task Save(IAppFileSaveCriteria criteria)
        {
            return Task.Run(() =>
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
            });
        }

        private void MakeDirectory()
        {
            _info.Directory.Create();
        }

        public override Task Delete()
        {
            return Task.Run(() => File.Delete(PhysicalPath));
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

        public override Task<Dictionary<string, string>> GetAttributes()
        {
            throw new NotImplementedException();
        }

        public override Task Save(IAppFileSaveCriteria criteria)
        {
            return Task.Run(() =>
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
            });
        }

        public override Task Delete()
        {
            return Task.Run(() => Directory.Delete(PhysicalPath));
        }
    }
}
