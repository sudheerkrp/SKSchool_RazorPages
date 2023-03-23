namespace SKSchool.DataClass
{
	public class StudentsEnrollmentInfo
	{
		public Guid RollNo { get; set; }
		public Guid SubjCode { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
	}

	public class StudentsEnrollmentInfoJoinSubjectsTeachers : StudentsEnrollmentInfo
	{
		public string? SubjectName { get; set; }
		public string? TeacherName { get; set; }
	}
}