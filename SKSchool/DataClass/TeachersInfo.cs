namespace SKSchool.DataClass
{
	public class TeachersInfo
	{
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public Guid BranchCode { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
	}

	public class TeachersInfoJoinBranches : TeachersInfo
	{
		public string? BranchName { get; set; }
	}
}
