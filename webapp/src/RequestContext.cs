using System;
using System.Net;
using BlogApp.Framework;
using Npgsql;
using Scriban;

namespace webapp
{
    public class RequestContext : IDisposable
    {
        public HttpRequest Request { get; private set; }

        public HttpResponse Response { get; }

        public BlogRepository BlogRepository => new BlogRepository(DbConnection);

        public Session Session { get; set; }

        public SessionRepository SessionRepository => new SessionRepository();

        readonly HttpListenerContext _originalContext;
        private TemplateProvider _templateProvider;
        private DbConnectionFactory _dbConnectionFactory;

        private PgsqlConnection _dbConnection;
        private TemplateContext _templateContext;

        public PgsqlConnection DbConnection
        {
            get
            {
                if (_dbConnection == null)
                {
                    _dbConnection = _dbConnectionFactory.GetNewConnection();
                }

                return _dbConnection;
            }
        }

        internal RequestContext(HttpListenerContext originalContext,
            TemplateProvider templateProvider,
            DbConnectionFactory dbConnectionFactory)
        {
            _originalContext = originalContext;
            Request = new HttpRequest(_originalContext.Request);
            _templateProvider = templateProvider;
            Response = new HttpResponse(_originalContext.Response, this, templateProvider);
            _dbConnectionFactory = dbConnectionFactory;
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}