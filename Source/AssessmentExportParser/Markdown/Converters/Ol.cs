using System;

using HtmlAgilityPack;

namespace AssessmentExportParser.Markdown.Converters
{
	public class Ol: ConverterBase
	{
		public Ol(Converter converter)
			: base(converter)
		{
			Converter.Register("ol", this);
			Converter.Register("ul", this);
		}

		public override string Convert(HtmlNode node)
		{
			return Environment.NewLine + TreatChildren(node);
		}
	}
}
