using Moq;
using Moq.Protected;
using MSPApplication.Shared;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject.Services
{
    public class UnitTestUserDataService
    {
        private string usersJson = "";
        private List<AspNetUser> users;
        public UnitTestUserDataService()
        {
            usersJson = LoadJson(@"services\users.json");
            users = JsonConvert.DeserializeObject<List<AspNetUser>>(usersJson);
        }

        private string LoadJson(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }

        [Fact]
        public async Task GetAllUsers_TestAsync()
        {
            // ARRANGE
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(usersJson),
               })
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/83"),
            };

            UserDataService sut = new UserDataService(httpClient);
            var result = (await sut.GetAllUsers()).ToList();
            Assert.NotNull(result);
            Assert.Equal("testuser@domain.co.uk", result[0].UserName);

            // also check the 'http' call was like we expected it
            var expectedUri = new Uri("http://test.com/api/user");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1), // we expected a single external request
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get  // we expected a GET request
                  && req.RequestUri == expectedUri // to this uri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
