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
using NodeBlock.Engine.Interop.Plugin;

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

        [HttpPost("create")]
        public async Task<IActionResult> GenerateNewPersonalWallet([FromBody] ManagedWallet walletParam)
        {
            var wallet = await _walletService.GenerateNewManagedWallet(walletParam.WalletId, walletParam.WalletName);
            return Ok(wallet);
        }
    }
}
