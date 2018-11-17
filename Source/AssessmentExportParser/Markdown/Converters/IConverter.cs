using HtmlAgilityPack;

namespace AssessmentExportParser.Markdown.Converters
{
	public interface IConverter
	{
		string Convert(HtmlNode node);
	}
}
