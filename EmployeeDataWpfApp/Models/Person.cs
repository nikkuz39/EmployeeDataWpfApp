using System;
using System.Collections.Generic;

#nullable disable

namespace EmployeeDataWpfApp
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateEmploy { get; set; }
        public DateTime? DateUnemploy { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }
        
        public int DepId { get; set; }
        public Dep Dep { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
