using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StudentOffice.Entities
{
    [Index(nameof(RepId))]
    public class Representant
    {
        [Key]
        public int RepId { get; set; }
        public string RepFullName { get; set; }
        public string RepAdress { get; set; }
        public string RepPhone { get; set; }
        public string RepEmail { get; set; }

        public string RepDocType { get; set; }
        public string RepDoc { get; set; }

        public int PassId { get; set; }

        public virtual Passport Passport { get; set; }
    }
}
