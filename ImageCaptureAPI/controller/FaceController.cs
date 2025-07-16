using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FaceController : ControllerBase
{
    private readonly HttpClient httpClient = new();
    private readonly string _basePath = Directory.GetCurrentDirectory();

    /// <summary>
    /// Upload reference image and save to disk as Registered.jpg
    /// </summary>
    [HttpPost("upload")]
   public async Task<IActionResult> UploadImage(IFormFile image)
{
    if (image == null || image.Length == 0)
        return BadRequest("No image provided");

    var filePath = Path.Combine(_basePath, "Registered.jpg");
    try
    {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        using var content = new MultipartFormDataContent();
        using var imageStream = image.OpenReadStream();
        content.Add(new StreamContent(imageStream), "image", "reference.jpg");

        var response = await httpClient.PostAsync("http://localhost:5500/upload", content);
        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, result); // Handle non-200 responses
        }

        return Content(result, "application/json");
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "An error occurred", error = ex.Message });
    }
}


    /// <summary>
    /// Upload live image, save it as Live.jpg
    /// </summary>
    [HttpPost("compare")]
   public async Task<IActionResult> CompareImage(IFormFile liveImage)
{
    if (liveImage == null || liveImage.Length == 0)
        return BadRequest("No live image provided");

    var filePath = Path.Combine(_basePath, "Live.jpg");
    try
    {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await liveImage.CopyToAsync(stream);
        }

        using var content = new MultipartFormDataContent();
        using var imageStream = liveImage.OpenReadStream();
        content.Add(new StreamContent(imageStream), "liveImage", "live.jpg");

        var response = await httpClient.PostAsync("http://localhost:5500/verify", content);
        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, result);
        }

        return Content(result, "application/json");
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "An error occurred", error = ex.Message });
    }
}

}
