using Inferno.src.Adapters.Inbound.Controllers.Model;
using Inferno.src.Core.Domain.Interfaces.UseCases.Category;
using Microsoft.AspNetCore.Mvc;

namespace Inferno.src.Adapters.Inbound.Controllers.Category
{
    [Route("api/[controller]")]
    public class CategoryController(
        ILogger<CategoryController> logger,
        ICategoryUseCase categoryUseCase
    ) : Controller
    {
        private readonly ILogger<CategoryController> _logger = logger;
        private readonly ICategoryUseCase _categoryUseCase = categoryUseCase;
        private const string InvalidInputMessage = "Invalid input provided";

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? pageSize,
            [FromQuery] int? pageNumber
        )
        {
            _logger.LogInformation("received request to get all Categories");
            var (response, message) = await _categoryUseCase.ListAllCategory(pageSize, pageNumber);
            _logger.LogInformation(
                "sucessfuly retrieved:{ResponseCount} categories",
                response!.Count
            );
            return Ok(
                new APIResponse<List<CategoryResponse>>
                {
                    Status = true,
                    Data = response,
                    Message = message,
                }
            );
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            _logger.LogInformation("received request to get Category with id:{Id}", id);
            if (id == Guid.Empty)
                return BadRequest(new APIResponse<CategoryResponse>(InvalidInputMessage));
            var (response, message) = await _categoryUseCase.GetCategoryById(id);
            _logger.LogInformation("sucessfuly found category with id:{Id}", id);
            return Ok(new APIResponse<CategoryResponse> { Data = response, Message = message });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryInput category)
        {
            _logger.LogInformation("received request to Create Category:{Category}", category);
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
                return BadRequest(new APIResponse<CategoryResponse>(InvalidInputMessage));
            var (response, message) = await _categoryUseCase.CreateCategory(category);
            _logger.LogInformation("sucessfuly created category");
            return CreatedAtAction(
                nameof(CreateCategory),
                new { id = response!.CategoryId },
                new APIResponse<CategoryResponse>(response, message)
            );
        }

        [HttpPost("CreateMany")]
        public async Task<IActionResult> CreateMany([FromBody] List<CategoryInput> inputs)
        {
            _logger.LogInformation(
                "received request to create {InputCount} categories",
                inputs.Count
            );
            if (inputs == null || inputs.Count == 0)
                return BadRequest(new APIResponse<CategoryResponse>(InvalidInputMessage));

            if (inputs.Any(x => string.IsNullOrWhiteSpace(x.CategoryName)))
                return BadRequest(new APIResponse<CategoryResponse>(InvalidInputMessage));

            var (response, message) = await _categoryUseCase.CreateManyCategory(inputs);
            _logger.LogInformation(
                "sucessfuly created {ResponseCount} categories",
                response!.Count
            );
            return CreatedAtAction(
                nameof(CreateMany),
                new { id = response },
                new APIResponse<List<CategoryResponse>>(response, message)
            );
        }
    }
}
