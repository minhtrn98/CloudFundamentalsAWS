using Amazon.S3;
using Amazon.S3.Model;

namespace S3.Customers.Api.Services;

public class CustomerImageService(IAmazonS3 s3Client) : ICustomerImageService
{
    private readonly string _bucketName = "minhsawsbucket";

    public async Task<DeleteObjectResponse> DeleteImageAsync(Guid id)
    {
        DeleteObjectRequest request = new()
        {
            BucketName = _bucketName,
            Key = $"images/{id}"
        };

        return await s3Client.DeleteObjectAsync(request);
    }

    public async Task<GetObjectResponse> GetImageAsync(Guid id)
    {
        GetObjectRequest request = new()
        {
            BucketName = _bucketName,
            Key = $"images/{id}"
        };

        return await s3Client.GetObjectAsync(request);
    }

    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
    {
        PutObjectRequest request = new()
        {
            BucketName = _bucketName,
            Key = $"images/{id}",
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType,
            Metadata =
            {
                ["x-amz-meta-origionalname"] = file.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(file.FileName)
            }
        };

        return await s3Client.PutObjectAsync(request);
    }
}
