using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using SKSchool.Pages.Branches;
using System.Data.SqlClient;

namespace SKSchool.Pages.Students
{
	public class CreateModel : PageModel
	{
		private readonly IDatabaseConnection databaseConnection;
		public StudentsInfo info = new();
		public List<BranchesInfo> branchesList = new();
		public string errorMsg = "";

		public CreateModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnGet()
		{
			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
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
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"INSERT INTO Students(rollNo, name, branchCode, update_on) VALUES(newid(), @name, @branchCode, @currentDateTime)";
				await connection.ExecuteAsync(sql, new { name = info.Name, branchCode = info.BranchCode, currentDateTime = info.UpdatedOn });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect("/Students");
		}
	}
}