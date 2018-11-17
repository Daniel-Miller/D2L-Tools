using System.IO;
using System.Text;
using System.Xml;

using AssessmentExportParser.Json;

using Newtonsoft.Json;

using Formatting = Newtonsoft.Json.Formatting;

namespace AssessmentExportParser
{
    public class AssessmentWriter
    {
        public static string WriteToJson(AssessmentModel exam)
        {
            var sb = new StringBuilder();
            var text = new StringWriter(sb);

            var writer = new JsonTextWriter(text);
            var serializer = new JsonSerializer { Formatting = Formatting.Indented };
            serializer.Serialize(writer, exam);
            text.Close();

            return sb.ToString();
        }

        public static string WriteToMarkdown(AssessmentModel exam)
        {
            var sb = new StringBuilder();

            var i = 0;
            foreach (var question in exam.Questions)
            {
                sb.AppendLine("---");
                sb.AppendLine();
                sb.AppendFormat("# Question #{0}", ++i);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine(question.Text);
                sb.AppendLine();

                if (question.Options.Count > 0)
                {
                    sb.AppendLine("## Options");
                    sb.AppendLine();

                    foreach (var option in question.Options)
                    {
                        sb.AppendFormat("- {0}", option.Points > 0 ? "*" + option.Text + "*" : option.Text);
                        sb.AppendLine();
                    }

                    sb.AppendLine();
                }

                if (!string.IsNullOrEmpty(question.FeedbackText))
                {
                    sb.AppendLine("## Feedback");
                    sb.AppendLine();
                    sb.AppendLine(question.FeedbackText);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        public static string WriteToXml(AssessmentModel exam)
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings {Indent = true};

            var writer = XmlWriter.Create(sb, settings);
            writer.WriteStartElement("questions");

            var i = 0;
            foreach (var question in exam.Questions)
            {
                writer.WriteStartElement("question");
                writer.WriteAttributeString("number", (++i).ToString());
                writer.WriteAttributeString("code", question.Code);
                writer.WriteElementString("text", question.Text);
                writer.WriteStartElement("options");

                foreach (var option in question.Options)
                {
                    writer.WriteStartElement("option");
                    if (option.Points > 0)
                        writer.WriteAttributeString("score", option.Points.ToString());
                    writer.WriteValue(option.Text);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteElementString("feedback", question.FeedbackText);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

            return sb.ToString();
        }
    }
}