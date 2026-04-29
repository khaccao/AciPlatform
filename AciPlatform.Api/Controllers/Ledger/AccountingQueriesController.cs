using AciPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AciPlatform.Api.Controllers.Ledger
{
    [Authorize]
    [ApiController]
    [Route("api/v1/accounting")]
    public class AccountingQueriesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        public AccountingQueriesController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("ledgers")]
        public async Task<IActionResult> GetLedgers([FromQuery] int year = 2026, [FromQuery] int isInternal = 1)
        {
            var ledgers = await _context.LedgerEntries
                .Where(x => x.Year == year && x.IsInternal == isInternal)
                .OrderByDescending(x => x.BookDate)
                .ThenByDescending(x => x.Id)
                .Take(100) // limit for demo
                .ToListAsync();

            return Ok(ledgers);
        }

        [HttpGet("chart-of-accounts")]
        public async Task<IActionResult> GetChartOfAccounts([FromQuery] int year = 2026, [FromQuery] int isInternal = 1)
        {
            var accounts = await _context.ChartOfAccounts
                .Where(x => x.Year == year)
                .OrderBy(x => x.Code)
                .ToListAsync();

            return Ok(accounts);
        }
    }
}
