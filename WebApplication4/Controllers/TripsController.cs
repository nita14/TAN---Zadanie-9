using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;
using WebApplication4.Models.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {

        private PgagoContext _context;

        public TripsController(PgagoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTripsFromDbAsync()
        {


            var result = await _context.Trips.Include(x => x.ClientTrips).ThenInclude(y => y.IdClientNavigation)
               .Include(x => x.CountryTrips).ThenInclude(y => y.IdCountryNavigation)
               .Select(x => new GetClientStatistiscsResponseDto
               {
                   Name = x.Name,
                   Description = x.Description,
                   DateFrom = x.DateFrom,
                   DateTo = x.DateTo,
                   MaxPeople = x.MaxPeople,
                   Clients = x.ClientTrips.Select(y => new ClientDTO { FirstName = y.IdClientNavigation.FirstName }).ToHashSet(),
                   Countries = x.CountryTrips.Select(y => new CountryDTO { Name = y.IdCountryNavigation.Name }).ToHashSet(),
               })
               .OrderByDescending(x => x.DateFrom)
               .ToListAsync();

            return Ok(result);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> UpdateClients(ClientDTO clients)
        {

            bool result = true;
            var isInDb = await _context.Clients.SingleOrDefaultAsync(x => x.Pesel == clients.Pesel);
            var isTrip = await _context.Trips.AnyAsync(x => x.IdTrip == clients.IdTrip);
            var isClientInTrip = await _context.ClientTrips.AnyAsync(x => x.IdTrip == clients.IdTrip);

            if (isInDb != null || isTrip || isClientInTrip)
            {
                result = false;
            }

    
            var client = _context.Clients.Max(x => x.IdClient);
            var newClient = await _context.AddAsync(new Client
            {
                IdClient = client + 1,
                FirstName = clients.FirstName,
                LastName = clients.LastName,
                Email = clients.Email,
                Telephone = clients.Telephone,
                Pesel = clients.Pesel,

            });

            var clientTrip = await _context.AddAsync(new ClientTrip
            {
                IdClient = client + 1,
                IdTrip = clients.IdTrip,
                PaymentDate = DateTime.ParseExact(clients.PaymentDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                RegisteredAt = DateTime.Now
            });


            var trip = await _context.AddAsync(new Trip
            {
                IdTrip = clients.IdTrip,
                Name = clients.TripName,
            });


            if (result)
            {
                return Ok("Client added");
            }

            return BadRequest("Something went wrong");
        }
    }
}
