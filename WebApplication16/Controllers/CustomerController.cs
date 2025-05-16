using Microsoft.AspNetCore.Mvc;
using WebApplication16.Services;

namespace WebApplication16.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomerController: ControllerBase
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True;";
    private readonly ICustomerService _service;
    
    public CustomerController(ICustomerService service)
    {
        _service = service;
    }
    
    // ENDPOINT ZWRACA INFORMACJE O WSZYSTKICH WYCIECZKACH ORAZ KRAJACH W KTÓRYCH SIĘ ODBYWAJĄ
    [HttpGet("{id}")]
    public async Task<IActionResult> getInfoForClient(int id)
    {
        return await _service.getInfoForClientById(id);
    }

}