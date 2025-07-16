// using Microsoft.AspNetCore.Mvc;

// namespace ImageCaptureAPI.Controllers
// {
//     [ApiController]
//     [Route("api/image")]
//     public class ImageUploadController : ControllerBase
//     {
//         [HttpPost("upload")]
//         public async Task<IActionResult> UploadImage(IFormFile image)
//         {
//             if (image == null || image.Length == 0)
//                 return BadRequest("No image received");

//             var filePath = Path.Combine(Directory.GetCurrentDirectory(), "LatestImage.jpg");

//             using (var stream = new FileStream(filePath, FileMode.Create))
//             {
//                 await image.CopyToAsync(stream);
//             }

//             Console.WriteLine($"Image received and saved as: {filePath} at {DateTime.Now}");

//             return Ok(new { message = "Image saved successfully" });
//         }
//     }
    
// }
