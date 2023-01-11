using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;
using WebApplication4.Models.DTOs.Requests;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private PgagoContext _context;

        public ClientsController(PgagoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var context = new PgagoContext();
            var client = context.Clients
                                .Select(c => new
                                {
                                    cos = c.LastName
                                });
            return Ok(client);
        }

        [HttpPost]
        public IActionResult CreateClientAndTrip(CreateClientAndTripRequestDto request)
        {

            var client = new Client
            {
                LastName = request.LastName
            };
            var trip = new Trip
            {
                Name = request.TripName
            };

            return Ok();
        }


        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(x => x.IdClient == idClient);
            var result = await (from ct in _context.ClientTrips where ct.IdClient == idClient select ct).ToListAsync();

            await _context.SaveChangesAsync();

            if (result.Count > 0)
            {
                return Ok("Client deleted");
            }

            return BadRequest("Client has trips or doesn't exists");
        }


    }
}
