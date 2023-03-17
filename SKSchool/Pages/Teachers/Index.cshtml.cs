using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using SKSchool.Pages.Students;
using System.Data.SqlClient;

namespace SKSchool.Pages.Teachers
{
	public class IndexModel : PageModel
	{
		public List<TeachersInfo> teachersList = new();
		public Dictionary<Guid, string> branchMapping = new();

		//[Inject]
		//public IDatabaseConnection Dbc { get; set; }

		public async Task OnGet()
		{
			try
			{
				/*using SqlConnection connection = Dbc.GetConnection();
				string Sql = @"SELECT * FROM Branches WHERE active_bit = 1";
				BranchesList = (List<BranchesInfo>)await Connection.QueryAsync<BranchesInfo>(Sql);*/

				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql1 = @"SELECT * FROM Teachers WHERE active_bit = 1";
				string sql2 = @"SELECT code, name FROM Branches WHERE active_bit = 1";
				teachersList = new(await connection.QueryAsync<TeachersInfo>(sql1));
				List<BranchInfo> branches = new(await connection.QueryAsync<BranchInfo>(sql2));
				foreach (var ele in branches)
					branchMapping.Add(ele.Code, ele.Name);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception Index : " + ex.ToString());
			}
		}
	}

	public class TeachersInfo
	{
		//[Inject]
		//public IDatabaseConnection Dbc { get; set; }

		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid BranchCode { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;

		public TeachersInfo()
		{
			Name = "";
		}
		public TeachersInfo(Guid id, string name, Guid branchCode, bool activeBit, DateTime updatedOn)
		{
			Id = id;
			Name = name;
			BranchCode = branchCode;
			ActiveBit = activeBit;
			UpdatedOn = updatedOn;
		}
	}
}
