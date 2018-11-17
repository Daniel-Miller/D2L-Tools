using System.Linq;

using HtmlAgilityPack;

namespace AssessmentExportParser.Markdown.Converters
{
	public class Strong: ConverterBase
	{
		public Strong(Converter converter)
			: base(converter)
		{
			Converter.Register("strong", this);
			Converter.Register("b", this);
		}

		public override string Convert(HtmlNode node)
		{
			string content = TreatChildren(node);
			if (string.IsNullOrEmpty(content.Trim()) || AlreadyBold(node))
			{
				return content;
			}
			else
			{
				return "**" + content.Trim() + "**";
			}
		}

		private bool AlreadyBold(HtmlNode node)
		{
			return node.Ancestors("strong").Count() > 0 || node.Ancestors("b").Count() > 0;
		}
	}
}
