namespace SKSchool.DataClass
{
	public class SubjectsEnrollmentInfo
	{
		public Guid SubjCode { get; set; }
		public Guid BranchCode { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
	}

	public class SubjectsEnrollmentInfoJoinBranchesSubjects : SubjectsEnrollmentInfo
	{
		public string? SubjectName;
		public string? BranchName;
	}
}