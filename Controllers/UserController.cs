namespace API.Controllers;

using Models;
using Data;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

  private readonly UserRepo _userRepo;

  public UserController(UserRepo userRepo)
  {
    _userRepo = userRepo;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var users = await _userRepo.GetAllUsersAsync();
    return Ok(users);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetByID(int id)
  {
    var user = await _userRepo.GetUserById(id);

    if (user == null)
    {
      return NotFound();
      // 404 error
    }

    return Ok(user);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] User user)
  {
    await _userRepo.AddUserAsync(user);
    return Ok("User added.");
  }
  
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    await _userRepo.DeleteUserAsync(id);

    return Ok("User deleted.");
  }
}