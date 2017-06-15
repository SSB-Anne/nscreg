﻿using Microsoft.AspNetCore.Mvc;
using nscreg.Data;
using nscreg.Server.Services;
using nscreg.Server.Models.StatUnits;
using nscreg.Server.Models.StatUnits.Create;
using nscreg.Server.Models.StatUnits.Edit;
using nscreg.Data.Constants;
using System.Threading.Tasks;
using nscreg.Data.Entities;
using nscreg.Server.Core;
using nscreg.Server.Core.Authorize;
using nscreg.Server.Models;
using nscreg.Server.Services.StatUnit;

namespace nscreg.Server.Controllers
{
    [Route("api/[controller]")]
    public class StatUnitsController : Controller
    {
        private readonly StatUnitService _statUnitService;
        private readonly SearchService _searchService;
        private readonly ViewService _viewService;
        private readonly CreateService _createService;
        private readonly EditService _editService;
        private readonly DeleteService _deleteService;
        private readonly LookupService _lookupService;

        public StatUnitsController(NSCRegDbContext context)
        {
            _statUnitService = new StatUnitService(context);
            _searchService = new SearchService(context);
            _viewService = new ViewService(context);
            _createService = new CreateService(context);
            _editService = new EditService(context);
            _deleteService = new DeleteService(context);
            _lookupService = new LookupService(context);
        }

        [HttpGet]
        [SystemFunction(SystemFunctions.StatUnitView)]
        public async Task<IActionResult> Search([FromQuery] SearchQueryM query)
            => Ok(await _searchService.Search(query, User.GetUserId()));

        [HttpGet("[action]")]
        [SystemFunction(SystemFunctions.StatUnitView, SystemFunctions.LinksView)]
        public async Task<IActionResult> SearchByStatId(string code)
            => Ok(await _searchService.Search(code));

        [HttpGet("[action]/{type}/{id}")]
        [SystemFunction(SystemFunctions.StatUnitView)]
        public async Task<IActionResult> History(StatUnitTypes type, int id)
            => Ok(await _statUnitService.ShowHistoryAsync(type, id));

        [HttpGet("[action]/{type}/{id}")]
        [SystemFunction(SystemFunctions.StatUnitView)]
        public async Task<IActionResult> HistoryDetails(StatUnitTypes type, int id)
            => Ok(await _statUnitService.ShowHistoryDetailsAsync(type, id, User.GetUserId()));

        [HttpGet("[action]/{type}")]
        [SystemFunction(SystemFunctions.StatUnitView)]
        public async Task<IActionResult> GetStatUnits(StatUnitTypes type)
            => Ok(await _lookupService.GetLookupByType(type));

        [HttpGet("[action]/{type}")]
        [SystemFunction(SystemFunctions.StatUnitCreate)]
        public async Task<IActionResult> GetNewEntity(StatUnitTypes type)
            => Ok(await _statUnitService.GetViewModel(null, type, User.GetUserId()));

        [HttpGet("[action]/{type}/{id}")]
        [SystemFunction(SystemFunctions.StatUnitView)]
        public async Task<IActionResult> GetUnitById(StatUnitTypes type, int id)
            => Ok(await _statUnitService.GetViewModel(id, type, User.GetUserId()));

        [HttpGet("{type:int}/{id}")]
        [SystemFunction(SystemFunctions.StatUnitView)]
        public async Task<IActionResult> GetEntityById(StatUnitTypes type, int id)
            => Ok(await _viewService.GetUnitByIdAndType(id, type, User.GetUserId(), true));

        [HttpDelete("{type}/{id}")]
        [SystemFunction(SystemFunctions.StatUnitDelete)]
        public IActionResult Delete(StatUnitTypes type, int id)
        {
            _deleteService.DeleteUndelete(type, id, true, User.GetUserId());
            return NoContent();
        }

        [HttpPost(nameof(LegalUnit))]
        [SystemFunction(SystemFunctions.StatUnitCreate)]
        public async Task<IActionResult> CreateLegalUnit([FromBody] LegalUnitCreateM data)
        {
            await _createService.CreateLegalUnit(data, User.GetUserId());
            return NoContent();
        }

        [HttpPost(nameof(LocalUnit))]
        [SystemFunction(SystemFunctions.StatUnitCreate)]
        public async Task<IActionResult> CreateLocalUnit([FromBody] LocalUnitCreateM data)
        {
            await _createService.CreateLocalUnit(data, User.GetUserId());
            return NoContent();
        }

        [HttpPost(nameof(EnterpriseUnit))]
        [SystemFunction(SystemFunctions.StatUnitCreate)]
        public async Task<IActionResult> CreateEnterpriseUnit([FromBody] EnterpriseUnitCreateM data)
        {
            await _createService.CreateEnterpriseUnit(data, User.GetUserId());
            return NoContent();
        }

        [HttpPost(nameof(EnterpriseGroup))]
        [SystemFunction(SystemFunctions.StatUnitCreate)]
        public async Task<IActionResult> CreateEnterpriseGroup([FromBody] EnterpriseGroupCreateM data)
        {
            await _createService.CreateEnterpriseGroupUnit(data, User.GetUserId());
            return NoContent();
        }

        [HttpPut(nameof(LegalUnit))]
        [SystemFunction(SystemFunctions.StatUnitEdit)]
        public async Task<IActionResult> EditLegalUnit([FromBody] LegalUnitEditM data)
        {
            await _editService.EditLegalUnit(data, User.GetUserId());
            return NoContent();
        }

        [HttpPut(nameof(LocalUnit))]
        [SystemFunction(SystemFunctions.StatUnitEdit)]
        public async Task<IActionResult> EditLocalUnit([FromBody] LocalUnitEditM data)
        {
            await _editService.EditLocalUnit(data, User.GetUserId());
            return NoContent();
        }

        [HttpPut(nameof(EnterpriseUnit))]
        [SystemFunction(SystemFunctions.StatUnitEdit)]
        public async Task<IActionResult> EditEnterpriseUnit([FromBody] EnterpriseUnitEditM data)
        {
            await _editService.EditEnterpiseUnit(data, User.GetUserId());
            return NoContent();
        }

        [HttpPut(nameof(EnterpriseGroup))]
        [SystemFunction(SystemFunctions.StatUnitEdit)]
        public async Task<IActionResult> EditEnterpriseGroup([FromBody] EnterpriseGroupEditM data)
        {
            await _editService.EditEnterpiseGroup(data, User.GetUserId());
            return NoContent();
        }

        [HttpGet("[action]")]
        [SystemFunction(SystemFunctions.StatUnitView)]
        public async Task<IActionResult> AnalyzeRegister([FromQuery] PaginationModel model)
            => Ok(await _statUnitService.GetInconsistentRecordsAsync(model));
    }
}
