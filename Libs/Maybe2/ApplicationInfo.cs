using System;
using System.Linq;
using System.Reflection;

namespace Maybe2
{
    static public class ApplicationInfo
    {

        public static Version GetVersion<T>()
        {
            return Assembly.GetAssembly(typeof(T)).GetName().Version;
        }

        public static Version Version { get { return Assembly.GetCallingAssembly().GetName().Version; } }

        public static string Title
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title.Length > 0) return titleAttribute.Title;
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string ProductName
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string Description
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string CopyrightHolder
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string CompanyName
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        public static bool IsHosted()
        {
            try
            {
                var webAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(a => a.FullName.StartsWith("System.Web"));

                foreach (var webAssembly in webAssemblies)
                {
                    var hostingEnvironmentType = webAssembly.GetType("System.Web.Hosting.HostingEnvironment");
                    if (hostingEnvironmentType != null)
                    {
                        var isHostedProperty = hostingEnvironmentType.GetProperty("IsHosted",
                          BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public);
                        if (isHostedProperty != null)
                        {
                            object result = isHostedProperty.GetValue(null, null);
                            if (result is bool)
                            {
                                return (bool)result;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Failed to find or execute HostingEnvironment.IsHosted; assume false
            }
            return false;
        }
    }
}
