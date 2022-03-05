using Common.Models;
using Newtonsoft.Json;
using System.Text;

namespace PaymentGatewayAPI.Helpers
{
    public class BankSimulatorClient : IBankSimulatorClient
    {
        private readonly HttpClient httpClient;
        public BankSimulatorClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<AccuringBankResponse> TakePayment(Payment payment)
        {
            string uri = "/api/Payment";
            string payloadJson = JsonConvert.SerializeObject(payment);
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            { Content = new StringContent(payloadJson, Encoding.UTF8, "application/json") };
            var response = await httpClient.SendAsync(httpRequestMessage);
            //Ensure payment is done if not it will raise exception and middleware catches it ans send 500 to user.
            response.EnsureSuccessStatusCode();
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccuringBankResponse>(jsonResult);
            return result;
        }
    }
}
