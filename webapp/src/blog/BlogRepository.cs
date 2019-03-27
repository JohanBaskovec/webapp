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
        private const int ArticlesPerPage = 5;

        private readonly PgsqlConnection _dbConnection;

        public BlogRepository(PgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<BlogArticle> GetAll(int page)
        {
            Logger.Info("Getting all articles.");
            int offset = page * ArticlesPerPage;
            List<BlogArticle> ret = new List<BlogArticle>();
            using (DbCommand cmd = _dbConnection.CreateCommand())
            {
                cmd.CommandText =
                    $"select id, title, content from article  order by id desc limit {ArticlesPerPage} offset {offset}";
                cmd.Prepare();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BlogArticle article = CreateArticleFromDbRow(reader);
                        ret.Add(article);
                    }
                }
            } 

            return ret;
        }

        public int CountPages()
        {
            using (DbCommand cmd = _dbConnection.CreateCommand())
            {
                cmd.CommandText = $"select count(*) from article";
                cmd.Prepare();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    int nRows = reader.GetInt32(0);
                    int nPages = (nRows / ArticlesPerPage) + 1;

                    if (nPages != 1)
                    {
                        int articlesOnLastPage = nRows % ArticlesPerPage;
                        if (articlesOnLastPage == 0)
                        {
                            nPages -= 1;
                        }
                    }

                    return nPages;
                }
            }
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
            int idColumnId = reader.GetOrdinal("id");
            int titleColumnId = reader.GetOrdinal("title");
            int contentColumnId = reader.GetOrdinal("content");
            var article = new BlogArticle(
                id: reader.GetInt32(idColumnId),
                title: reader.GetString(titleColumnId),
                content: reader.GetString(contentColumnId)
            );
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("Creating article: {0}", article);
            }

            return article;
        }
    }
}