using Amazon.S3;
using Amazon.S3.Model;
using System.Text;

AmazonS3Client s3Client = new(new AmazonS3Config()
{
    Profile = new Amazon.Profile("developer-01-mfa"),
    RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1,
});


//GetObjectRequest getObjectRequest = new()
//{
//    BucketName = "minhtrn98-bucket",
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
//    BucketName = "minhsawsbucket",
//    Key = "images/face.png",
//    ContentType = "image/png",
//    InputStream = fileStream
//};

//await s3Client.PutObjectAsync(objectRequest);


await foreach (string key in ListAllObjectKeysByPrefix(s3Client, "minhtrn98-bucket", "temp/"))
{
    Console.WriteLine($"Key: {key.Replace("temp/", "")}");
}

Console.ReadKey();

static async IAsyncEnumerable<string> ListAllObjectKeysByPrefix(
    AmazonS3Client s3Client,
    string bucketName,
    string prefix)
{
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
            yield return entry.Key;
        }
        lastKey = listObjectsResponse.S3Objects.LastOrDefault()?.Key;

    } while (listObjectsResponse.IsTruncated);
}