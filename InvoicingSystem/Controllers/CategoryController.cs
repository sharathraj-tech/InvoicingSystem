using InvoicingSystem.Models;
using InvoicingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvoicingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("get-all")]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost("create")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null.");
            }
            try
            {
                if (_categoryService.CategoryExists(category.Name))
                {
                    return BadRequest($"Category with Name: {category.Name} already exist.");
                    
                }
                else {
                    _categoryService.AddCategory(category);
                    return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        [HttpPut("update/{id:int}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null || id != category.Id)
            {
                return BadRequest("Invalid category data.");
            }

            var existingCategory = _categoryService.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            _categoryService.UpdateCategory(id, category);
            return NoContent();
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
