using HtmlAgilityPack;

namespace AssessmentExportParser.Markdown.Converters
{
	public class Drop: ConverterBase
	{
		public Drop(Converter converter)
			: base(converter)
		{
		}

		public override string Convert(HtmlNode node)
		{
			return "";
		}
	}
}
