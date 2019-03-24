using System.Collections.Generic;
using System.Data.Common;
using BlogApp.Framework;
using NLog;
using Npgsql;
using NpgsqlTypes;
using webapp.blog;

namespace webapp
{
    public class BlogRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly PgsqlConnection _dbConnection;

        public BlogRepository(PgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<BlogArticle> GetAll()
        {
            Logger.Info("Getting all articles.");
            List<BlogArticle> ret = new List<BlogArticle>();
            using (DbCommand cmd = _dbConnection.CreateCommand())
            {
                cmd.CommandText = "select id, title, content from article";
                cmd.Prepare();
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    BlogArticle article = CreateArticleFromDbRow(reader);
                    ret.Add(article);
                }
            }

            return ret;
        }

        public void Persist(BlogArticle article)
        {
            if (article.Id == 0)
            {
                Logger.Info($"Saving new article");
                if (Logger.IsTraceEnabled)
                {
                    Logger.Trace(article.ToString);
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = _dbConnection.NpgsqlConnection;
                    cmd.CommandText = "insert into article (title, content) values(@title, @content)";
                    cmd.Parameters.AddWithValue("title", NpgsqlDbType.Text, article.Title);
                    cmd.Parameters.AddWithValue("content", NpgsqlDbType.Text, article.Content);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                Logger.Info($"Saving existing article");
                if (Logger.IsTraceEnabled)
                {
                    Logger.Trace(article.ToString);
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = _dbConnection.NpgsqlConnection;
                    cmd.CommandText = "update article set title=@title, content=@content where id=@id";
                    cmd.Parameters.AddWithValue("title", NpgsqlDbType.Text, article.Title);
                    cmd.Parameters.AddWithValue("content", NpgsqlDbType.Text, article.Content);
                    cmd.Parameters.AddWithValue("id", NpgsqlDbType.Integer, article.Id);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        
        public static BlogArticle CreateArticleFromDbRow(DbDataReader reader)
        {
            var article = new BlogArticle(
                id: reader.GetInt32(0),
                title: reader.GetString(1),
                content: reader.GetString(2)
            );
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("Creating article: {0}", article);
            }

            return article;
        }
    }
}