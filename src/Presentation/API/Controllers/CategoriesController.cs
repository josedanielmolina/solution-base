using Core.Application.DTOs.Categories;
using Core.Application.Features.Categories;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IGetCategories _getCategories;
    private readonly IGetCategoriesByCountry _getCategoriesByCountry;
    private readonly IGetCategoryById _getCategoryById;
    private readonly ICreateCategory _createCategory;
    private readonly IUpdateCategory _updateCategory;
    private readonly IDeleteCategory _deleteCategory;

    public CategoriesController(
        IGetCategories getCategories,
        IGetCategoriesByCountry getCategoriesByCountry,
        IGetCategoryById getCategoryById,
        ICreateCategory createCategory,
        IUpdateCategory updateCategory,
        IDeleteCategory deleteCategory)
    {
        _getCategories = getCategories;
        _getCategoriesByCountry = getCategoriesByCountry;
        _getCategoryById = getCategoryById;
        _createCategory = createCategory;
        _updateCategory = updateCategory;
        _deleteCategory = deleteCategory;
    }

    [HttpGet]
    [HasPermission("categories.view")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll([FromQuery] bool activeOnly = true)
    {
        var categories = await _getCategories.ExecuteAsync(activeOnly);
        return Ok(categories);
    }

    [HttpGet("by-country/{countryId:int}")]
    [HasPermission("categories.view")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetByCountry(int countryId, [FromQuery] Gender? gender = null)
    {
        var categories = await _getCategoriesByCountry.ExecuteAsync(countryId, gender);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    [HasPermission("categories.view")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _getCategoryById.ExecuteAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    [HasPermission("categories.create")]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
    {
        try
        {
            var category = await _createCategory.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [HasPermission("categories.edit")]
    public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            var category = await _updateCategory.ExecuteAsync(id, dto);
            if (category == null) return NotFound();
            return Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [HasPermission("categories.delete")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _deleteCategory.ExecuteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
