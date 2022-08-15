using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkyWeb.Models
{
    public class Trail
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }
        public DiffcultyType Diffculty { get; set; }
        [Required]

        public int NationalParkId { get; set; }
        [ValidateNever]
        public NationalPark NationalPark { get; set; }
        public enum DiffcultyType { Easy, Moderate, Difficult, Expert }

    }
}
