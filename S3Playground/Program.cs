using Amazon.S3;
using Amazon.S3.Model;
using System.Text;

AmazonS3Client s3Client = new ();


GetObjectRequest getObjectRequest = new()
{
    BucketName = "minhsawsbucket",
    Key = "files/movies.csv"
};

using GetObjectResponse response = await s3Client.GetObjectAsync(getObjectRequest);
using MemoryStream memoryStream = new();
await response.ResponseStream.CopyToAsync(memoryStream);
string text = Encoding.UTF8.GetString(memoryStream.ToArray());

Console.WriteLine(text);

//using FileStream fileStream = new("./face.png", FileMode.Open, FileAccess.Read);

//PutObjectRequest objectRequest = new ()
//{
//    BucketName = "minhsawsbucket",
//    Key = "images/face.png",
//    ContentType = "image/png",
//    InputStream = fileStream
//};

//await s3Client.PutObjectAsync(objectRequest);