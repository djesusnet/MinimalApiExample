using Microsoft.AspNetCore.Mvc;
using OldAPIExample.Models;
using OldAPIExample.Repositories.Interfaces.Interfaces;

namespace OldAPIExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepositoryCustomer _repositoryCustomer;

        public CustomerController(IRepositoryCustomer repositoryCustomer)
        {
            _repositoryCustomer = repositoryCustomer;
        }

        [HttpGet(nameof(GetCustomer))]
        public IActionResult GetCustomer(int id)
        {
            var result = _repositoryCustomer.Get(id);
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest("No records found");
        }

        [HttpGet(nameof(GetAllCustomer))]
        public IActionResult GetAllCustomer()
        {
            var result = _repositoryCustomer.GetAll();
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest("No records found");
        }

        [HttpPost(nameof(InsertCustomer))]
        public IActionResult InsertCustomer(Customer customer)
        {
            _repositoryCustomer.Insert(customer);
            return Ok("Data inserted");
        }

        [HttpPut(nameof(UpdateCustomer))]
        public IActionResult UpdateCustomer(Customer customer)
        {
            _repositoryCustomer.Update(customer);
            return Ok("Updation done");
        }

        [HttpDelete(nameof(DeleteCustomer))]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _repositoryCustomer.Get(id);
            _repositoryCustomer.Delete(customer);
            return Ok("Data Deleted");
        }

    }
}
