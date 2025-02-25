using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SimpleS3Lambda;

public class Function
{
    IAmazonS3 S3Client { get; set; }

    public Function()
    {
        S3Client = new AmazonS3Client();
    }

    public Function(IAmazonS3 s3Client)
    {
        this.S3Client = s3Client;
    }

    public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
    {
        List<S3Event.S3EventNotificationRecord> eventRecords = evnt.Records ?? [];
        foreach (S3Event.S3EventNotificationRecord? record in eventRecords)
        {
            S3Event.S3Entity s3Event = record.S3;
            if (s3Event == null)
            {
                continue;
            }

            try
            {
                Amazon.S3.Model.GetObjectMetadataResponse response = await S3Client.GetObjectMetadataAsync(s3Event.Bucket.Name, s3Event.Object.Key);

                if (response.Metadata["x-amz-meta-resized"] == true.ToString())
                {
                    context.Logger.LogLine($"Object {s3Event.Object.Key} was already resized");
                    continue;
                }

                await using Stream itemStream = await S3Client.GetObjectStreamAsync(s3Event.Bucket.Name, s3Event.Object.Key, new Dictionary<string, object>());

                using MemoryStream outStream = new ();
                using (Image image = await Image.LoadAsync(itemStream))
                {
                    image.Mutate(x => x.Resize(500, 500));
                    string originalName = response.Metadata["x-amz-meta-originalname"];
                    await image.SaveAsync(outStream, image.DetectEncoder(originalName));
                }

                await S3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = s3Event.Bucket.Name,
                    Key = s3Event.Object.Key,
                    InputStream = outStream,
                    ContentType = response.Headers.ContentType,
                    Metadata =
                    {
                        ["x-amz-meta-resized"] = true.ToString(),
                        ["x-amz-meta-originalname"] = response.Metadata["x-amz-meta-originalname"],
                        ["x-amz-meta-extension"] = response.Metadata["x-amz-meta-extension"]
                    }
                });

                context.Logger.LogInformation("Resized image with key: {name}", s3Event.Bucket.Name);
            }
            catch (Exception e)
            {
                context.Logger.LogError($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}. Make sure they exist and your bucket is in the same region as this function.");
                context.Logger.LogError(e.Message);
                context.Logger.LogError(e.StackTrace);
                throw;
            }
        }
    }
}