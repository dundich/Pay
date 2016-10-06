namespace Maybe2
{
    using System;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    public static class uriHelper
    {

        public static bool IsLocalIPAddress(string uriOrHost, bool IsOnlyLoopback = false)
        {
            if (uriOrHost.IsNullOrWhiteSpace()) return false;

            try
            {
                uriOrHost = uriHelper.GetHost(uriOrHost);

                // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(uriOrHost);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    if (!IsOnlyLoopback)
                    {
                        // is local address
                        foreach (IPAddress localIP in localIPs)
                        {
                            if (hostIP.Equals(localIP)) return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// внутренюю адресацию меняем на внешнюю ....        
        /// </summary>
        public static string ReplaceLoopbackToProxyHost(this string fromUri, string toProxyUriOrHost)
        {
            if (toProxyUriOrHost.IsNullOrEmpty())
                return fromUri;

            //http://localhost/...  -> http://selfrec/...
            var islocal = uriHelper.IsLocalIPAddress(fromUri, true);
            if (islocal)// && !uriHelper.IsLocalIPAddress(toProxyHost))
            {
                var newRepUri = uriHelper.ReplaceHost(fromUri, toProxyUriOrHost);
                if (!newRepUri.IsLoopback)
                    return newRepUri.AbsoluteUri.EnsureTrailingSlash();
            }

            return fromUri;
        }

        public static bool IsLocalIPAddress(this Uri uri)
        {
            if (uri == null) return false;
            return IsLocalIPAddress(uri.Host);
        }

        public static Uri ReplaceHost(string fromUri, string toUriOrHost)
        {
            var newUriBuilder = new UriBuilder(fromUri);
            newUriBuilder.Host = GetHost(toUriOrHost);
            newUriBuilder.Port = GetTryUri(toUriOrHost).NoNull(c => c.Port);
            return newUriBuilder.Uri;
        }

        public static string GetHost(string uriOrHost)
        {
            return GetTryUri(uriOrHost).NoNull(c => c.Host, uriOrHost);
        }


        public static bool IsLocalOrLanIP(string ip)
        {
            return IsLocalIPAddress(ip) || IsLanIP(ip);
        }

        public static bool IsLanIP(string ip)
        {
            if (ip.IsNullOrWhiteSpace())
                return false;

            IPAddress _ip = null;
            if (IPAddress.TryParse(ip, out _ip))
                return _ip.IsLanIP();
            return false;
        }

        public static bool IsLanIP(this IPAddress address)
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var iface in interfaces)
            {
                var properties = iface.GetIPProperties();
                foreach (var ifAddr in properties.UnicastAddresses)
                {
                    if (ifAddr.IPv4Mask != null &&
                        ifAddr.Address.AddressFamily == AddressFamily.InterNetwork &&
                        CheckMask(ifAddr.Address, ifAddr.IPv4Mask, address))
                        return true;
                }
            }
            return false;
        }

        private static bool CheckMask(IPAddress address, IPAddress mask, IPAddress target)
        {
            if (mask == null)
                return false;

            var ba = address.GetAddressBytes();
            var bm = mask.GetAddressBytes();
            var bb = target.GetAddressBytes();

            if (ba.Length != bm.Length || bm.Length != bb.Length)
                return false;

            for (var i = 0; i < ba.Length; i++)
            {
                int m = bm[i];

                int a = ba[i] & m;
                int b = bb[i] & m;

                if (a != b)
                    return false;
            }

            return true;
        }

        public static Uri GetTryUri(string uriOrHost)
        {
            Uri result = null;
            Uri.TryCreate(uriOrHost, UriKind.Absolute, out result);
            return result;
        }

        public static bool IsHTTPs(string uriName)
        {
            return GetHttpUri(uriName) != null;
        }

        public static Uri GetHttpUri(string uriName)
        {
            Uri uriResult;
            if (Uri.TryCreate(uriName, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                return uriResult;
            return null;
        }
    }
}


