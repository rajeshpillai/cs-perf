using System;
using System.Collections.Generic;
using System.Text;

namespace CS_Perf.Models
{
    [Serializable]
    class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public Employee()
        {

        }

        public Employee(int id, string firstName)
        {
            this.Id = id;
            this.FirstName = firstName;
        }
    }   
}
