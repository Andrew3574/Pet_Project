using AutoMapper;
using EventsAPI.ModelProfiles;
using EventsAPI.Models;
using EventsAPI.Services;
using EventsAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Repositories;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace EventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly GuestsRepository _guestsRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IValidator<RegisterModel> _validator;

        public AccountController(IMapper mapper,IMemoryCache memoryCache, EmailService emailService, GuestsRepository guestsRepository, IValidator<RegisterModel> validator)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _emailService = emailService;
            _guestsRepository = guestsRepository;
            _validator = validator;
        }

        [HttpPost("request_code")]
        public async Task<IActionResult> RequestConfirmationCode(string email)
        {
            if(MailAddress.TryCreate(email,out var mailAddress))
            {
                var guest = await _guestsRepository.GetByEmail(email);
                if (guest == null)
                {
                    return NotFound("Register before logIn");
                }
                var code = new Random().Next(1000, 9999).ToString();
                await _emailService.SendAsync(email, "Email confirmation code", code);
                _memoryCache.Set(email, code, TimeSpan.FromMinutes(10));
                return Ok($"Code has been sent to email");
            }

            return BadRequest("Неверная почта");

        }


        [HttpGet("login")]
        public async Task<IActionResult> Login(string email, string code)
        {
            if(_memoryCache.TryGetValue(email,out var storedCode))
            {
                if (storedCode.Equals(code))
                {
                    var guest = await _guestsRepository.GetByEmail(email);
                    var token = AuthenticationService.GenerateJSONWebToken(guest);
                    _memoryCache.Remove(email);
                    return Ok(token);
                }
            }            

            return BadRequest("Wrong email or code");

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                var validatorResult = _validator.Validate(model);

                if (validatorResult.IsValid)
                {
                    MailAddress mailAddress = new MailAddress(model.Email);

                    var guest = _mapper.Map<Guest>(model);
                    await _guestsRepository.Create(guest);
                    return Ok("Success registration");
                }
                var errors = new HttpValidationProblemDetails(validatorResult.ToDictionary());
                return BadRequest(errors);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
