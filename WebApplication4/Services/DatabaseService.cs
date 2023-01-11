using System.Collections.Generic;
using System.Linq;
using WebApplication4.Models;
using WebApplication4.Models.DTOs.Responses;

namespace WebApplication4.Services
{
    public interface IDbService
    {

    }

    public class DatabaseService : IDbService
    {
        private readonly PgagoContext _context;

        public DatabaseService(PgagoContext context)
        {
            _context = context;
        }

        public IEnumerable<dynamic> GetReport()
        {
            return _context.Clients
                           .Select(c => new
                           {

                           });
        }
    }
}
