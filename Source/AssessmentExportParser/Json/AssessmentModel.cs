using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace AssessmentExportParser.Json
{
    [Serializable]
    public class AssessmentModel
    {
        #region Properties

        [MaxLength(256)]
        public string Title { get; set; }

        public List<QuestionModel> Questions { get; set; }

        #endregion

        #region Construction

        public AssessmentModel()
        {
            Questions = new List<QuestionModel>();
        }

        #endregion

        public void Renumber()
        {
            for (var i = 0; i < Questions.Count; i++)
                Questions[i].Sequence = i + 1;
        }
    }
}