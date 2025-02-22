using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Movies.Api;
using System.Text.Json;

//await new DataSeeder().ImportDataAsync();

Movie newMovie = new()
{
    Id = Guid.NewGuid(),
    Title = "21 Jump Street",
    AgeRestriction = 18,
    ReleaseYear = 2012,
    RottenTomatoesPercentage = 85
};

Movie2 newMovie2 = new()
{
    Id = Guid.NewGuid(),
    Title = "21 Jump Street",
    AgeRestriction = 18,
    ReleaseYear = 2012,
    RottenTomatoesPercentage = 85
};

string movieAsJson = JsonSerializer.Serialize(newMovie);
Dictionary<string, AttributeValue> attributeMap = Document.FromJson(movieAsJson).ToAttributeMap();

string movie2AsJson = JsonSerializer.Serialize(newMovie2);
Dictionary<string, AttributeValue> attributeMap2 = Document.FromJson(movie2AsJson).ToAttributeMap();

TransactWriteItemsRequest transactionRequest = new()
{
    TransactItems =
    [
        new TransactWriteItem
        {
            Put = new Put
            {
                TableName = "movies-year-title",
                Item = attributeMap
            }
        },
        new TransactWriteItem
        {
            Put = new Put
            {
                TableName = "movies-title-rotten",
                Item = attributeMap2
            }
        }
    ]
};

AmazonDynamoDBClient dynamoDb = new();
TransactWriteItemsResponse response =  await dynamoDb.TransactWriteItemsAsync(transactionRequest);