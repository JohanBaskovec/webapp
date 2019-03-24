using System.Collections.Generic;
using System.Net;
using Scriban;
using Scriban.Runtime;

namespace webapp
{
    public class HttpResponse
    {
        private readonly TemplateProvider _templateProvider;
        private readonly HttpListenerResponse _response;
        private RequestContext _context;
        public string ResponseString { get; private set; }

        public HttpResponse(HttpListenerResponse response, RequestContext context, TemplateProvider templateProvider)
        {
            _response = response;
            _templateProvider = templateProvider;
            _context = context;
        }
        
        public void RenderHtml(string templateName, ScriptObject model)
        { 
            model["flashMessageLists"] = _context.Session.FlashMessageLists;
            model["flashMessages"] = _context.Session.FlashMessages;
            ResponseString = _templateProvider.Render(templateName, model);
        }

        public void AddHeader(string name, string value)
        {
            _response.AddHeader(name, value);
        }

        public void AppendCookie(Cookie cookie)
        {
            _response.AppendCookie(cookie);
        }

        public void SetCookie(Cookie cookie)
        {
            _response.SetCookie(cookie);
        }

        public string ContentType
        {
            get => _response.ContentType;
            set => _response.ContentType = value;
        }

        public CookieCollection Cookies
        {
            get => _response.Cookies;
            set => _response.Cookies = value;
        }

        public string RedirectLocation
        {
            get => _response.RedirectLocation;
            set => _response.RedirectLocation = value;
        }

        public int StatusCode
        {
            get => _response.StatusCode;
            set => _response.StatusCode = value;
        }
    }
}