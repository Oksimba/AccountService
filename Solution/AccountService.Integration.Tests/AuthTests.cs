using System.Text;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace AccountService.Integration.Tests
{
    [TestFixture]
    public class AuthModuleTests : TestBase
    {
        [Test]
        public async Task LoginWithCorrectCreds_ShouldBeSuccessful()
        {
            var login = new
            {
                loginName = "user3",
                password = "password3"
            };
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/login", content);

            response.Should().BeSuccessful();

            var resStr = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Token>(resStr);

            res.access_token.Should().NotBeEmpty();

        }

        [Test]
        public async Task LoginWithWrongCreds_ShouldBeUnauthorized()
        {
            var login = new
            {
                loginName = "user3",
                password = "wrongpass"
            };
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/login", content);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }



    }

    public class Token
    {
        public string access_token { get; init; }
    }
}