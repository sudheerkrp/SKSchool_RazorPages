using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.Branches
{
    public class IndexModel : PageModel
    {
		public List<BranchesInfo> branchesList = new();

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
				string sql = @"SELECT * FROM Branches WHERE active_bit = 1";
				branchesList = new(await connection.QueryAsync<BranchesInfo>(sql));

			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}
	}

	public class BranchesInfo
	{
		//[Inject]
		//public IDatabaseConnection Dbc { get; set; }

		public Guid Code { get; set; }
		public string Name { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;

		public BranchesInfo()
		{
			Name = "";
		}
		public BranchesInfo(Guid code, string name, bool activeBit, DateTime updatedOn)
		{
			Code = code;
			Name = name;
			ActiveBit = activeBit;
			UpdatedOn = updatedOn;
		}
	}
}
