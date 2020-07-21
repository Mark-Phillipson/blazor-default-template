using System.Collections.Generic;
using MSPApplication.Shared;

namespace MSPApplication.Api.Models
{
    public interface ISurveyRepository
    {
        void AddAnswer(Answer answer);
        Survey AddSurvey(Survey survey);
        IEnumerable<Survey> GetAllSurveys();
        Survey GetSurveyById(int id);
    }
}