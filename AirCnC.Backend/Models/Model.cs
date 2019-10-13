using System;
using System.ComponentModel.DataAnnotations;

namespace AirCnC.Backend.Models
{
    public abstract class Model
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
