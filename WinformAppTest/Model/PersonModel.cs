using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinformAppTest.Model
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }


    public class PersonModel
    {
        public List<Person> GetPersons()
        {
            return new List<Person>()
            {
                new Person() { Name = "John Doe", Age = 30, City = "New York" },
                new Person() { Name = "Jane Smith", Age = 25, City = "Los Angeles" },
                new Person() { Name = "Bob Johnson", Age = 40, City = "Chicago" }
            };

        }
    }
}
