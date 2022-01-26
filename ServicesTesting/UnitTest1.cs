using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer;
using RepoLayer;
using EFWebSiteTest;
using EFWebSiteTest.Controllers;
using System.Threading.Tasks;

namespace ServicesTesting
{
    [TestClass]
    public class UnitTest1
    {
        private BrandRepo repo;

        public UnitTest1(BrandRepo repo) 
        {
            this.repo = repo;
        }

        [TestMethod]
        public void TestMethod1()
        {
            //var x =  repo.GetBrandPageAsync(1, 2);
            //EntityPage<BrandSelect> page =  brandService.GetPageAsync(1, 5);
            //int x = 0;
            //Assert.IsNotNull(x);

            //var x = page.ListEntities.GetEnumerator();
            //int num = 0;
            //while (x.MoveNext())
            //    num++;
            //Assert.AreEqual(5, num);
        }
    }
}
