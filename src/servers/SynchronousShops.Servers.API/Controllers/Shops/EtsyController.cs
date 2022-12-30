using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SynchronousShops.Domains.Core.Identity;
using SynchronousShops.Domains.Core.Identity.Extensions;
using SynchronousShops.Domains.Core.Session;
using SynchronousShops.Domains.Core.Shops.Etsy;
using SynchronousShops.Libraries.Constants;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Servers.API.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Controllers.Shops
{
    [ApiController]
    [Route(Api.V1.Etsy.Url)]
    public class EtsyController : AuthentifiedBaseController
    {
        private readonly IEtsyShopService _etsyShopService;

        public EtsyController(
            IEtsyShopService etsyShopService,
            IUserSession session,
            IUserManager userManager,
            IMapper mapper,
            ILogger<EtsyController> logger
        ) : base(session, userManager, mapper, logger)
        {
            _etsyShopService = etsyShopService;
        }



        [HttpGet]
        [ProducesResponseType(typeof(Uri), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route(Api.V1.Etsy.RequestAuthorizationCodeUrl)]
        [Authorize]
        public async Task<IActionResult> GetRequestAnAuthorizationCodeLinkAsync()
        {
            var currentUser = Session.CurrentUser;
            Logger.LogInformation($"{nameof(GetRequestAnAuthorizationCodeLinkAsync)}, current:{currentUser.ToJson()}.");
            var (link, codeVerifier, state)= _etsyShopService.GenerateRequestAnAuthorizationCodeUri();
            currentUser.SetEtsyCodeVerifier(codeVerifier);
            currentUser.SetEtsyState(state);
            await _userManager.UpdateAsync(currentUser);
            return new ObjectResult(link);
        }

        [HttpGet]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route(Api.V1.Etsy.Ping)]
        public async Task<IActionResult> PingAsync()
        {
            Logger.LogInformation($"{nameof(PingAsync)}.");
            var result = await _etsyShopService.PingAsync();
            return new ObjectResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route(Api.V1.Etsy.Redirect)]
        public async Task<IActionResult> RedirectAsync(
            [FromQuery] string code,
            [FromQuery] string state
        )
        {
            // Get User
            var currentUser = await _userManager.FindByEtsyStateAsync(state);
            if (currentUser == null)
            {
                throw new KeyNotFoundException($"No user found with state:{state}.");
            }
            Logger.LogInformation($"{nameof(RedirectAsync)}, current:{currentUser.ToJson()}.");

            // Get OAuth Token
            var token = await _etsyShopService.RequestAccessTokenAsync(
                code,
                currentUser.GetEtsyCodeVerifier()
            );
            currentUser.SetEtsyOAuthToken(token);

            // Get User Information
            var me = await _etsyShopService.GetMeAsync(token);
            currentUser.SetEtsyUserId(me.UserId);
            currentUser.SetEtsyShopId(me.ShopId);
            await _userManager.UpdateAsync(currentUser);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [AuthorizeAdministrators]
        [Route(Api.V1.Etsy.Me)]
        public async Task<IActionResult> MeAsync()
        {
            var currentUser = Session.CurrentUser;
            var shopId = currentUser.GetEtsyShopId();
            var result = await _etsyShopService.GetAllListingsAsync(
                shopId,
                25,
                0
            );
            return Ok();
        }
    }
}
