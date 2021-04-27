using System;
namespace Lab_Practice.Models
{
    public class EmployeeClub   // Mapping table for many to many Employee with Club 
    {
        /* 
         * This mapping class/table can be skipped in EF 5.0
         * Just adding opposite entity Collection in each of the entities 
         * Composite key and mapping table will be generated in the DB automatically 
         */



        public DateTime JoinDate { get; set; }


        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }





        public int ClubId { get; set; }

        public Club Club { get; set; }


    }
}
