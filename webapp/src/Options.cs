namespace webapp
{
    public class Options
    {
        public bool ReloadTemplates { get; set; }
        public bool DisplayExceptions { get; set; }
        public string TemplatesDirectory { get; set; }
        public string PostgresConnectionString;

        public override string ToString()
        {
            return $"PostgresConnectionString: {PostgresConnectionString}, ReloadTemplates: {ReloadTemplates}, DisplayExceptions: {DisplayExceptions}, TemplatesDirectory: {TemplatesDirectory}";
        }
    }
}