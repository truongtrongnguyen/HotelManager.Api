using HotelManage.Authentication.Configuration;
using HotelManage.Authentication.Models.Incoming;
using HotelManage.Authentication.Models.Outgoing;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Employ;
using HotelManager.Entities.Dto.Incoming.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelManager.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly JwtConfig _jwtConfig;
        public UsersController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager,
                                IOptionsMonitor<JwtConfig> jwtConfig
                                ) : base(unitOfWork, userManager)
        {
            _jwtConfig = jwtConfig.CurrentValue;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAll();
            return Ok(users);
        }

        [HttpPost("EditUser")]
        public async Task<IActionResult> EditUser([FromForm]UpdateUserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Payload");
            }

            var isUpdate = await _unitOfWork.Users.UpdateUser(request);

            if (isUpdate)
            {
                await _unitOfWork.CompleteAsync();
                return Ok(request);
            }
            return BadRequest("Something went wrong, please try again latter");
        }

        [HttpPut("EditEmployee")]
        public async Task<IActionResult> EditEmployee([FromForm] UpdateEmployeeDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Payload");
            }

            var updateUserDto = new UpdateUserDto()
            {
                FullName =  request.FullName,
                Sex = request.Sex,
                BirthDay = request.BirthDay,
                Address = request.Address,
                Avata = request.Avata,
                PhoneNumber = request.PhoneNumber,
                IdentityCard = request.IdentityCard,
                Id = request.Id
            };

            // Updating User and Employee are the same, we will use the same function UpdateUser
            var isUpdateEmployee = await _unitOfWork.Users.UpdateUser(updateUserDto);
            var isUpdateUser = await _unitOfWork.Employees.UpdateEmployee(request);

            if (isUpdateEmployee && isUpdateUser)
            {
                await _unitOfWork.CompleteAsync();
                return Ok(request);
            }
            return BadRequest("Something went wrong, please try again latter");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if(user == null)
                {
                    return BadRequest(new UserLogInResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid Payload"}
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

                if (isCorrect)
                {
                    var token = GenerateJwtToken(user);

                    return Ok(new UserLogInResponse()
                    {
                        Success = true,
                        Token = token
                    });
                }
                else
                {
                    return BadRequest(new UserLogInResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid Payload" }
                    });
                }
            }
            else
            {
                return BadRequest(new UserLogInResponse()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }

        [HttpPost("Registrantion")]
        public async Task<IActionResult> Resigtrantion(UserRegistrantionRequestDto request)
        {
            if (ModelState.IsValid)
            {
                var userExist = await _userManager.FindByEmailAsync(request.Email);

                if (userExist != null)
                {
                    return BadRequest(new UserRegistrantionResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Email already in use" }
                    });
                }

                var user = new AppUser()
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    UserName = request.Email,
                    EmailConfirmed = true,
                    IsCustomer = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new UserRegistrantionResponse()
                    {
                        Success = false,
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    });
                }

                // create jwt Token
                var token = GenerateJwtToken(user);

                return Ok(new UserRegistrantionResponse()
                {
                    Token = token,
                    Success = true,
                });
            }
            else
            {
                return BadRequest(new UserRegistrantionResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid Payload" }
                });
            }
        }


        [HttpPost("ResigtrantionEmployee")]
        public async Task<IActionResult> ResigtrantionEmployee(EmployeeRegistrantionRequest request)
        {
            if (ModelState.IsValid)
            {
                var userExist = await _userManager.FindByEmailAsync(request.Email);

                if (userExist != null)
                {
                    return BadRequest(new EmployeeRegistrantionResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Email already in use" }
                    });
                }

                var user = new AppUser()
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    UserName = request.Email,
                    EmailConfirmed = true,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Sex = request.Sex,
                    BirthDay = request.BirthDay,
                    IdentityCard = request.IdentityCard,
                    IsEmployee = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new EmployeeRegistrantionResponse()
                    {
                        Success = false,
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    });
                }

                var employee = new Employee()
                {
                    Position = request.Position,
                    IdentityId = user.Id,
                    Allowvance = request.Allowvance,
                    Salary = request.Salary,
                    DateContact = request.DateContact,
                    ContactTerm = request.ContactTerm,
                    BankNumber = request.BankNumber
                };
                await _unitOfWork.Employees.Add(employee);
                await _unitOfWork.CompleteAsync();

                // create jwt Token
                var token = GenerateJwtToken(user);

                return Ok(new UserRegistrantionResponse()
                {
                    Token = token,
                    Success = true,
                });
            }
            else
            {
                return BadRequest(new EmployeeRegistrantionResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid Payload" }
                });
            }
        }


        private string GenerateJwtToken(AppUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),      // todo update expiration time to minutes
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            // generate the security obj token
            var token = jwtHandler.CreateToken(tokenDescription);

            var jwtToken = jwtHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
