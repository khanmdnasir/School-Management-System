using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.Model
{
    public class Students
    {

        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string Mobile { get; set; }
        public string BloodGroup { get; set; }
        public int GenderId { get; set; }
        public int BranchId { get; set; }
        public byte[] Photo { get; set; }
        public string CreatedBy { get; set; }
        

    }
}
