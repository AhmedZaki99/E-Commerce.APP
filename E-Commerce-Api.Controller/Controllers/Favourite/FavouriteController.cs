using E_Commerce.APIs.Controllers.Base;
using E_Commerce.App.Application.Abstruction.Models.Favourite;
using E_Commerce.App.Application.Abstruction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Api.Controller.Controllers.Favourite
{
    [Authorize]
    public class FavouriteController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddFavourite(FavouriteItemDto item)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email)!;

            var result = await serviceManager.FavouriteService.AddFavouriteAsync(buyerEmail, item);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email)!;

            var result = await serviceManager.FavouriteService.GetFavouriteAsync(buyerEmail);

            return Ok(result);
        }

        [HttpGet("category")]
        public async Task<ActionResult> GetByCategory(string category)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email)!;

            var result = await serviceManager.FavouriteService.GetFavouriteByCategoryAsync(buyerEmail, category);

            return Ok(result);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> RemoveProduct(int productId)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email)!;

            var result = await serviceManager.FavouriteService.RemoveProductAsync(buyerEmail, productId);

            return Ok(result);
        }
    }
}
