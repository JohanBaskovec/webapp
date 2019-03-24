using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace webapp
{
    public class TemplateProvider
    {
        private readonly Dictionary<string, Template> _templatesCache = new Dictionary<string, Template>();
        private readonly bool _reloadTemplates;
        private readonly string _templateFolder;
        private readonly TemplateLoader _templateLoader;

        internal TemplateProvider(string templateFolder, bool reloadTemplates)
        {
            _templateLoader = new TemplateLoader(this);
            _reloadTemplates = reloadTemplates;
            _templateFolder = templateFolder;
        }

        internal Template Get(string templateName)
        {
            if (_reloadTemplates)
            {
                return ParseTemplate(templateName);
            }

            if (_templatesCache.ContainsKey(templateName))
            {
                return _templatesCache[templateName];
            }

            Template template = ParseTemplate(templateName);
            _templatesCache.Add(templateName, template);
            return template;
        }

        public string Render(string templateName, ScriptObject model)
        {
            Template template = Get(templateName);
            TemplateContext context = new TemplateContext
                {TemplateLoader = _templateLoader, MemberRenamer = member => member.Name};
            context.PushGlobal(model);
            return template.Render(context);
        }

        private Template ParseTemplate(string templateName)
        {
            return Template.Parse(File.ReadAllText(GetPath(templateName)));
        }

        internal string GetPath(string templateName)
        {
            return _templateFolder + "/" + templateName;
        }
    }

    /**
     * This is used to load templates from
     * include statements in Scriban files.
     */
    class TemplateLoader : ITemplateLoader
    {
        private readonly TemplateProvider _templateProvider;

        public TemplateLoader(TemplateProvider templateProvider)
        {
            _templateProvider = templateProvider;
        }

        public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
        {
            return _templateProvider.GetPath(templateName);
        }

        public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
        {
            // TODO: cache files content.
            // Scriban uses a cache in the TemplateContext, but because
            // we recreate a new TemplateContext on every call to TemplateProvider.Render,
            // it actually loads the file from the hard drive on every render!
            return File.ReadAllText(templatePath);
        }

        public ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
        {
            return new ValueTask<string>(File.ReadAllText(templatePath));
        }
    }
}