using AutoMapper;
using HotelManage.Authentication.Configuration;
using HotelManage.Authentication.Models.Dto.Generic;
using HotelManage.Authentication.Models.Dto.Incoming;
using HotelManage.Authentication.Models.Dto.Outgoing;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Employ;
using HotelManager.Entities.Dto.Incoming.User;
using HotelManager.Entities.Dto.Outgoing.Generic;
using HotelManager.Entities.Dto.Outgoing.UserDto;
using HotelManager.Entities.Message;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelManager.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public UsersController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager,
                                IOptionsMonitor<JwtConfig> jwtConfig,
                                IMapper _mapper,
                                TokenValidationParameters tokenValidationParameters
                                ) : base(unitOfWork, userManager, _mapper)
        {
            _jwtConfig = jwtConfig.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAll();
            return Ok(users);
        }

        [HttpGet("GetEmployeeByEmail")]
        public async Task<IActionResult> GetEmployeeByEmail(string email)
        {
            var user = await _unitOfWork.Users.GetEmployeeByEmail(email);

            var result = new Result<UserDto>();

            if (user == null)
            {
                result.Error = PopulateError(400,
                                           ErrorMessage.UserMessage.UserNotFound,
                                           ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }

            var employee = await _unitOfWork.Employees.GetEmployeeByEmail(user.Id);

            if (employee == null)
            {
                result.Error = PopulateError(400,
                                           ErrorMessage.UserMessage.UserNotFound,
                                           ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }

            var mapper = _mapper.Map<UserDto>(user);

            result.Content = mapper;
            result.IsSuccess = true;

            return Ok(result);
        }

        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(email);

            var result = new Result<UserDto>();

            if (user == null)
            {
                result.Error = PopulateError(400,
                                           ErrorMessage.UserMessage.UserNotFound,
                                           ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }

            var mapper = _mapper.Map<UserDto>(user);

            result.Content = mapper;
            result.IsSuccess = true;

            return Ok(result);
        }

        [HttpPost("EditUser")]
        public async Task<IActionResult> EditUser([FromForm]UpdateUserDto request)
        {
            var result = new Result<UpdateUserDto>();

            if (!ModelState.IsValid)
            {
                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.InvalidPayload,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }

            var isUpdate = await _unitOfWork.Users.UpdateUser(request);

            if (isUpdate)
            {
                await _unitOfWork.CompleteAsync();
                result.Content = request;
                result.IsSuccess = true;
                return Ok(result);
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
                    var token = await GenerateJwtToken(user);

                    return Ok(new UserLogInResponse()
                    {
                        Success = true,
                        Token = token.JwtToken,
                        RefreshToken = token.RefreshToken
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
                var token = await GenerateJwtToken(user);

                return Ok(new UserRegistrantionResponse()
                {
                    Token = token.JwtToken,
                    RefreshToken = token.RefreshToken,
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
                var token = await  GenerateJwtToken(user);

                return Ok(new UserRegistrantionResponse()
                {
                    Token = token.JwtToken,
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

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenRequestDto token)
        {
            if (ModelState.IsValid)
            {
                var result = await VerifyAndGenerateToken(token);

                if (result == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid Token"
                        }
                    });
                }

                return Ok(result);
            }
            else
            {
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "Invalid Payload"
                        }
                });
            }
        }

        private async Task<AuthResult> VerifyAndGenerateToken(TokenRequestDto token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _tokenValidationParameters.ValidateLifetime = false;

                var tokenVerifiecation = jwtTokenHandler.ValidateToken(token.Token,
                                                                        _tokenValidationParameters,
                                                                        out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                                    StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        return null;
                    }
                }

                var utcExpiryDate = long.Parse(tokenVerifiecation.Claims
                                                                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token has not expiry"
                        }
                    };
                }

                var storeToken = await _unitOfWork.RefreshTokens.GetByRefreshToken(token.RefreshToken);

                if (storeToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid token"
                        }
                    };
                }

                if (storeToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh Token has been  used, is cannot be reused"
                        }
                    };
                }

                if (storeToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh Token has been  Revoked, is cannot be used"
                        }
                    };
                }

                var jti = tokenVerifiecation.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (jti != storeToken.JwtId)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Refresh Token has expired, please login again"
                        }
                    };
                }

                if (storeToken.ExpiryTime < DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Expiry token"
                        }
                    };
                }

                // update token
                var updateResult = await _unitOfWork.RefreshTokens.MarkRefreskTokenAsUsed(storeToken);

                if (updateResult)
                {
                    await _unitOfWork.CompleteAsync();

                    var user = await _userManager.FindByIdAsync(storeToken.UserId);

                    if (user == null)
                    {
                        return new AuthResult()
                        {
                            Success = false,
                            Errors = new List<string>()
                        {
                            "Error processing request"
                        }
                        };
                    }

                    var tokenData = await GenerateJwtToken(user);

                    return new AuthResult()
                    {
                        Token = tokenData.JwtToken,
                        RefreshToken = tokenData.RefreshToken,
                        Success = true,
                    };
                }
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "Error processing request"
                        }
                };
            }
            catch(Exception ex)
            {
                // TODO: Add better error handling
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "Server Error",
                            ex.Message
                        }
                };
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Set the time to 1, Jan, 1970
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            // add the number os seconds from 1 Jan 1970
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTimeVal;

        }

        private async Task<TokenData> GenerateJwtToken(AppUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),          // tell which user this token belongs to
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),      // todo update expiration time to minutes
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            // generate the security obj token
            var token = jwtHandler.CreateToken(tokenDescription);

            var jwtToken = jwtHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                UserId = user.Id,
                Token = $"{RamdomStringGenerator(20)}_{Guid.NewGuid()}",
                IsUsed = false,
                IsRevoked = false,
                ExpiryTime = DateTime.UtcNow.AddMonths(6),
                JwtId = token.Id
            };

            await _unitOfWork.RefreshTokens.Add(refreshToken);
            await _unitOfWork.CompleteAsync();

            var tokenData = new TokenData()
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };
            return tokenData;
        }

        private string RamdomStringGenerator(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
