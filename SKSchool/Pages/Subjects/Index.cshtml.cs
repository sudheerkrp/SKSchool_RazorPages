using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SKSchool.Pages.Subjects
{
    public class IndexModel : PageModel
    {
		public List<SubjectsInfo> subjectsList = new();

		//[Inject]
		//public IDatabaseConnection Dbc { get; set; }

		public async Task OnGet()
		{
			try
			{
				/*using var Connection = Dbc.GetConnection();
				string Sql = @"SELECT * FROM Branches WHERE active_bit = 1";
				BranchesList = (List<BranchesInfo>)await Connection.QueryAsync<BranchesInfo>(Sql);*/

				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"SELECT * FROM Subjects WHERE active_bit = 1";
				subjectsList = (List<SubjectsInfo>)await connection.QueryAsync<SubjectsInfo>(sql);

			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}
	}

	public class SubjectsInfo
	{
		//[Inject]
		//public IDatabaseConnection Dbc { get; set; }

		public Guid Code { get; set; }
		public string Name { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;

		public SubjectsInfo()
		{
			Name = "";
		}
		public SubjectsInfo(Guid code, string name, bool activeBit, DateTime updatedOn)
		{
			Code = code;
			Name = name;
			ActiveBit = activeBit;
			UpdatedOn = updatedOn;
		}
	}
}
