using System;
using System.ComponentModel.DataAnnotations;

namespace AirCnC.Shared.Models
{
    public abstract class Model
    {
        [Key]
        public Guid Guid { get; set; }
    }
}
