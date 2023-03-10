using SynchronousShops.Integration.Tests.Extensions;
using SynchronousShops.Servers.API.Controllers.Identity.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests.Controllers.Identity
{
    [Collection(Constants.TEST_COLLECTION)]
    public class Role_Tests : BaseTest
    {

        public Role_Tests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Should_Get_Roles_As_Administrator()
        {
            await Client.AuthenticateAsAdministratorAsync(Output);
            var response = await Client.GetAsync(
                Libraries.Constants.Api.V1.Role.Url,
                Output
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dto = await response.ConvertToAsync<IEnumerable<RoleDto>>(Output);
            Assert.Equal(3, dto.Count());
        }
    }
}
