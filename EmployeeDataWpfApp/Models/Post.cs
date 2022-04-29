using System;
using System.Collections.Generic;

#nullable disable

namespace EmployeeDataWpfApp
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Person> Persons { get; set; }
    }
}
