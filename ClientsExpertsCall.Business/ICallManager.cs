using ClientsExpertsCall.Model;

namespace ClientsExpertsCall.Business
{
    public interface ICallManager
    {
        Task<List<ExpertCallPrice>> CalculateCallPriceAsync(string clientsUrl, string expertsUrl, string ratesUrl);
    }
}
