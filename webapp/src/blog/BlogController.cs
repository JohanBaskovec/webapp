using System.Collections.Generic;
using NLog;
using Scriban.Runtime;

namespace webapp.blog
{
    public static class BlogController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Get(RequestContext context)
        {
            HttpRequest request = context.Request;
            string pageString = request.QueryString["page"];
            
            int page = pageString == null ? 0 : int.Parse(request.QueryString["page"]);
            HttpResponse response = context.Response;
            BlogRepository blogRepository = context.BlogRepository;
            List<BlogArticle> articles = blogRepository.GetAll(page);
            int nPages = blogRepository.CountPages();
            response.RenderHtml("template.html", new ScriptObject
            {
                { "articles", articles },
                { "templateName", "blog/blog.html" },
                { "title", "Blog" },
                { "nPages", nPages }
            });
        }

        public static void Post(RequestContext context)
        {
            BlogArticleForm form = new BlogArticleForm(context.Request.ParseEncodedFormUrlBody());
            HttpResponse response = context.Response;
            Session session = context.Session;

            if (!form.IsValid)
            {
                form.AddFlashMessagesToSession(session);
            }
            else
            {
                BlogRepository blogRepository = context.BlogRepository;
                BlogArticle blogArticle = new BlogArticle(form.Title, form.Content);
                blogRepository.Persist(blogArticle);
                session.AddFlashMessage("confirmation", "blogArticle.saved");
            }
            
            response.RedirectLocation = "/blog";
            response.StatusCode = 303;
        }
    }
}