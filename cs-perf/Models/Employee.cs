﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CS_Perf.Models
{
    [Serializable]
    class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public List<string> Fields { get; set; }

        public Employee()
        {

        }

        public Employee(int id, string firstName, string[] fields)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.Fields = new List<string>(fields);
        }
    }   
}
