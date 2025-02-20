using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizAppAPI.Data;
using QuizAppAPI.Model.DTO.Users;
using QuizAppAPI.Model.DTO.UsersDTOs;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QuizDbContext _quizDbContext;
        private readonly IMapper _userMapper;

        public UserController(QuizDbContext quizDbContext,IMapper userMapper)
        {
            _quizDbContext = quizDbContext;
            _userMapper = userMapper;
        }

        private string HashPassword(string password)
        {
            // Use a secure hashing algorithm (e.g., BCrypt, PBKDF2) in a real application
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _quizDbContext.Users.Include(u => u.Quizzes).ToListAsync();
            var userInfos = _userMapper.Map<IEnumerable<UserInfoDTO>>(users);
            return Ok(userInfos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _quizDbContext.Users
                .Include(u => u.Quizzes)
                .FirstOrDefaultAsync( u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            var userInfo = _userMapper.Map<UserInfoDTO>(user);
            return Ok(userInfo);
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<LoginResponseDTO>> Login(UserRegisterDTO userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest("Invalid user data.");
            }

            if (await _quizDbContext.Users.AnyAsync(u => u.UserName == userDto.UserName))
            {
                var user = await _quizDbContext.Users.Where(user => user.UserName == userDto.UserName).FirstOrDefaultAsync();

                if (user.Password == HashPassword(userDto.Password))
                {
                    var loginresponse = _userMapper.Map<LoginResponseDTO>(user);
                    return (ActionResult<LoginResponseDTO>)Ok(loginresponse);
                } 
                else 
                { 
                    return (ActionResult<LoginResponseDTO>)BadRequest("Incorrect Password"); 
                }
            }
            return BadRequest("Username does not exist!");
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserInfoDTO>> RegisterUser(UserRegisterDTO userDto)
        {

            if (userDto == null || string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest(new
                {
                    message = "Invalid user data."
                });
            }

            if (await _quizDbContext.Users.AnyAsync(u => u.UserName == userDto.UserName))
            {
                return Conflict(new
                {
                    message = "Username already exists."
                });
            }

            var addUser = new User
            {
                UserName = userDto.UserName,
                Password = HashPassword(userDto.Password), 
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now
            };

            _quizDbContext.Users.Add(addUser);
            await _quizDbContext.SaveChangesAsync();

            var createdUserDto = _userMapper.Map<UserInfoDTO>(addUser);

            return CreatedAtAction(nameof(GetUserById), new { id = addUser.UserId }, createdUserDto);
        }

        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UserChangePassDTO updated_user)
        {

            var user = await _quizDbContext.Users.Where(user => user.UserName == username).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }
            user.Password = HashPassword(updated_user.Password) ;
            user.LastUpdatedAt = DateTime.Now;
            _quizDbContext.Update(user);
            await _quizDbContext.SaveChangesAsync();

            var changepass = _userMapper.Map<UserInfoDTO>(user);

            return Ok("Successfully changed pass for" + changepass);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _quizDbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _quizDbContext.Users.Remove(user);
            await _quizDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
