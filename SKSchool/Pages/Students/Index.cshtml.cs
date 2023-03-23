using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.Students
{
	public class IndexModel : PageModel
	{
		private readonly IDatabaseConnection databaseConnection;
		public List<StudentsInfoJoinBranches> studentsList = new();
		public List<BranchesInfo> branchesList = new();
		public StudentsInfo info = new();
		public string errorMsg = "";

		public IndexModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnGet()
		{
			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT rollNo, Students.name AS name, branchCode, Students.update_on AS updatedOn, Branches.name AS branchName FROM Students JOIN Branches ON Students.branchCode = Branches.code WHERE Students.active_bit = 1 AND Branches.active_bit = 1 ORDER BY CAST(Students.name AS varchar)";
				string sql2 = @"SELECT code, name FROM Branches WHERE active_bit = 1";
				studentsList = new(await connection.QueryAsync<StudentsInfoJoinBranches>(sql1));
				branchesList = new(await connection.QueryAsync<BranchesInfo>(sql2));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}

		public async Task OnPostDeleteStudent()
		{
			try
			{
				Guid reqRollNo = new(Request.Form["roll_no"]);
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"UPDATE Students SET active_bit = 0, update_on = @currentDateTime WHERE rollNo = @rollNo";
				await connection.ExecuteAsync(sql, new { rollNo = reqRollNo, currentDateTime = DateTime.UtcNow });
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception Delete : " + ex.Message);
			}
			Response.Redirect("/Students");
		}

		public async Task OnPostEditStudent()
		{
			info.RollNo = new Guid(Request.Form["roll_no"]);
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
				string Sql = @"UPDATE Students SET name = @name, branchCode = @branchCode, update_on = @currentDateTime WHERE rollNo = @rollNo";
				await connection.ExecuteAsync(Sql, new { name = info.Name, branchCode = info.BranchCode, currentDateTime = DateTime.UtcNow, rollNo = info.RollNo });
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