using System;

using CommonMark;

using Newtonsoft.Json;

namespace AssessmentExportParser.Json
{
    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class OptionModel
    {
        [JsonProperty]
        public string Text { get; set; }

        public string TextHtml => CommonMarkConverter.Convert(Text);

        [JsonProperty]
        public int Points { get; set; }
    }
}