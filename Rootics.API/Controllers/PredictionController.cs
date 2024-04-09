using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PredictionController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public PredictionController()
    {
        _httpClient = new HttpClient();
    }

    [HttpPost("predict")]
    public async Task<IActionResult> Predict([FromBody] ImageDataRequest imageDataRequest)
    {
        if (imageDataRequest == null || string.IsNullOrWhiteSpace(imageDataRequest.ImageData))
        {
            return BadRequest("Invalid image data");
        }

        try
        {
            // Validate base64-encoded image data
            if (!IsValidBase64String(imageDataRequest.ImageData))
            {
                return BadRequest("Invalid base64-encoded image data");
            }

            // Remove data URL prefix if present
            var base64String = imageDataRequest.ImageData.Split(',').LastOrDefault()?.Trim();

            // Decode base64-encoded image data
            byte[] imageData = Convert.FromBase64String(base64String);

            // Send the image data to the Flask endpoint
            var apiUrl = "http://127.0.0.1:5000"; // Update with your Flask server's address
            var endpointUrl = $"{apiUrl}/"; // Update with your Flask endpoint
            var content = new ByteArrayContent(imageData);
            var response = await _httpClient.PostAsync(endpointUrl, content);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                var predictionResult = await response.Content.ReadAsStringAsync();
                // Process the prediction result as needed
                return Ok(predictionResult);
            }
            else
            {
                // Handle the case where the request to Flask failed
                return StatusCode((int)response.StatusCode, "Prediction request failed");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    private bool IsValidBase64String(string s)
    {
        try
        {
            // Check if the string is a valid base64 string
            byte[] data = Convert.FromBase64String(s);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}

public class ImageDataRequest
{
    public string ImageData { get; set; }
}
