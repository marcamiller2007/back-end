namespace API.Data;

using Models;

using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

public class UserRepo
{

  private readonly string _connectionString;
  public UserRepo(IConfiguration config)
  {
    _connectionString = config.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
  }

  public async Task<List<User>> GetAllUsersAsync()
  {
    var users = new List<User>();

    await using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();

    var command = new SqlCommand("SELECT * FROM Users;", connection);
    await using var reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
      users.Add(new User
      {
        Id = reader.GetInt32(0),
        FirstName = reader.GetString(1),
        LastName = reader.GetString(2),
        Email = reader.GetString(3)
      });
    }

    return users;
  }

  public async Task<User> GetUserById(int id)
  {
    await using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();

    var command = new SqlCommand("SELECT ID, FirstName, LastName, Email FROM Users WHERE ID = @id", connection);
    command.Parameters.AddWithValue("@id", id);

    await using var reader = await command.ExecuteReaderAsync();

    if (await reader.ReadAsync())
    {
      return new User
      {
        Id = reader.GetInt32(0),
        FirstName = reader.GetString(1),
        LastName = reader.GetString(2),
        Email = reader.GetString(3)
      };
    }

    //? Supressing a null warning
    return null!;
  }

  public async Task AddUserAsync(User user)
  {
    await using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();

    var command = new SqlCommand(
      "INSERT INTO Users (FirstName, LastName, Email) Values (@FirstName, @LastName, @Email);", 
      connection
      );
    command.Parameters.AddWithValue("@FirstName", user.FirstName);
    command.Parameters.AddWithValue("@LastName", user.LastName);
    command.Parameters.AddWithValue("@Email", user.Email);

    await command.ExecuteNonQueryAsync();
  }

  public async Task DeleteUserAsync(int id)
  { 
    await using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();
    
    var command = new SqlCommand("DELETE FROM Users WHERE ID = @id", connection);
    command.Parameters.AddWithValue("@id", id);

    await command.ExecuteNonQueryAsync();
  }

}