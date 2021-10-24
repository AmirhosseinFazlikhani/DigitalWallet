using DigitalWallet.Application.Base;
using DigitalWallet.Application.Inputs;
using DigitalWallet.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DigitalWallet.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly GetService _getService;
        private readonly CreateService _createService;
        private readonly DepositService _depositService;
        private readonly WithdrawService _withdrawService;

        public WalletController(
            GetService getService,
            CreateService createService,
            DepositService depositService,
            WithdrawService withdrawService)
        {
            _getService = getService;
            _createService = createService;
            _depositService = depositService;
            _withdrawService = withdrawService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _getService.GetAsync(id);

            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound();
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInput input)
        {
            var result = await _createService.CreateAsync(input);

            if (result.IsSuccess)
            {
                return Created(Url.Action("Get", new { Id = result.Value }), result.Value);
            }

            return BadRequest(result.ErrorCode);
        }

        [HttpPatch("{id}/deposit")]
        public async Task<IActionResult> Deposit(Guid id, DepositOrWithdrawInput input)
        {
            var result = await _depositService.DepositAsync(id, input);

            if (result.IsSuccess)
            {
                return Accepted();
            }

            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCode);
        }

        [HttpPatch("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(Guid id, DepositOrWithdrawInput input)
        {
            var result = await _withdrawService.WithdrawAsync(id, input);

            if (result.IsSuccess)
            {
                return Accepted();
            }

            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCode);
        }
    }
}
