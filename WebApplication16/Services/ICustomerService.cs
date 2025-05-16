using Microsoft.AspNetCore.Mvc;

namespace WebApplication16.Services;

public interface ICustomerService
{
    public Task<IActionResult> getInfoForClientById(int id);
}