using Amazon.S3;
using Amazon.S3.Model;
using System.Text;

const string bucketName = "minhtrn98-bucket";

AmazonS3Client s3Client = new(new AmazonS3Config()
{
    Profile = new Amazon.Profile("developer-01-mfa"),
    RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1,
});


//GetObjectRequest getObjectRequest = new()
//{
//    BucketName = bucketName,
//    Key = "files/movies.csv"
//};

//using GetObjectResponse response = await s3Client.GetObjectAsync(getObjectRequest);
//using MemoryStream memoryStream = new();
//await response.ResponseStream.CopyToAsync(memoryStream);
//string text = Encoding.UTF8.GetString(memoryStream.ToArray());

//Console.WriteLine(text);

//using FileStream fileStream = new("./face.png", FileMode.Open, FileAccess.Read);

//PutObjectRequest objectRequest = new ()
//{
//    BucketName = bucketName,
//    Key = "images/face.png",
//    ContentType = "image/png",
//    InputStream = fileStream
//};

//await s3Client.PutObjectAsync(objectRequest);

List<string> tempKeys = await ListAllObjectKeysByPrefix(s3Client, bucketName, "temp/");
List<string> imagesKeys = await ListAllObjectKeysByPrefix(s3Client, bucketName, "images/");

string[] tempKeysToDelete = [.. tempKeys.Except(imagesKeys).Select(x => $"temp/{x}")];

await DeleteObjectsByKey(s3Client, bucketName, tempKeysToDelete);

Console.ReadKey();

static async Task<List<string>> ListAllObjectKeysByPrefix(
    AmazonS3Client s3Client,
    string bucketName,
    string prefix)
{
    List<string> result = [];
    ListObjectsV2Response listObjectsResponse;
    string? lastKey = string.Empty;
    do
    {
        ListObjectsV2Request listObjectsRequest = new()
        {
            BucketName = bucketName,
            Prefix = prefix,
            MaxKeys = 5,
            StartAfter = lastKey
        };
        listObjectsResponse = await s3Client.ListObjectsV2Async(listObjectsRequest);
        foreach (S3Object entry in listObjectsResponse.S3Objects)
        {
            result.Add(entry.Key.Replace(prefix, ""));
        }
        lastKey = listObjectsResponse.S3Objects.LastOrDefault()?.Key;

    } while (listObjectsResponse.IsTruncated);
    return result;
}

static async Task DeleteObjectsByKey(
    AmazonS3Client s3Client,
    string bucketName,
    string[] keys)
{
    DeleteObjectsRequest deleteRequest = new ()
    {
        BucketName = bucketName,
        Objects = [.. keys.Select(x => new KeyVersion() { Key = x,  })],
    };

    DeleteObjectsResponse deleteResponse = await s3Client.DeleteObjectsAsync(deleteRequest);

    if (deleteResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
    {
        Console.WriteLine($"Deleted {deleteResponse.DeletedObjects.Count} objects.");
    }
    else
    {
        Console.WriteLine($"Failed to delete objects. Status code: {deleteResponse.HttpStatusCode}");
    }
}