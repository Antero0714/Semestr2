using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Runtime.InteropServices;
using MyHttpServer.Models;

namespace httpserver.Controllers // Замените на пространство имён вашего проекта
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartoonsController : ControllerBase
    {
        private readonly string _connectionString = @"Data Source=localhost;Initial Catalog=user;User ID=sa;Password=P@ssw0rd;";

        [HttpGet]
        [Route("cartoons")]
        public IActionResult GetCartoons()
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    var query = "SELECT * FROM dbo.Users";
                    var cartoons = db.Query<Film>(query).ToList();
                    return Ok(cartoons);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}