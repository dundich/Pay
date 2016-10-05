using System;

namespace Maybe2.FileSystems
{

    //public class AppIoDefaultConfig
    //{
    //    public AppFileSystemConfig filesystem { get; set; }
    //}

    //public class AppIO
    //{
    //    public Func<AppIO> init;
    //    public Func<IAppFileSystem> filesystem;

    //    public AppIO(bool? isHosted = null)
    //    {
    //        init = FuncUtils.One(() =>
    //        {
    //            filesystem = FuncUtils.If<IAppFileSystem>(
    //                () => (isHosted ?? ApplicationInfo.IsHosted()),
    //                () => new AppWebFileSystem(   ),
    //                () => new AppWinFileSystem(   ));
                
    //            return this;
    //        });
    //    }

    //    public T Get<T>()
    //    {
    //        init();

    //        var t = typeof(T);

    //        if (t.Is<IAppFileSystem>())
    //            return filesystem().As<T>();

    //        return default(T);
    //    }

    //    #region xxx

    //    #endregion
    //}
}
