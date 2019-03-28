using System;
using System.IO;
using System.Net;
using System.Text;
using NLog;
using webapp.blog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace webapp
{
    static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static int Main(string[] args)
        {
            string configFilePath = "config.yml";
            if (args.Length >= 1)
            {
                configFilePath = args[0];
            }

            Options options = ReadOptionsFile(configFilePath);
            Console.WriteLine(options);

            Console.WriteLine("Initializing TemplateProvider...");
            TemplateProvider templateProvider = new TemplateProvider(
                options.TemplatesDirectory,
                options.ReloadTemplates
            );

            Console.WriteLine("Initialized TemplateProvider.");

            DbConnectionFactory dbConnectionFactory = new DbConnectionFactory(options.PostgresConnectionString);
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8085/");
            listener.Start();
            Console.WriteLine("Server started.");

            Router router = new Router();
            router.AddGetRoute(@"^/blog(\?.*|$)", BlogController.Get);
            router.AddPostRoute(@"^/blog(\?.*|$)", BlogController.Post);
            router.AddGetRoute(@"^/blog/edition(\?.*|$)", BlogController.Edit);


            while (true)
            {
                ProcessRequest(listener, templateProvider, router, options, dbConnectionFactory);
            }
        }

        private static void ProcessRequest(HttpListener listener,
            TemplateProvider templateProvider,
            Router router,
            Options options,
            DbConnectionFactory dbConnectionFactory)
        {
            HttpListenerContext originalContext = listener.GetContext();
            HttpListenerResponse originalResponse = originalContext.Response;


            RequestContext context = new RequestContext(originalContext, templateProvider, dbConnectionFactory);
            Stream output = originalResponse.OutputStream;

            try
            {
                Before(context);
                router.RouteRequestToMethod(context);

                SendResponse(context, originalResponse, output);
            }
            catch (Exception e)
            {
                originalResponse.StatusCode = 500;

                if (options.DisplayExceptions)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(e.ToString());
                    originalResponse.ContentLength64 = buffer.Length;
                    output.Write(buffer, 0, buffer.Length);
                }

                Logger.Error(e);
            }
            finally
            {
                context.Dispose();
                output.Close();
            }
        }

        private static void Before(RequestContext context)
        {
            HttpRequest request = context.Request;
            Cookie sessionId = request.Cookies["sessionId"];
            SessionRepository sessionRepository = context.SessionRepository;
            Session session;
            if (sessionId == null)
            {
                session = CreateSession(context, sessionRepository);
            }
            else
            {
                session = sessionRepository.Get(sessionId.Value);

                // session has expired
                if (session == null)
                {
                    session = CreateSession(context, sessionRepository);
                }
            }

            context.Session = session;
        }

        private static Session CreateSession(RequestContext context, SessionRepository sessionRepository)
        {
            Session session = sessionRepository.Create();
            Cookie cookie = new Cookie {Name = "sessionId", Value = session.Id};
            DateTime maxAge = DateTime.Now.Add(new TimeSpan(90, 0, 0, 0));
            ;
            cookie.Expires = maxAge;
            cookie.HttpOnly = true;
            context.Response.SetCookie(cookie);
            return session;
        }

        private static void SendResponse(RequestContext context,
            HttpListenerResponse originalResponse,
            Stream output)
        {
            string responseString = context.Response.ResponseString;
            if (responseString == null) return;
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            originalResponse.ContentLength64 = buffer.Length;
            output.Write(buffer, 0, buffer.Length);
        }

        private static Options ReadOptionsFile(string configFilePath)
        {
            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            string optionsYamlFile = File.ReadAllText(configFilePath);
            Options options = deserializer.Deserialize<Options>(optionsYamlFile);
            return options;
        }
    }
}