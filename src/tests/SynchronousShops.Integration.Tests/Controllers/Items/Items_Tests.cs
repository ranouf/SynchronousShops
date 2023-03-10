using SynchronousShops.Integration.Tests.Extensions;
using SynchronousShops.Servers.API.Controllers.Dtos;
using SynchronousShops.Servers.API.Controllers.Identity.Dtos;
using SynchronousShops.Servers.API.Controllers.Items.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests.Controllers.Items
{
    [Collection(Constants.TEST_COLLECTION)]
    public class Items_Tests : BaseTest
    {
        public const string NewItem = "NewItem";
        public const string EditedItem = "EditedItem";

        public Items_Tests(
            ITestOutputHelper output
        ) : base(output) { }

        [Fact]
        public async Task Should_List_Items()
        {
            // As Admin
            await Client.AuthenticateAsAdministratorAsync(Output);

            var response = await Client.GetAsync(
                Libraries.Constants.Api.V1.Item.Url,
                Output,
                new FilterRequestDto()
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dto = await response.ConvertToAsync<List<ItemDto>>(Output);
            Assert.NotNull(dto);
            Assert.Equal(3, dto.Count);
        }

        [Fact]
        public async Task Should_Create_Update_And_Delete_Item_As_Administrator_Then_Get_Read_Delete_Notifications_As_Manager()
        {
            // As Manager
            var response = await Client.AuthenticateAsManagerAsync(Output);
            var loginResponseDto = await response.ConvertToAsync<LoginResponseDto>(Output);

            // As Admin
            await Client.AuthenticateAsAdministratorAsync(Output);

            // Create Item
            var insertItemRequest = new UpsertItemRequest()
            {
                Name = NewItem
            };

            response = await Client.PostAsync(
                 Libraries.Constants.Api.V1.Item.Url,
                 Output,
                 insertItemRequest
             );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var newItem = await response.ConvertToAsync<ItemDto>(Output);
            Assert.Equal(NewItem, newItem.Name);

            // Edit Item
            var updateItemRequest = new UpsertItemRequest()
            {
                Name = EditedItem
            };

            response = await Client.PutByIdAsync(
                Libraries.Constants.Api.V1.Item.Url,
                Output,
                newItem.Id,
                updateItemRequest
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var editedItem = await response.ConvertToAsync<ItemDto>(Output);
            Assert.Equal(EditedItem, editedItem.Name);

            // Delete Item
            response = await Client.DeleteByIdAsync(
                Libraries.Constants.Api.V1.Item.Url,
                Output,
                newItem.Id
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
