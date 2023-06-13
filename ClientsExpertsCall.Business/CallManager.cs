using ClientsExpertsCall.Model;
using System.Text.Json;

namespace ClientsExpertsCall.Business
{
    public class CallManager : ICallManager
    {
        public async Task<List<ExpertCallPrice>> CalculateCallPriceAsync(string clientsUrl, string expertsUrl, string ratesUrl)
        {
            var expertCallPriceList = new List<ExpertCallPrice>();

            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync(clientsUrl);
            // Get Calls
            ClientRoot clients = JsonSerializer.Deserialize<ClientRoot>(response);

            string responseExperts = await client.GetStringAsync(expertsUrl);
            // Get Experts
            ExpertRoot experts = JsonSerializer.Deserialize<ExpertRoot>(responseExperts);

            string responseRates = await client.GetStringAsync(ratesUrl);
            // Get Rates
            Rate rates = JsonSerializer.Deserialize<Rate>(responseRates);


            foreach (var expert in experts.experts)
            {
                var expertCallPrice = new ExpertCallPrice();
                expertCallPrice.Name = expert.name;
                var callPriceList = new List<CallPrice>();

                var isCallWithSameExpert = false;
                var clientName = new List<string>();


                foreach (var call in expert.calls)
                {
                    var callPrice = new CallPrice();
                    callPrice.client = call.client;
                    callPrice.duration = call.duration;

                    // Calculate intervals
                    var callIntervalsBefore60MinDuration = 0;
                    var callIntervalsAfter60MinDuration = 0;
                    decimal price = 0.0M;

                    if (clientName.Exists(x=> x == call.client))
                    {
                        isCallWithSameExpert = true;
                    }
                    else
                    {
                        clientName.Add(call.client);
                    }

                    if (call.duration <= 60)
                    {
                        callIntervalsBefore60MinDuration = call.duration / 30;
                    }
                    else
                    {
                        callIntervalsBefore60MinDuration = 2;
                        callIntervalsAfter60MinDuration = (call.duration - 60) / 30;
                    }

                    // Calculate total call minutes with/ grace period
                    var callMinutes = (callIntervalsBefore60MinDuration * 30) + (callIntervalsAfter60MinDuration * 15) - ((callIntervalsBefore60MinDuration + callIntervalsAfter60MinDuration - 1) * 5);

                    price = ((decimal)callMinutes / 60M) * expert.hourlyRate;

                    // Check for discounts

                    var discounts = clients?.clients.FirstOrDefault(x => x.name == call.client)?.discounts;

                    if (discounts != null)
                    {
                        foreach (var discount in discounts)
                        {
                            if (discount == "1 hour agreement" && call.duration > 30 && call.duration < 60)
                            {
                                price = (45M / 60M) * expert.hourlyRate;
                            }

                            if (discount == "Follow" && isCallWithSameExpert)
                            {
                                price -= (price * 20) / 100;
                            }
                        }
                    }

                    // Convert to expert's currency
                    if (expert.currency != "USD")
                    {
                        switch(expert.currency)
                        {
                            case "AUD":
                                price = price * (1/rates.AUD.USD);
                                break;

                            case "CAD":
                                price = price * (1/rates.CAD.USD);
                                break;

                            case "EUR":
                                price = price * (1/rates.EUR.USD);
                                break;

                            case "GBP":
                                price = price * (1/rates.GBP.USD);
                                break;

                            case "JPY":
                                price = price * (1/rates.JPY.USD);
                                break;
                        }
                    }

                    callPrice.price = price;

                    callPriceList.Add(callPrice);
                }

                expertCallPrice.CallPrice = callPriceList;
                expertCallPriceList.Add(expertCallPrice);
            }

            return expertCallPriceList;
        }
    }
}