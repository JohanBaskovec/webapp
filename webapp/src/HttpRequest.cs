using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace webapp
{
    public class HttpRequest
    {
        internal HttpListenerRequest _originalRequest;

        public HttpRequest(HttpListenerRequest originalRequest)
        {
            _originalRequest = originalRequest;
        }

        public NameValueCollection ParseEncodedFormUrlBody()
        {
            StreamReader reader = new StreamReader(InputStream);
            string body = reader.ReadToEnd();
            NameValueCollection bodyObject = HttpUtility.ParseQueryString(body);
            return bodyObject;
        }

        public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
        {
            return _originalRequest.BeginGetClientCertificate(requestCallback, state);
        }

        public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
        {
            return _originalRequest.EndGetClientCertificate(asyncResult);
        }

        public X509Certificate2 GetClientCertificate()
        {
            return _originalRequest.GetClientCertificate();
        }

        public Task<X509Certificate2> GetClientCertificateAsync()
        {
            return _originalRequest.GetClientCertificateAsync();
        }

        public string[] AcceptTypes => _originalRequest.AcceptTypes;

        public int ClientCertificateError => _originalRequest.ClientCertificateError;

        public Encoding ContentEncoding => _originalRequest.ContentEncoding;

        public long ContentLength64 => _originalRequest.ContentLength64;

        public string ContentType => _originalRequest.ContentType;

        public CookieCollection Cookies => _originalRequest.Cookies;

        public bool HasEntityBody => _originalRequest.HasEntityBody;

        public NameValueCollection Headers => _originalRequest.Headers;

        public string HttpMethod => _originalRequest.HttpMethod;

        public Stream InputStream => _originalRequest.InputStream;

        public bool IsAuthenticated => _originalRequest.IsAuthenticated;

        public bool IsLocal => _originalRequest.IsLocal;

        public bool IsSecureConnection => _originalRequest.IsSecureConnection;

        public bool IsWebSocketRequest => _originalRequest.IsWebSocketRequest;

        public bool KeepAlive => _originalRequest.KeepAlive;

        public IPEndPoint LocalEndPoint => _originalRequest.LocalEndPoint;

        public Version ProtocolVersion => _originalRequest.ProtocolVersion;

        public NameValueCollection QueryString => _originalRequest.QueryString;

        public string RawUrl => _originalRequest.RawUrl;

        public IPEndPoint RemoteEndPoint => _originalRequest.RemoteEndPoint;

        public Guid RequestTraceIdentifier => _originalRequest.RequestTraceIdentifier;

        public string ServiceName => _originalRequest.ServiceName;

        public TransportContext TransportContext => _originalRequest.TransportContext;

        public Uri Url => _originalRequest.Url;

        public Uri UrlReferrer => _originalRequest.UrlReferrer;

        public string UserAgent => _originalRequest.UserAgent;

        public string UserHostAddress => _originalRequest.UserHostAddress;

        public string UserHostName => _originalRequest.UserHostName;

        public string[] UserLanguages => _originalRequest.UserLanguages;
    }
}