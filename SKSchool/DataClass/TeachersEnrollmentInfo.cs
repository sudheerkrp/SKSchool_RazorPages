namespace SKSchool.DataClass
{
    public class TeachersEnrollmentInfo
    {
        public Guid EmpId { get; set; }
        public Guid SubjCode { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public bool ActiveBit { get; set; } = true;
    }
}
