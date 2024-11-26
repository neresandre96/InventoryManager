using AutoMapper;
using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Data.App.Models.Types;
using InventoryManager.API.Data.App.Services;
using InventoryManager.API.Handlers;
using InventoryManager.API.Providers;
using InventoryManager.API.Providers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace InventoryManager.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthProvider))]
    public class EntityController<TEntity, YDto>(
    IEntityService<TEntity> entityService, ITenantProvider tenantProvider,
    IRequestHandler<TEntity> requestHandler, IMapper mapper) :
    ControllerBase
    where TEntity : class
    where YDto : class
    {
        private readonly IRequestHandler<TEntity> _requestHandler = requestHandler;
        private readonly IEntityService<TEntity> _entityService = entityService;
        private readonly ITenantProvider _tenantProvider = tenantProvider;
        private readonly IMapper _mapper = mapper;

        // GET: api/entities
        [HttpGet]
        public ActionResult<IEnumerable<TEntity>> Get([FromQuery] string[] includes, [FromQuery] string? filter = null)
        {
            SetTenantId();
            var includeExpressions = includes.Select(_requestHandler.CreateIncludeExpression).ToArray();
            Expression<Func<TEntity, bool>>? filterExpression = string.IsNullOrEmpty(filter)
                ? null
                : _requestHandler.CreateFilterExpression(filter);
            var entities = _entityService.ListAll(filterExpression, includeExpressions);
            return Ok(entities);
        }

        // GET: api/entities/name?name=someName
        [HttpGet("name")]
        public ActionResult<IEnumerable<TEntity>> GetByName([FromQuery] string name)
        {
            SetTenantId();
            var result = _entityService.CheckIfExist(name);
            return Ok(result);
        }

        // GET: api/entities/5
        [HttpGet("{id}")]
        public ActionResult<TEntity> GetById(int id)
        {
            SetTenantId();
            var entity = _entityService.GetById(id);
            return entity == null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public IActionResult Create([FromBody] YDto entityDto)
        {
            if (entityDto == null)
            {
                return BadRequest("Invalid entity data.");
            }

            var entity = _mapper.Map<TEntity>(entityDto);

            return HandleCrudOperation(entity, CrudActions.Add, "Could not create");
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] YDto entityDto)
        {
            if (entityDto == null)
            {
                return BadRequest("Invalid entity data.");
            }

            var entity = _entityService.GetById(id);

            if (entity == null)
            {
                return NotFound($"Entity with ID {id} not found.");
            }

            _mapper.Map(entityDto, entity);

            return HandleCrudOperation(entity, CrudActions.Update, "Could not update");
        }


        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
            => HandleCrudOperation(_entityService.GetById(id), CrudActions.Remove, "Could not delete");



        private void SetTenantId() => _entityService.SetTenantId(_tenantProvider.GetTenantId());

        private ActionResult HandleCrudOperation(TEntity entity, CrudActions actionType, string failureMessage)
        {
            SetTenantId();

            var result = _entityService.CrudOperation(entity, actionType);

            if (result == null && actionType != CrudActions.Remove)
            {
                return BadRequest(failureMessage);
            }

            return Ok(result);
        }
    }
}
