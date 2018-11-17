using System;

using HtmlAgilityPack;

namespace AssessmentExportParser.Markdown.Converters
{
	public class Br: ConverterBase
	{
		public Br(Converter converter)
			: base(converter)
		{
			Converter.Register("br", this);
		}

		public override string Convert(HtmlNode node)
		{
			return "  " + Environment.NewLine;
		}
	}
}
