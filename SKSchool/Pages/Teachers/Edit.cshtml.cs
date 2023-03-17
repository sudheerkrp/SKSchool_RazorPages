using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.Pages.Branches;
using System.Data.SqlClient;

namespace SKSchool.Pages.Teachers
{
	public class EditModel : PageModel
	{
		public TeachersInfo info = new();
		public List<BranchesInfo> branchesList = new();
		public string errorMsg = "";

		public async Task OnGet()
		{
			Guid reqId = new(Request.Query["id"]);
			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql1 = @"SELECT * FROM Teachers WHERE id = @id";
				string sql2 = @"SELECT * FROM Branches WHERE active_bit = 1";
				info = await connection.QuerySingleAsync<TeachersInfo>(sql1, new { id = reqId });
				branchesList = new(await connection.QueryAsync<BranchesInfo>(sql2));
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				Console.WriteLine("Eidt.cs1 : " + errorMsg);
			}
		}

		public async Task OnPost()
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
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"UPDATE Teachers SET name = @name, branchCode = @branchCode, update_on = @currentDateTime WHERE id = @id";
				await connection.ExecuteAsync(sql, new { name = info.Name, branchCode = info.BranchCode, currentDateTime = DateTime.Now, id = info.Id });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				Console.WriteLine("Eidt.cs3 : " + errorMsg);
				return;
			}

			Response.Redirect("/Teachers/Index");
		}
	}
}
