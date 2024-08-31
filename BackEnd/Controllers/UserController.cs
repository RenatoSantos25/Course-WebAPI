using Microsoft.AspNetCore.Mvc;
using ReserveiAPI.Objects.Contracts;
using ReserveiAPI.Objects.DTOs.Entities;
using ReserveiAPI.Objects.Utilities;
using ReserveiAPI.Services.interfaces;
using System.Dynamic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Jose.Compact;
namespace ReserveiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly Response _response;

        public UserController(IUserService userService)
        {
            _userService = userService;
            _response = new Response();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<UserDTOs>>> GetAll()
        {
            try
            {
                var userDTO = await _userService.GetAll();
                _response.SetSucces();
                _response.Message = userDTO.Any() ?
                    "Lista do(s) Usuário(s) obtida com sucesso." :
                    "Nenhum Usuário encontrado";
                _response.Data = userDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Nao foi possivel adquirir a lista do(s) Usuário(s)";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace availabel!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

        }
        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<UserDTOs>> GetById(int id)
        {
            try
            {
                var userDTO = await _userService.GetByID(id);
                if (userDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "Usuário nao encontrado";
                    _response.Data = userDTO;
                    return NotFound(_response);
                }
                _response.SetSucces();
                _response.Message = "Usuário" + userDTO.NameUSer + "obtido com sucesso";
                _response.Data = userDTO;
                return NotFound(_response);

            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Nao foi possivel adquirir usuario informado!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace availabel!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] UserDTOs userDTO)
        {
            if (userDTO is null)
            {
                _response.SetInvalid();
                _response.Message = "Dado(s) inválido(s)!";
                _response.Data = userDTO;
                return BadRequest(_response);
            }
            userDTO.Id = 0;

            try
            {
                dynamic errors = new ExpandoObject();
                var hasErrors = false;

                CheckDatas(userDTO, ref errors, ref hasErrors);

                if (hasErrors)
                {
                    _response.SetConflict();
                    _response.Message = "Dado(s) com conflito!";
                    _response.Data = errors;
                    return BadRequest(_response);
                }

                var usersDTO = await _userService.GetAll();
                CheckDuplicates(usersDTO, userDTO, ref errors, ref hasErrors);

                if (hasErrors)
                {
                    _response.SetConflict();
                    _response.Message = "Dado(s) com conflito!";
                    _response.Data = errors;
                    return BadRequest(_response);
                }

                // Criptografa a senha
                var hashedPassword = OperatorUtilitie.HashPassword(userDTO.PasswordUser);

                // Remove o primeiro caractere da senha criptografada
                if (hashedPassword.Length > 0)
                {
                    hashedPassword = hashedPassword.Substring(0);
                }

                userDTO.PasswordUser = hashedPassword;

                await _userService.Create(userDTO);

                _response.SetSucces();
                _response.Message = "Usuário " + userDTO.NameUSer + " cadastrado com sucesso.";
                _response.Data = userDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível cadastrar o Usuário!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
        [HttpPost("Login")]
		public async Task<ActionResult> Login([FromBody] Login login)
		{
			if (login is null)
			{

				_response.SetInvalid();
				_response.Message = "Dado(s) inválido(s)";
				_response.Data = login;
				return BadRequest(_response);
			}
			

			try
			{
				login.Password = login.Password.HashPassword();
                var userDTO = await _userService.Login(login);

				if (userDTO is null)
				{
					_response.SetUnauthorized();
					_response.Message = "Login inválido";
					_response.Data = new { errorLogin = "Login invalido" };
					return BadRequest(_response);
				}

                var token = new Token();
                token.GenerateToken(userDTO.EmailUser);

				_response.SetSucces();
				_response.Message = "Login realizado com sucesso";
				_response.Data = token;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.SetError();
				_response.Message = "Não foi possível realizar o Login!";
				_response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}
		[HttpPost("Validate")]
		public async Task<ActionResult> Validate([FromBody] Token token)
		{
			if (token is null)
			{

				_response.SetInvalid();
				_response.Message = "Dado(s) inválido(s)";
				_response.Data = token;
				return BadRequest(_response);
			}


			try
			{
				var email = token.ExtractSubject();

                if (string.IsNullOrEmpty(email) || await _userService.GetByEmail(email) == null)
                {
                    _response.SetUnauthorized();
                    _response.Message = "Token inválido";
                    _response.Data = new { errorToken = "Token invalido" };
                    return BadRequest(_response);
                }
                else if (!token.ValidateToken())
                {
					_response.SetUnauthorized();
					_response.Message = "Token inválido";
					_response.Data = new { errorToken = "Token invalido" };
					return BadRequest(_response);
				}

				_response.SetSucces();
				_response.Message = "token validado com sucesso";
				_response.Data = token;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.SetError();
				_response.Message = "Não foi possível validar o Token!";
				_response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}

		[HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] UserDTOs userDTO)
        {
            if (userDTO is null)
            {

                _response.SetInvalid();
                _response.Message = "Dado(s) inválido(s)";
                _response.Data = userDTO;
                return BadRequest(_response);
            }
            try
            {

                var existingUserDTO = await _userService.GetByID(userDTO.Id);
                if (existingUserDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "Dado(s) com conflito";
                    _response.Data = new { errorId = "O usuario informado nao existe!" };
                    return NotFound(_response);
                }


                dynamic errors = new ExpandoObject();
                var hasErrors = false;

                if (hasErrors)
                {
                    _response.SetConflict();
                    _response.Message = "Dado(s) com conflitos";
                    _response.Data = errors;
                    return BadRequest(_response);
                }
                
                CheckDatas(userDTO, ref errors, ref hasErrors);

                if (hasErrors)
                {
                    _response.SetConflict();
                    _response.Message = "Dado(s) com conflito!";
                    _response.Data = errors;
                    return BadRequest(_response);
                }
                var usersDTO = await _userService.GetAll();
                CheckDuplicates(usersDTO, userDTO, ref errors, ref hasErrors);

                if (hasErrors)
                {
                    _response.SetConflict();
                    _response.Message = "Dado(s) com conflitos";
                    _response.Data = errors;
                    return BadRequest(_response);
                }

                userDTO.PasswordUser = existingUserDTO.PasswordUser;
                await _userService.Update(userDTO);

                _response.SetSucces();
                _response.Message = "Usuário " + userDTO.NameUSer + " alterado com sucesso. ";
                _response.Data = userDTO;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível cadastrar o Usuário!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<UserDTOs>> Delete(int id)
        {
            try
            {
                var userDTO = await _userService.GetByID(id);

                if (userDTO is null)
                {

                    _response.SetNotFound();
                    _response.Message = "dado com conflito";
                    _response.Data = new { errorId = "Usuario nao encontrado" };
                    return NotFound(_response);
                }
                await _userService.Delete(userDTO);

                _response.SetNotFound();
                _response.Message = "Usuário " + userDTO.NameUSer + " excluido com sucesso. ";
                _response.Data = userDTO;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível cadastrar o Usuário!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
        private static void CheckDatas(UserDTOs userDTO, ref dynamic errors, ref bool hasErrors)
        {
            if (!ValidatorUtilitie.CheckValidPhone(userDTO.PhoneUser))
            {
                errors.errorPhoneUser = "Número Inválido!";
                hasErrors = true;
            }

            int status = ValidatorUtilitie.CheckValidEmail(userDTO.EmailUser);
            if (status == -1)
            {
                errors.errorEmailUser = "E-mail inválido!";
                hasErrors = true;
            }
            else if (status == -2)
            {
                errors.errorEmailUser = "Domínio inválido!";
                hasErrors = true;
            }
        }

        private static void CheckDuplicates(IEnumerable<UserDTOs> usersDTO, UserDTOs userDTO, ref dynamic errors, ref bool hasErrors)
        {
            foreach (var user in usersDTO)
            {
                if (userDTO.Id == user.Id)
                {
                    continue;
                }

                if (ValidatorUtilitie.CompareString(userDTO.EmailUser, user.EmailUser))
                {
                    errors.errorEmailUser = "O e-mail " + userDTO.EmailUser + " já está sendo utilizado!";
                    hasErrors = true;

                    break;


                }


            }
        }
    }
}
