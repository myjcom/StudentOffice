using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StudentOffice.Entities
{
    [Index(nameof(SpecId))]
    public class Specialization
    {
        [Key]
        public int SpecId { get; set; }
        public string SpecName { get; set; }

        public virtual Faculty Faculty { get; set; }
    }
}
