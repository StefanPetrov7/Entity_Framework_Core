using System.Collections.Generic;

namespace Lab_Practice.Models
{
    public class Club
    {
        public Club()
        {
            this.EmployeeClub = new HashSet<EmployeeClub>();
        }

        public int Id { get; set; }

        public string Name { get; set; }


        public ICollection<EmployeeClub> EmployeeClub { get; set; }   // mapping table collection
    }
}
