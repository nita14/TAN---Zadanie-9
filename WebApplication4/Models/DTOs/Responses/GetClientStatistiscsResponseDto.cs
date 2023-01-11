using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models.DTOs.Responses
{
    public class GetClientStatistiscsResponseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }

        public HashSet<ClientDTO> Clients { get; set; }
        public HashSet<CountryDTO> Countries { get; set; }
    }
}
