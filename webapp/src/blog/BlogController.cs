using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        
        public static void Edit(RequestContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            Dictionary<string, object> flash = context.Session.Flash;

            
            string idString = request.QueryString["id"];
            BlogArticleForm form;

            flash.TryGetValue("form", out var flashForm);
            if (flashForm != null)
            {
                form = (BlogArticleForm) flashForm;
            }
            else
            {
                form = new BlogArticleForm();
            }
            if (idString != null)
            {
                int id = int.Parse(idString);
                BlogRepository blogRepository = context.BlogRepository;
                BlogArticle article = blogRepository.GetById(id);
                if (article == null)
                {
                    response.RedirectLocation = "/404";
                    response.StatusCode = 404;
                    return;
                }

                form.Title = article.Title;
                form.Content = article.Content;
                form.Id = article.Id.ToString();
            }

            response.RenderHtml("template.html", new ScriptObject
            {
                { "article", form },
                { "templateName", "blog/article-form.html" },
                { "title", "Edition" },
                { "flash", flash },
                { "form", form }
            });
        }

        public static void Post(RequestContext context)
        {
            BlogArticleForm form = new BlogArticleForm(context.Request.ParseEncodedFormUrlBody());
            string blogArticleId = form.Id;
            HttpResponse response = context.Response;
            Session session = context.Session;

            if (blogArticleId == null)
            {
                if (form.IsValid)
                {
                    BlogRepository blogRepository = context.BlogRepository;
                    BlogArticle blogArticle = new BlogArticle(form.Title, form.Content);
                    blogRepository.Persist(blogArticle);
                    session.AddFlash("confirmation", "blogArticle.saved");
                    response.RedirectLocation = $"/blog/edition?id={blogArticle.Id}";
                }
                else
                {
                    form.AddFlashMessagesToSession(session);
                    response.RedirectLocation = "/blog/edition";
                }
            }
            else
            {
                BlogRepository blogRepository = context.BlogRepository;
                BlogArticle blogArticle = blogRepository.GetById(int.Parse(blogArticleId));
                if (blogArticle == null)
                {
                    response.RedirectLocation = "/blog";
                }
                else
                {
                    blogArticle.Content = form.Content;
                    blogArticle.Title = form.Title;
                    blogRepository.Persist(blogArticle);
                    session.AddFlash("confirmation", "blogArticle.saved");
                    response.RedirectLocation = $"/blog/edition?id={blogArticle.Id}";
                }

            }

            response.StatusCode = 303;
        }
    }
}