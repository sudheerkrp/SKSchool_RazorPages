using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using SKSchool.Pages.Students;
using System.Data.SqlClient;

namespace SKSchool.Pages.Teachers
{
	public class IndexModel : PageModel
	{
		private readonly IDatabaseConnection databaseConnection;
		public List<TeachersInfoJoinBranches> teachersList = new();
		public TeachersInfo info = new();
		public List<BranchesInfo> branchesList = new();
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
				string sql1 = @"SELECT id, Teachers.name AS name, branchCode, Teachers.update_on AS updatedOn, Branches.name AS branchName FROM Teachers JOIN Branches ON Teachers.branchCode = Branches.code WHERE Teachers.active_bit = 1 AND Branches.active_bit = 1 ORDER BY CAST(Teachers.name AS varchar)";
				string sql2 = @"SELECT code, name FROM Branches WHERE active_bit = 1";
				teachersList = new(await connection.QueryAsync<TeachersInfoJoinBranches>(sql1));
				branchesList = new(await connection.QueryAsync<BranchesInfo>(sql2));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception Index : " + ex.ToString());
			}
		}

		public async Task OnPostDeleteTeacher()
		{
			try
			{
				Guid reqId = new(Request.Form["id"]);
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"UPDATE Teachers SET active_bit = 0, update_on = @currentDateTime WHERE id = @id";
				await connection.ExecuteAsync(sql, new { id = reqId, currentDateTime = DateTime.UtcNow});
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception Delete : " + ex.Message);
			}
			Response.Redirect("/Teachers");
		}

		public async Task OnPostEditTeacher()
		{
			info.Id = new(Request.Form["id"]);
			info.Name = Request.Form["name"];
			if (info.Name.Length == 0 || Request.Form["branch_code"] == "select")
			{
				errorMsg = "Branch Name is required!";
				Console.WriteLine("Eidt.cs2 : " + errorMsg);
				return;
			}
			info.BranchCode = new(Request.Form["branch_code"]);

			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"UPDATE Teachers SET name = @name, branchCode = @branchCode, update_on = @currentDateTime WHERE id = @id";
				await connection.ExecuteAsync(sql, new { name = info.Name, branchCode = info.BranchCode, currentDateTime = DateTime.UtcNow, id = info.Id });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				Console.WriteLine("Eidt.cs3 : " + errorMsg);
				return;
			}
			Response.Redirect("/Teachers");
		}
	}
}