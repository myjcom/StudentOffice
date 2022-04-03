using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace StudentOffice.Entities
{
    [Index(nameof(ContractId))]
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; }
        public string ContractDateAdmission { get; set; }
    }
}
