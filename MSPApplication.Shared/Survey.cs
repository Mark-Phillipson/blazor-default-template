using System;
using System.Collections.Generic;
using System.Text;

namespace MSPApplication.Shared
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<Answer> Answers { get; set; }
    }
}
