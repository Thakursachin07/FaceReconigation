// using Microsoft.AspNetCore.Mvc;
// using System.Net.Http.Headers;
// using System.IO;


// namespace ImageCaptureAPI.Controllers
// {
//     [ApiController]
//     [Route("api/image")]
//     public class FaceCompareController : ControllerBase
//     {
//         private readonly string _basePath = Directory.GetCurrentDirectory();
//         private readonly HttpClient _httpClient;

//         public FaceCompareController()
//         {
//             _httpClient = new HttpClient
//             {
//                 BaseAddress = new Uri("http://localhost:5000") // DeepStack URL
//             };
//         }

//         /// <summary>
//         /// Upload and save the registered face image
//         /// </summary>
//         [HttpPost("register")]
//         public async Task<IActionResult> UploadRegisteredImage(IFormFile image)
//         {
//             if (image == null || image.Length == 0)
//                 return BadRequest("No registered image received");

//             var filePath = Path.Combine(_basePath, "Registered.jpg");

//             using var stream = new FileStream(filePath, FileMode.Create);
//             await image.CopyToAsync(stream);

//             Console.WriteLine($" Registered image saved at: {filePath}");
//             return Ok(new { message = "Registered image saved successfully" });
//         }

//         /// <summary>
//         /// Upload live image, compare with registered image using DeepStack, return match %
//         /// </summary>
//         [HttpPost("verify")]
//         public async Task<IActionResult> UploadLiveImageAndCompare(IFormFile image)
//         {
//             var registeredPath = Path.Combine(_basePath, "Registered.jpg");
//             var livePath = Path.Combine(_basePath, "Live.jpg");

//             if (!System.IO.File.Exists(registeredPath))
//                 return BadRequest("Registered image not found");

//             // Save live image
//             using (var stream = new FileStream(livePath, FileMode.Create))
//             {
//                 await image.CopyToAsync(stream);
//             }

//             await Task.Delay(100); // Small delay to ensure file is released

//             Console.WriteLine(image.FileName);
//             Console.WriteLine(registeredPath.Length);
//             // Call DeepStack
//             double similarity = await CompareFacesWithDeepStack(registeredPath, livePath);

//             string resultText = similarity >= 85 ? "Matched"
//                                : similarity >= 60 ? "Possibly Same Person"
//                                : "Mismatch";

//             return Ok(new
//             {
//                 matchingPercentage = similarity,
//                 result = resultText
//             });
//         }

//         /// <summary>
//         /// Call DeepStack /v1/vision/face/match API with two images
//         /// </summary>
//       private async Task<double> CompareFacesWithDeepStack(string imgPath1, string imgPath2)
// {
//     try
//     {
//         var content = new MultipartFormDataContent();

//         //  Read and add first image
//         var imgBytes1 = await System.IO.File.ReadAllBytesAsync(imgPath1);
//         var img1Content = new ByteArrayContent(imgBytes1);
//         img1Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
//         content.Add(img1Content, "images1", "image1.jpg"); //  Correct field name and filename

//         //  Read and add second image
//         var imgBytes2 = await System.IO.File.ReadAllBytesAsync(imgPath2);
//         var img2Content = new ByteArrayContent(imgBytes2);
//         img2Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
//         content.Add(img2Content, "images2", "image2.jpg"); // Same field name

//         //  Send to DeepStack
//         var response = await _httpClient.PostAsync("/v1/vision/face/match", content);
//         var responseBody = await response.Content.ReadAsStringAsync();

//                 Console.WriteLine("DeepStack Response: " + responseBody);
//                 Console.WriteLine(imgPath1.Length);
//                 Console.WriteLine(imgPath2.Length); 
       

//         if (!response.IsSuccessStatusCode)
//                 {
//                     Console.WriteLine($"DeepStack error: {response.StatusCode}, Body: {responseBody}");
//                     return 0;
//                 }

//         var json = System.Text.Json.JsonDocument.Parse(responseBody);
//         if (!json.RootElement.GetProperty("success").GetBoolean())
//         {
//             Console.WriteLine("DeepStack returned success = false");
//             return 0;
//         }

//         var similarity = json.RootElement.GetProperty("similarity").GetDouble();
//         Console.WriteLine($"DeepStack Match: {similarity}%");
//         return similarity;
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"DeepStack Exception: {ex.Message}");
//         return 0;
//     }
// }




//     }
// }
