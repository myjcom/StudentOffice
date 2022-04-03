using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StudentOffice.Entities
{
    [Index(nameof(FacId))]
    public class Faculty
    {
        [Key]
        public int FacId { get; set; }
        public string FacName { get; set; }
    }
}
