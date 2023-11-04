using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLiteCRUD.Interface;
using SQLiteCRUD.Models;
using SQLiteCRUD.Validator;

namespace SQLiteCRUD.Controller
{
    [Route("api/drivers")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ILogger<DriversController> _logger;
        private readonly DriverRequestValidator _validator;

        public DriversController(IDriverRepository driverRepository, ILogger<DriversController> logger, DriverRequestValidator validator)
        {
            _driverRepository = driverRepository;
            _logger = logger;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> AddDriver([FromBody] Driver driverRequest)
        {

            ValidationResult validationResult = _validator.Validate(driverRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            try
            {
                // Add the driver using the repository
                await _driverRepository.AddDriver(driverRequest);

                return Ok("Driver added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while adding a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
