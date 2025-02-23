using Amazon.S3.Model;

namespace S3.Customers.Api.Services;

public interface ICustomerImageService
{
    Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file);

    Task<GetObjectResponse> GetImageAsync(Guid id);

    Task<DeleteObjectResponse> DeleteImageAsync(Guid id);
}
