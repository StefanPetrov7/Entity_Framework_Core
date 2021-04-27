using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_Practice.Models
{
    public class Address
    {

        [Key]
        public int Id { get; set; }



        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }  // one to one with employee

        public Employee Employee { get; set; }  // one to one with employee
    } 
}
