using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynamoDb.Customers.Api.Contracts.Data;
using System.Text.Json;

namespace DynamoDb.Customers.Api.Repositories;

public class CustomerRepository(IAmazonDynamoDB dynamoDb) : ICustomerRepository
{
    private readonly string _tableName = "customers";

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        string customerAsJson = JsonSerializer.Serialize(customer);
        Dictionary<string, AttributeValue> customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        PutItemRequest createItemRequest = new()
        {
            TableName = _tableName,
            Item = customerAsAttributes
        };

        PutItemResponse response = await dynamoDb.PutItemAsync(createItemRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        GetItemRequest getItemRequest = new()
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue(){ S = id.ToString() } },
                { "sk", new AttributeValue(){ S = id.ToString() } }
            }
        };
        GetItemResponse response = await dynamoDb.GetItemAsync(getItemRequest);
        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK || response.Item.Count == 0)
        {
            return null;
        }
        string customerAsJson = Document.FromAttributeMap(response.Item).ToJson();
        return JsonSerializer.Deserialize<CustomerDto>(customerAsJson);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        ScanRequest scanRequest = new()
        {
            TableName = _tableName
        };

        ScanResponse response = await dynamoDb.ScanAsync(scanRequest);

        return response.Items.Select(item =>
        {
            string customerAsJson = Document.FromAttributeMap(item).ToJson();
            return JsonSerializer.Deserialize<CustomerDto>(customerAsJson)!;
        });
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        string customerAsJson = JsonSerializer.Serialize(customer);
        Dictionary<string, AttributeValue> customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        PutItemRequest updatedItemRequest = new()
        {
            TableName = _tableName,
            Item = customerAsAttributes
        };

        PutItemResponse response = await dynamoDb.PutItemAsync(updatedItemRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        DeleteItemRequest deleteItemRequest = new()
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue(){ S = id.ToString() } },
                { "sk", new AttributeValue(){ S = id.ToString() } }
            }
        };
        DeleteItemResponse response = await dynamoDb.DeleteItemAsync(deleteItemRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }
}
