using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using NodeBlock.Engine.API.Services;
using NodeBlock.Engine.API.Entities;
using NodeBlock.Engine.Interop.Plugin;
using NodeBlock.Engine.Storage.MariaDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace NodeBlock.Engine.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WalletsController : ControllerBase
    {
        private IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [AllowAnonymous]
        [HttpPost("informations")]
        public async Task<IActionResult> GetWalletInformations([FromBody] Wallet walletParam)
        {
            var wallet = await _walletService.GetWalletInformations(walletParam.IdentifierId);

            if (wallet == null)
                return BadRequest(new { message = "invalid wallet identifier" });

            return Ok(wallet);
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GenerateNewPersonalWallet([FromBody] Wallet walletParam)
        {
            var plugin = PluginManager.FetchPluginByName("Ethereum");

            using (var scope = GraphsContainer.GetServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<MariaDBStorage>();

                return Ok(new
                {

                });
            }
        }
    }
}
