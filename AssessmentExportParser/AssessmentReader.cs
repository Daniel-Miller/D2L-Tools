using System.IO;

using AssessmentExportParser.Json;

using Newtonsoft.Json;

namespace AssessmentExportParser
{
    public class AssessmentReader
    {
        public static AssessmentModel ReadFromJson(string path)
        {
            var text = File.ReadAllText(path);

            var quiz = JsonConvert.DeserializeObject<AssessmentModel>(text);

            return quiz;
        }
    }
}