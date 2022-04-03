using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace StudentOffice.Entities
{
    [Index(nameof(StudentId))]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }

        public string DocType { get; set; }

        public virtual Passport Passport { get; set; }

        public virtual Contract Contract { get; set; }

        public virtual Specialization Specialization { get; set; }

        public virtual Representant Representant { get; set; }

        public virtual Faculty Faculty { get; set; }

        public virtual Course Course { get; set; }

    }
}
