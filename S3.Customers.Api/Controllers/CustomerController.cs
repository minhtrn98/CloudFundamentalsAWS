using S3.Customers.Api.Contracts.Requests;
using S3.Customers.Api.Mapping;
using S3.Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace S3.Customers.Api.Controllers;

[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpPost("customers")]
    public async Task<IActionResult> Create([FromBody] CustomerRequest request)
    {
        Domain.Customer customer = request.ToCustomer();

        await customerService.CreateAsync(customer);

        Contracts.Responses.CustomerResponse customerResponse = customer.ToCustomerResponse();

        return CreatedAtAction("Get", new { customerResponse.Id }, customerResponse);
    }

    [HttpGet("customers/{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        Domain.Customer? customer = await customerService.GetAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        Contracts.Responses.CustomerResponse customerResponse = customer.ToCustomerResponse();
        return Ok(customerResponse);
    }

    [HttpGet("customers")]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Domain.Customer> customers = await customerService.GetAllAsync();
        Contracts.Responses.GetAllCustomersResponse customersResponse = customers.ToCustomersResponse();
        return Ok(customersResponse);
    }

    [HttpPut("customers/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromMultiSource] UpdateCustomerRequest request)
    {
        DateTime requestStarted = DateTime.UtcNow;

        Domain.Customer? existingCustomer = await customerService.GetAsync(request.Id);

        if (existingCustomer is null)
        {
            return NotFound();
        }

        Domain.Customer customer = request.ToCustomer();
        await customerService.UpdateAsync(customer, requestStarted);

        Contracts.Responses.CustomerResponse customerResponse = customer.ToCustomerResponse();
        return Ok(customerResponse);
    }

    [HttpDelete("customers/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        bool deleted = await customerService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
