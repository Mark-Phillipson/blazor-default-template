using System.Collections.Generic;
using MSPApplication.Shared;

namespace MSPApplication.Data.Repositories
{
    public interface ISurveyRepository
    {
        void AddAnswer(Answer answer);
        Survey AddSurvey(Survey survey);
        IEnumerable<Survey> GetAllSurveys();
        Survey GetSurveyById(int id);
    }
}