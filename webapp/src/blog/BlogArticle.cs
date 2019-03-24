namespace webapp.blog
{
    public class BlogArticle
    {
        public string Title { get; set; }

        public string Content
        {
            get; set;
        }
        
        public int Id { get; set; }

        public BlogArticle(string title = "", string content = "", int id = 0)
        {
            Title = title;
            Content = content;
            Id = id;
        }
     
        protected bool Equals(BlogArticle other)
        {
            return string.Equals(Title, other.Title) && string.Equals(Content, other.Content);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BlogArticle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Title != null ? Title.GetHashCode() : 0) * 397) ^ (Content != null ? Content.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"Title: {Title}, Content: {Content}";
        }
    }
}