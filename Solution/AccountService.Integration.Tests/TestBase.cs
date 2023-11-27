using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Integration.Tests
{
    public  class TestBase
    {
        protected HttpClient _httpClient;
        protected TestFactory<Program> _application;

        [SetUp]
        public void Setup()
        {
            _application = new TestFactory<Program>();
            _httpClient = _application.CreateClient();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _application.DisposeAsync();
            _httpClient.Dispose();
        }

        protected async Task<string> AuthorizeUser(string username = "admin3", string password = "password3")
        {
            var login = new
            {
                loginName = username,
                password = password
            };

            var json = JsonConvert.SerializeObject(login);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/login", content);

            var resStr = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Token>(resStr);
            var token = res.access_token;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return token;
        }
    }
}
