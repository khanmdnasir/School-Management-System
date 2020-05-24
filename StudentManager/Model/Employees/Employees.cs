using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.Model.Employees
{
    public class Employees
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NICNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public int GenderId { get; set; }
        public DateTime EmploymentDate { get; set; }
        public int BranchId { get; set; }
        public byte[] Photo { get; set; }
        public string AddressLine { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string PostCode { get; set; }
        public int JObTitleId { get; set; }
        public decimal CurrentSalary { get; set; }
        public decimal StartingSalary { get; set; }
        public bool HasLeft { get; set; }
        public DateTime DateLeft { get; set; }
        public int ReasonLeftId { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
    }
}
