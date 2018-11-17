using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using AssessmentExportParser.Json;
using AssessmentExportParser.Markdown;

namespace AssessmentExportParser
{
    public class XmlFileParser
    {
        public void Parse(string filePath, string imageRoot)
        {
            // Attempt to parse the file using file format #1.
            var assessments = ParseFileFormatOne(filePath);

            // If that fails to produce anything then attempt to parse the file using file format #2.
            if (assessments.Count == 0)
                assessments = ParseFileFormatTwo(filePath);

            // Write each assessment to JSON and Markdown file formats.
            foreach (var assessment in assessments)
            {
                foreach (var question in assessment.Questions)
                {
                    question.Text = ResolveImagePath(question.Text, imageRoot);
                    question.FeedbackText = ResolveImagePath(question.FeedbackText, imageRoot);
                }

                var path = Path.Combine(Path.GetDirectoryName(filePath), CreateValidFileName(assessment.Title));
                File.WriteAllText(path + ".json", AssessmentWriter.WriteToJson(assessment));
                File.WriteAllText(path + ".md", AssessmentWriter.WriteToMarkdown(assessment));
            }
        }

        #region Methods (private)

        private List<AssessmentModel> ParseFileFormatTwo(string path)
        {
            var xml = XDocument.Load(path);

            var query = from assessment in xml.Element("questestinterop").Elements("objectbank")
                select new AssessmentModel
                {
                    Title = assessment.Elements("section").Attributes().First(x => x.Name == "title").Value,
                    Questions = (from section in assessment.Elements("section")
                        from item in section.Elements("item")
                        select new QuestionModel
                        {
                            Code = item.Attribute("title").Value,
                            Text =
                                HtmlToText(
                                    item.Element("presentation")
                                        .Element("flow")
                                        .Element("material")
                                        .Element("mattext")
                                        .Value),
                            FeedbackText = HtmlToText(ParseFeedback(item.Element("itemfeedback").ToString())),
                            Options = (from presentation in item.Elements("presentation").Elements("flow")
                                from material in
                                    presentation.Elements("response_lid")
                                        .Elements("render_choice")
                                        .Elements("flow_label")
                                        .Elements("response_label")
                                select new OptionModel
                                {
                                    Text = HtmlToText(
                                        material
                                            .Element("flow_mat")
                                            .Element("material")
                                            .Element("mattext").Value),
                                    Points =
                                        ParseOption(
                                            item.Element("resprocessing").ToString(),
                                            material.Attribute("ident").Value)
                                }).ToList()
                        }).ToList()
                };

            var result = query.ToList();

            foreach (var quiz in result)
                quiz.Renumber();

            return result;
        }

        private List<AssessmentModel> ParseFileFormatOne(string path)
        {
            var xml = XDocument.Load(path);

            var query = from assessment in xml.Element("questestinterop").Elements("assessment")
                select new AssessmentModel
                {
                    Title = assessment.Attribute("title").Value,
                    Questions = (from section in assessment.Elements("section")
                        from item in section.Elements("item")
                        select new QuestionModel
                        {
                            Code = item.Attribute("title")?.Value,
                            Text =
                                HtmlToText(
                                    item.Element("presentation")
                                        .Element("flow")
                                        .Element("material")
                                        .Element("mattext")
                                        .Value),
                            FeedbackText = HtmlToText(ParseFeedback(item.Element("itemfeedback").ToString())),
                            Options = (from presentation in item.Elements("presentation").Elements("flow")
                                from material in
                                    presentation.Elements("response_lid")
                                        .Elements("render_choice")
                                        .Elements("flow_label")
                                        .Elements("response_label")
                                select new OptionModel
                                {
                                    Text = HtmlToText(
                                        material
                                            .Element("flow_mat")
                                            .Element("material")
                                            .Element("mattext").Value),
                                    Points =
                                        ParseOption(
                                            item.Element("resprocessing").ToString(),
                                            material.Attribute("ident").Value)
                                }).ToList()
                        }).ToList()
                };

            var result = query.ToList();

            foreach (var quiz in result)
                quiz.Renumber();

            return result;
        }

        private string ParseFeedback(string xml)
        {
            var document = XDocument.Parse(xml);

            var query = from content in document
                    .Element("itemfeedback")
                    .Element("material")
                    .Elements("mattext")
                select content.Value;

            var items = query.ToList();
            return items.Count == 1 ? items[0] : null;
        }

        private int ParseOption(string xml, string identity)
        {
            var document = XDocument.Parse(xml);

            var query = from variable in document
                    .Element("resprocessing")
                    .Elements("respcondition")
                    .Where(x => x.Element("setvar") != null && x.Element("setvar").Value.StartsWith("1"))
                    .Elements("conditionvar")
                    .Elements("varequal")
                select variable.Value;

            var items = query.ToList();
            if (items.Count == 1 && items[0] == identity)
                return 1;

            return 0;
        }

        private string CreateValidFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '-');

            return name;
        }

        private string HtmlToText(string html)
        {
            var converter = new Converter();
            var text = converter.Convert(html).Trim();
            text = Regex.Replace(text, @"(\r\n){2,}", "\r\n");
            text = Regex.Replace(text, @"!\[.*\]\(.*/(.+\.gif|jpg|png)", "![image]($1");
            return text.Replace("&nbsp;", " ");
        }

        private string ResolveImagePath(string text, string imageRoot)
        {
            const string pattern = @"!\[image\]\(([a-z0-9_-]+\.gif|jpg|png)\s+";
            var replacement = $@"![image]({imageRoot}$1 ";
            return Regex.Replace(text, pattern, replacement);
        }

        #endregion
    }
}