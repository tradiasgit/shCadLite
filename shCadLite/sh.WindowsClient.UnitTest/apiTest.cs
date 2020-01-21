using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sh.WindowsClient.UnitTest
{
    [TestClass]
    public class apiTest
    {
        private string host = "https://localhost:44371";
        [TestMethod]
        public void get()
        {
            var url = $"{host}/db/dev/test/5e16a1c6563400006d005146";           
            var result = WebApiClient.GetResult(url,"GET");
            Assert.IsTrue(result.IsSuccessful);
            
        }

        [TestMethod]
        public void post()
        {
            var url = $"{host}/db/dev/test/";
            var json = "{\"test\":\"post11111\"}";
            var result = WebApiClient.GetResult(url, "POST",json);
            Assert.IsTrue(result.IsSuccessful);
        }


        [TestMethod]
        public void put()
        {
            var url = $"{host}/db/dev/test/5e16a1c6563400006d005146";
            var json = "{\"test\":\"put111111\"}";
            var result = WebApiClient.GetResult(url, "PUT",json);
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
