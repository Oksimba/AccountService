using BusinessLogic.Interfaces;
using Common;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Common.Helpers;
using AutoMapper;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using AccountService.Filters;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        IMapper _mapper;
        ILogger<UserController> _logger;
        public UserController(IOptions<AuthOptions> authOptions, IUserService userService,
            IMapper mapper, ILogger<UserController> logger)
        {
            AuthOptions = authOptions;
            this._userService = userService;
            this._mapper = mapper;
            _logger = logger;
        }

        public IOptions<AuthOptions> AuthOptions { get; }


        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="request">Request which contains LoginNmae and user Password</param>
        /// <returns>Returns token</returns>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            var user = await AuthenticateUser(request.LoginName, request.Password);
            if (user != null)
            {
                var token = GenerateJWT(user);

                return Ok(new
                {
                    access_token = token
                });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Sign Up new user
        /// </summary>
        /// <param name="account">New account with field about new user login, password and roles</param>
        /// <returns>returns IActionResult</returns>
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserCreateDto user)
        {
            try
            {
                if (user == null)
                    return BadRequest();

                if ((await _userService.CheckIfUserExists(user.Login)))
                {
                    return BadRequest($"User with login: {user.Login} already exists.");
                }

                var userToCreate = _mapper.Map<User>(user);
                await _userService.Create(userToCreate);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">The id of the user to get</param>
        /// <returns>ActionResultUserDto></returns>
        /// <response code = "200">Returns the requested user</response>
        [HttpGet("{id}", Name = "GetUser")]
        [MustBeAccountOwner]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            try
            {
                var user = await _userService.Get(id);

                if (user == null)
                    return NotFound($"User with id: {id} doesn`t exists.");

                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(
                    $"Exception while hetting user by id {id}.");

                return StatusCode(500, $"A problem happened while handling your request.{ex.Message}");
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id">The id of the user to update</param>
        /// <param name="updatedUser">user with</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MustBeAccountOwner]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest();

            var user = await _userService.Get(id);

            if (user == null)
                return NotFound($"User with id: {id} doesn`t exists.");

            var userToUpdate = _mapper.Map<User>(updatedUser);
            await _userService.Update(id, userToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="id">The id of user to delete</param>
        /// <returns>Returns deleted user</returns>
        [HttpDelete("{id}")]
        [MustBeAccountOwner]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedUser = await _userService.Delete(id);

            if (deletedUser == null)
                return NotFound();

            _logger.LogInformation($"User with id {id} was successfully deleted.");
            return new ObjectResult(deletedUser);
        }

        private async Task<UserDto> AuthenticateUser(string login, string password)
        {
            var account = await _userService.GetByLogin(login);
            if (account != null && account.HashPassword == AuthHelper.GetHashString(password))
            {
                return _mapper.Map<UserDto>(account);
            }

            return null;
        }

        private string GenerateJWT(UserDto user)
        {
            var authParams = AuthOptions.Value;

            var securityKey = authParams.GetSymetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Login),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
