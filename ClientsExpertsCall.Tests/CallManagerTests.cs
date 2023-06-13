using ClientsExpertsCall.Business;

namespace ClientsExpertsCall.Tests
{
    public class CallManagerTests
    {
        string clientsUrl = string.Empty;
        string expertsUrl = string.Empty;
        string ratesUrl = string.Empty;

        [SetUp]
        public void Setup()
        {
            clientsUrl = "https://arbolus-tests-default-rtdb.firebaseio.com/clients.json";
            expertsUrl = "https://arbolus-tests-default-rtdb.firebaseio.com/experts.json";
            ratesUrl = "https://arbolus-tests-default-rtdb.firebaseio.com/rates.json";
        }

        [Test]
        public void CalculateCallPriceAsync_Success()
        {
            // Arrange
            CallManager callManager = new CallManager();

            // Act
            var expertCallPriceList = callManager.CalculateCallPriceAsync(clientsUrl, expertsUrl, ratesUrl);

            // Assert
            Assert.IsNotNull(expertCallPriceList);
        }
    }
}