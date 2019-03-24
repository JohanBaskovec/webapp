using System.Collections.Specialized;
using webapp.form;

namespace webapp.blog
{
    public class BlogArticleForm : Form
    {
        [NotEmpty]
        public string Title { get; set; }

        [NotEmpty]
        public string Content
        {
            get; set;
        }

        public BlogArticleForm(NameValueCollection requestBody) : base(requestBody)
        {
        }

        public BlogArticleForm() : base()
        {
            
        }
    }

}