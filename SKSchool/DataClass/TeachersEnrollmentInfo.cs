namespace SKSchool.DataClass
{
	public class TeachersEnrollmentInfo
	{
		public Guid Id { get; set; }
		public Guid SubjCode { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
	}

	public class TeachersEnrollmentInfoJoinSubjects : TeachersEnrollmentInfo
	{
		public string? SubjectName { get; set; }
	}
}