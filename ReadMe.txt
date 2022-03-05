#Brief Description:

This solution has 4 projects
1.PaymentGaywayApi
2.BankSimulator
3.Common
4.UnitTest

Paymentgatewayapi has two endpoint
1) Payment endpoint 
Method:  HttpPost
Url: <BaseUrl>/api/v1.0/payment
RequestBody:
{
  "merchantId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "payment": {
    "nameOnCard": "Mike",
    "cardNumber": "1111-2222-3333-4444", 
    "cvv": 123, 
    "amount": 120,
    "currency": "US", ["US", "EURO", "POUND"]
    "expiryMonthYear": "122022"
  }
}

Response:
400:

200:
{
  "status": "Approved",
  "message": "Ok"
}

2) GetPayments Endpoint

Method: HttpGet
URl: <BaseUrl>/api/v1.0/payment/{id}

Response 200:
{
  "merchantId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "payments": [
    {
      "merchantPaymentId": "00000000-0000-0000-0000-000000000000",
      "payment": {
        "nameOnCard": "Mike",
        "cardNumber": "XXXX-XXXX-XXXX-4444",
        "cvv": 999,
        "amount": 120,
        "currency": "US",
        "expiryMonthYear": "122022"
      },
      "status": "Approved"
    }
  ]
}

2) Second project is just a bank simulator which takes the request from paymentgatewayapi and reponse accordingly.

3) Common projects for some common classes between paymentgateway and banksimulator.

4) UnitTests
Its a nunit project with FakeItEasy to fakethings
Currently only two classes are tested not all functionality is tested

How to run?

1.You need to have the running instance of Bank simulator
2.Add Url of BankSimulator on appsettings of paymentgatewayapi
eg: 
  "BankSimulator": {
    "BaseUrl": "https://localhost:7227"
  }

TODO:
//Api is missing authentication, I haven't configured it due to time crunch.
Ideally protected with JWT token can be used any oauth provider.
//containerising will be great as it's kind of microservice
//Use of secret management once api are token protected.
//currently repository is in memory it can be replaced with actual database

	