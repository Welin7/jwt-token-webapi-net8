using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UploadFilesController : ControllerBase
{
    [Authorize]
    [HttpPost("upload")]
    public async Task<ActionResult> UploadFiles([FromForm] ICollection<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("There are no valid files");

        List<byte[]> data = new();

        foreach (var formFile in files)
        {
            if (formFile.Length > 0)
            {
                using (var stream  = new MemoryStream()) 
                {
                    await formFile.CopyToAsync(stream);
                    data.Add(stream.ToArray());
                }
            }
        }

        return File(data[0], files.FirstOrDefault().ContentType, "File.jpg");
    }
}
