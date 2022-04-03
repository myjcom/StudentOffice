using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StudentOffice.Entities
{
    [Index(nameof(PassId))]
    public class Passport
    {
        [Key]
        public int PassId { get; set; }
        public int PassSeries {get; set;}

        public int PassNumber { get; set; }

        public string Issued { get; set; }

        public string DateOfIssue { get; set; }

        public string Location { get; set; }

        public string PassCode { get; set; }

        public string Family { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string BrithDate { get; set; }

        public string Gender { get; set; }

        public string Sitizenship { get; set; }

        public string RegistrationAdress { get; set; }
        public string FactAdress { get; set; }
    }
}
