using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using BCrypt.Net;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    string connectionString = "server=localhost;user=root;password=HenryGeorge1!;database=sovitora";

    [HttpPost("register")]
public IActionResult Register([FromBody] UserLogin user)
{
    using var conn = new MySqlConnection(connectionString);
    conn.Open();

    // 1️⃣ Check if email already exists
    var checkCmd = new MySqlCommand(
        "SELECT COUNT(*) FROM users WHERE email=@Email", conn);
    checkCmd.Parameters.AddWithValue("@Email", user.Email);

    var count = Convert.ToInt32(checkCmd.ExecuteScalar());
    if (count > 0)
    {
        return BadRequest("Email already registered");
    }

    // 2️⃣ Hash the password
    var hash = BCrypt.Net.BCrypt.HashPassword(user.Password);

    // 3️⃣ Insert new user
    var insertCmd = new MySqlCommand(
        "INSERT INTO users (email,password) VALUES (@Email,@Password)", conn);
    insertCmd.Parameters.AddWithValue("@Email", user.Email);
    insertCmd.Parameters.AddWithValue("@Password", hash);
    insertCmd.ExecuteNonQuery();

    return Ok("User created");
}

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin user)
    {
        using var conn = new MySqlConnection(connectionString);
        conn.Open();

        var cmd = new MySqlCommand(
            "SELECT password FROM users WHERE email=@email", conn);

        cmd.Parameters.AddWithValue("@email", user.Email);

        var result = cmd.ExecuteScalar();

        if (result == null)
            return Unauthorized("Email not found");

        bool valid = BCrypt.Net.BCrypt.Verify(user.Password, result.ToString());

        if (!valid)
            return Unauthorized("Invalid password");

        return Ok("Login successful");
    }
}