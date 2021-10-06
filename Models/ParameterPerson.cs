using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSingleSignOn.Models
{
    public class ParameterPerson
    {
        public Int64 Person_ID { get; set; }
        public string Record_ID { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Father_Name { get; set; }
        public DateTime DOB { get; set; }
        public byte Gender_ID { get; set; }
        public byte Category_ID { get; set; }
        public IEnumerable<Person_Address> Person_Address { get; set; }
        public byte[] Profile_Photo { get; set; }
    }
    public class Person_Address
    { 
        public Int64 Person_Address_ID { get; set; }
        public Int64 Person_ID { get; set; }
        public string House_No { get; set; }
        public string Mohalla { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PIN { get; set; }
        public byte Person_Address_Type { get; set; }
        public string Record_ID { get; set; }

    }
}
