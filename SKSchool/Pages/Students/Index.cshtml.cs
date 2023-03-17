using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;
using System.Net.WebSockets;

namespace SKSchool.Pages.Students
{
	public class IndexModel : PageModel
	{
		public List<StudentsInfo> studentsList = new();
		public Dictionary<Guid, string> branchMapping = new();

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
				string sql1 = @"SELECT * FROM Students WHERE active_bit = 1";
				string sql2 = @"SELECT code, name FROM Branches WHERE active_bit = 1";
				studentsList = new(await connection.QueryAsync<StudentsInfo>(sql1));
				List<BranchInfo> branches = new(await connection.QueryAsync<BranchInfo>(sql2));
				foreach (var ele in branches)
					branchMapping.Add(ele.Code, ele.Name);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}
	}

	public class StudentsInfo
	{
		//[Inject]
		//public IDatabaseConnection Dbc { get; set; }

		public Guid RollNo { get; set; }
		public string Name { get; set; }
		public Guid BranchCode { get; set; }
		public bool ActiveBit { get; set; } = true;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;

		public StudentsInfo()
		{
			Name = "";
		}
		public StudentsInfo(Guid rollNo, string name, Guid branchCode, bool activeBit, DateTime updatedOn)
		{
			RollNo = rollNo;
			Name = name;
			BranchCode = branchCode;
			ActiveBit = activeBit;
			UpdatedOn = updatedOn;
		}
	}
	public class BranchInfo
	{
		public Guid Code { get; set; }
		public string Name { get; set; }
		public BranchInfo()
		{
			Name = "";
		}
		public BranchInfo(Guid code, string name)
		{
			Code = code;
			Name = name;
		}
	}
}
