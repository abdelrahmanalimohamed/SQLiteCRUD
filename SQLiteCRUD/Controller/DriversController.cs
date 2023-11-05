using FluentValidation.Results;
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
        private readonly IUtilities _utilities;

        public DriversController(
            IDriverRepository driverRepository, 
            ILogger<DriversController> logger, 
            DriverRequestValidator validator, 
            IUtilities utilities)
        {
            _driverRepository = driverRepository;
            _logger = logger;
            _validator = validator;
            _utilities=utilities;
        }

        [HttpPost("random")]
        public IActionResult InsertRandomNames()
        {
            _utilities.InsertRandomNames(10);
            return Ok("Random names inserted successfully.");
        }

        [HttpGet("alphabetical")]
        public async Task<IActionResult> GetUsersAlphabetically()
        {
            var sortedDrivers = await _utilities.GetUsersAlphabetically();
            return Ok(sortedDrivers);
        }

        [HttpGet("alphabetized")]
        public async Task<IActionResult> GetAlphabetizedName(int driverId)
        {
            var alphabetizedName = await _utilities.GetAlphabetizedName(driverId);
            return Ok(alphabetizedName);
        }

        [HttpPost]
        public async Task<IActionResult> AddDriver([FromBody] DriverRequest driverRequest)
        {

            ValidationResult validationResult = _validator.Validate(driverRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            try
            {
                // Add the driver using the repository
               var addDriver=  await _driverRepository.AddDriver(driverRequest);

                if (addDriver > 0)
                {
                    return Ok("Driver added successfully.");
                }

                return BadRequest("Driver Not added .");

            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while adding a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            try
            {
                // Check if the driver with the specified id exists
                var existingDriver = await _driverRepository.GetDriverById(id);

                if (existingDriver == null)
                {
                    return NotFound("Driver not found.");
                }

                // Delete the driver using the repository
               var deleted = await _driverRepository.DeleteDriver(id);
                if (deleted > 0)
                {
                    return Ok("Driver deleted successfully.");
                }

                return BadRequest("Driver not deleted .");

            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while deleting a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] DriverRequest driverRequest)
        {
            ValidationResult validationResult = _validator.Validate(driverRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            try
            {
                // Check if the driver with the specified id exists
                var existingDriver = await _driverRepository.GetDriverById(id);

                if (existingDriver == null)
                {
                    return NotFound("Driver not found.");
                }

                // Update the driver using the repository
              var driverUpdated =   await _driverRepository.UpdateDriver(id, driverRequest);

                if (driverUpdated > 0)
                {

                    return Ok("Driver updated successfully.");
                }

                return BadRequest("Driver Not updated .");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while updating a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            try
            {
                // Retrieve the driver by id using the repository
                var driver = await _driverRepository.GetDriverById(id);

                if (driver == null)
                {
                    return NotFound("Driver not found.");
                }

                return Ok(driver);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while retrieving a driver.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var drivers = await _driverRepository.GetAllDrivers();
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while retrieving all drivers.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
