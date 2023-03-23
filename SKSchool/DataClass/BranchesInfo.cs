namespace SKSchool.DataClass
{
    public class BranchesInfo
    {
        public Guid Code { get; set; }
        public string? Name { get; set; }
        public bool ActiveBit { get; set; } = true;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
