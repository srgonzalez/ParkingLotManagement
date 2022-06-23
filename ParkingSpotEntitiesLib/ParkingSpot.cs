using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Packt.Shared
{
    [Table("ParkingSpot")]
    public partial class ParkingSpot
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [Column(TypeName = "nvarchar (7)")]
        [StringLength(7)]
        public string LicensePlate { get; set; } = null!;
        [Column(TypeName = "bit")]
        public bool Ocuppied { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateEntered { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DepartureDate { get; set; }
        [Column(TypeName = "money")]
        public decimal Rate { get; set; }
    }
}
