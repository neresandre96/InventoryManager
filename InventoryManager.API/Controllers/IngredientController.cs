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
    public class IngredientController(
        IIngredientService entityService, ITenantProvider tenantProvider, 
        IRequestHandler<Ingredient> requestHandler, IMapper mapper) :
        EntityController<Ingredient, IngredientDto>(entityService, tenantProvider, requestHandler, mapper)
    {
    }

}
