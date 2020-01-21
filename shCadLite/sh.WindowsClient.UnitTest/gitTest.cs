using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sh.WindowsClient.UnitTest
{
    [TestClass]
    public class gitTest
    {
        [TestMethod]
        public void get()
        {
            var c =new sh.Repository.Class1();
            c.Init();
        }

      
    }
}
