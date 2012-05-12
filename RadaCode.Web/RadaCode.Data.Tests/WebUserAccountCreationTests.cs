using NUnit.Framework;
using RadaCode.Web.Data.EF;
using RadaCode.Web.Data.Repositories;

namespace RadaCode.Data.Tests
{
    [TestFixture]
    public class WebUserAccountCreationTests
    {
        private WebUserRepository _repo;
        private RadaCodeWebStoreContext _context;

        public WebUserAccountCreationTests()
        {
            var context = new RadaCodeWebStoreContext();

            var initializer = new RadaCodeWebStoreContextInitializer();
            initializer.InitializeDatabase(context);

            _repo = new WebUserRepository(context);

            _context = context;
        }

        [Test]
        public void CreateRoleAndUserTest()
        {
            _repo.AddRole("Star");
            var user = _repo.CreateUser("Max Pavlov", "q1w2e3", "max@radacode.com");

            _repo.AddRoleToUser(user.Id, "Star");

            var exists = _repo.UserExists(user);
            var isInRole = _repo.GetRolesForUser(user)[0].RoleName == "Star";

            Assert.IsTrue(exists);
            Assert.IsTrue(isInRole);
        }

        [Test]
        public void IsUserInRole()
        {
            _repo = new WebUserRepository(_context);

            var inRole = (_repo.GetRolesForUser("Max Pavlov")[0] != null &&
                          _repo.GetRolesForUser("Max Pavlov")[0].RoleName == "Star");

            Assert.IsTrue(inRole);
        }
    }
}
