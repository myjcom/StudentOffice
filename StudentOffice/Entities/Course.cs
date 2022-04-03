using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;

namespace StudentOffice.Entities
{
    [Index(nameof(CourseId))]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public int CourseNumber { get; set; }
    }
}
