using AutoMapper;
using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Data.App.Models.Dtos;
using InventoryManager.API.Data.App.Models.Entities;
using InventoryManager.API.Handlers;
using InventoryManager.API.Providers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementController(
        IMovementService entityService, ITenantProvider tenantProvider, 
        IRequestHandler<Movement> requestHandler, IMapper mapper) :
        EntityController<Movement, MovementDto>(entityService, tenantProvider, requestHandler, mapper)
    {
    }
}
