using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using S3.Customers.Api.Services;

namespace S3.Customers.Api.Controllers;

[ApiController]
public class CustomerImageController(ICustomerImageService customerImageService) : ControllerBase
{
    [HttpPost("customers/{id:guid}/image")]
    public async Task<IActionResult> Upload([FromRoute] Guid id, [FromForm(Name = "Data")] IFormFile file)
    {
        PutObjectResponse response = await customerImageService.UploadImageAsync(id, file);
        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpGet("customers/{id:guid}/image")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        try
        {
            GetObjectResponse response = await customerImageService.GetImageAsync(id);
            return File(response.ResponseStream, response.Headers.ContentType);
        }
        catch (AmazonS3Exception ex) when(ex.Message is "The specified key does not exist.")
        {
            return NotFound();
        }
    }

    [HttpDelete("customers/{id:guid}/image")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeleteObjectResponse response = await customerImageService.DeleteImageAsync(id);
        return response.HttpStatusCode switch
        {
            System.Net.HttpStatusCode.NoContent => Ok(),
            System.Net.HttpStatusCode.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}
