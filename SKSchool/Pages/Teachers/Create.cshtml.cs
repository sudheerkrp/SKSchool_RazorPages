using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using SKSchool.Pages.Branches;
using System.Data.SqlClient;

namespace SKSchool.Pages.Teachers
{
	public class CreateModel : PageModel
	{
		private readonly IDatabaseConnection databaseConnection;
		public TeachersInfo info = new();
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
				branchesList = new(await connection.QueryAsync<BranchesInfo>(sql));
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				Console.WriteLine("Create.cs1 : " + errorMsg);
				return;
			}
		}

		public async Task OnPost()
		{
			info.Name = Request.Form["name"];
			if (info.Name.Length == 0 || Request.Form["branch_code"] == "select")
			{
				errorMsg = "Branch & Name are required!";
				Console.WriteLine("Create3.cs : " + errorMsg);
				return;
			}
			info.BranchCode = new(Request.Form["branch_code"]);

			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"INSERT INTO Teachers(id, name, branchCode, active_bit, update_on) VALUES(newid(), @name, @branchCode, 1, @currentDateTime)";
				await connection.ExecuteAsync(sql, new { name = info.Name, branchCode = info.BranchCode, currentDateTime = info.UpdatedOn });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				Console.WriteLine("Create.cs2 : " + errorMsg);
				return;
			}
			info.Name = "";
			Response.Redirect("/Teachers");
		}


	}
}
