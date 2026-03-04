using Inferno.src.Adapters.api.Inbound.model;
using Inferno.src.Adapters.Outbound.Persistence;
using Inferno.src.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inferno.src.Adapters.api.Inbound.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ImageController(HellDbContext context) : ControllerBase
{
    private readonly HellDbContext _context = context;

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var imageEntity = new Image(file.FileName, file.ContentType, memoryStream.ToArray());

        await _context.Image.AddAsync(imageEntity);
        await _context.SaveChangesAsync();

        return Ok(new { imageEntity.IdImage, Message = "Image uploaded successfully" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(int id)
    {
        var image = await _context.Image.AsNoTracking().FirstOrDefaultAsync(x => x.IdImage == id);

        if (image == null)
            return NotFound();

        return File(image.ImageData, image.ContentType);
    }
}
