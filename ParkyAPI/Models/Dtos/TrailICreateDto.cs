using System.ComponentModel.DataAnnotations;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.Dtos
{
    public class TrailICreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public DiffcultyType Diffculty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        [Required]
        public double Elevation { get; set; }
    }
}
