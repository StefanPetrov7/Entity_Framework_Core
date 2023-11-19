using ORM_CodeFirst.Models;

namespace ORM_CodeFirst
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AplicationDBCOntext dbCOdeFirst = new AplicationDBCOntext();
            dbCOdeFirst.Database.EnsureCreated();





        }
    }
}
