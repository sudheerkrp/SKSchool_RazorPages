namespace SKSchool.DataClass
{
	public class StudentsInfo
	{
		public Guid RollNo { get; set; }
		public string? Name { get; set; }
		public Guid BranchCode { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
	}

	public class StudentsInfoJoinBranches : StudentsInfo
	{
		public string? BranchName { get; set; }
	}
}