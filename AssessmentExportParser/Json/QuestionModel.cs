using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Newtonsoft.Json;

namespace AssessmentExportParser.Json
{
    [Serializable]
    public class QuestionModel
    {
        #region Properties

        public int Sequence { get; set; }
        public string Text { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore), MaxLength(512)]
        public string Code { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore), MaxLength(256)]
        public string Tag { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore), MaxLength(256)]
        public string Labels { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FeedbackText { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Taxonomy { get; set; }

        public List<OptionModel> Options { get; set; }
        public List<int> Triggers { get; }

        [JsonIgnore]
        public string TextHtml => CommonMark.CommonMarkConverter.Convert(Text);

        [JsonIgnore]
        public string FeedbackHtml => CommonMark.CommonMarkConverter.Convert(FeedbackText);

        [JsonIgnore]
        public bool EnableRandomization { get; internal set; }

        [JsonIgnore]
        public int AssetFieldID { get; internal set; }

        #endregion

        public QuestionModel()
        {
            Options = new List<OptionModel>();
            Triggers = new List<int>();
        }
    }
}