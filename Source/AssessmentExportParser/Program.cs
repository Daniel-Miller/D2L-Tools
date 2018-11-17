using System.Configuration;
using System.IO;

namespace AssessmentExportParser
{
    internal class Program
    {
        private static void Main()
        {
            var path = ConfigurationManager.AppSettings["AssessmentExportPath"];
            var file = ConfigurationManager.AppSettings["AssessmentFileName"];
            var images = ConfigurationManager.AppSettings["AssessmentImageRoot"];

            var folders = Directory.GetDirectories(path);

            foreach (var folder in folders)
            {
                var parser = new XmlFileParser();
                parser.Parse(Path.Combine(folder, file), images);
            }
        }
    }
}