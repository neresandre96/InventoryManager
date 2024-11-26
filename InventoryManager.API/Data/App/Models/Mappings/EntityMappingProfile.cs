using AutoMapper;
using InventoryManager.API.Data.App.Models.Dtos;
using InventoryManager.API.Data.App.Models.Entities;

namespace InventoryManager.API.Data.App.Models.Mappings
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<IngredientDto, Ingredient>();
            CreateMap<Ingredient, IngredientDto>();

            CreateMap<MovementDto, Movement>();
            CreateMap<Movement, MovementDto>();

        }
    }

}
