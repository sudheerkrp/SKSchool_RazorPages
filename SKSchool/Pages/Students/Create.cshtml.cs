using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.Pages.Branches;
using System.Data.SqlClient;

namespace SKSchool.Pages.Students
{
	public class CreateModel : PageModel
	{
		public StudentsInfo info = new();
		public List<BranchesInfo> branchesList = new();
		public string errorMsg = "";

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
				branchesList = (List<BranchesInfo>)await connection.QueryAsync<BranchesInfo>(sql);

			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}

		public async Task OnPost()
		{
			info.Name = Request.Form["name"];
			if (info.Name.Length == 0 || Request.Form["branch_code"] == "select")
			{
				errorMsg = "Student and Branch Name are required!";
				return;
			}
			info.BranchCode = new(Request.Form["branch_code"]);

			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"INSERT INTO Students(rollNo, name, branchCode, update_on) VALUES(newid(), @name, @branchCode, @currentDateTime)";
				await connection.ExecuteAsync(sql, new { name = info.Name, branchCode = info.BranchCode, currentDateTime = info.UpdatedOn });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			info.Name = "";
			Response.Redirect("/Students/Index");
		}


	}
}
