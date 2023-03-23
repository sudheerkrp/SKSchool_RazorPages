using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.Branches
{
    public class IndexModel : PageModel
    {
		private readonly IDatabaseConnection databaseConnection;
		public List<BranchesInfo> branchesList = new();
		public BranchesInfo info = new();
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
				string sql = @"SELECT code, name, update_on AS updatedOn FROM Branches WHERE active_bit = 1";
				branchesList = new(await connection.QueryAsync<BranchesInfo>(sql));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}

		public async Task OnPostDeleteBranch()
		{
			try
			{
				Guid reqCode = new(Request.Form["code"]);
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"UPDATE Branches SET active_bit = 0, update_on = @currentDateTime WHERE code = @code";
				await connection.ExecuteAsync(sql, new { code = reqCode, currentDateTime = DateTime.UtcNow });
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.Message);
			}
			Response.Redirect("/Branches");
		}

		public async Task OnPostEditBranch()
		{
			info.Code = new Guid(Request.Form["code"]);
			info.Name = Request.Form["name"];
			if (info.Name.Length == 0)
			{
				errorMsg = "Branch Name is required!";
				return;
			}

			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"UPDATE Branches SET name = @name, update_on = @currentDateTime WHERE code = @code";
				await connection.ExecuteAsync(sql, new { name = info.Name, currentDateTime = DateTime.UtcNow, code = info.Code });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect("/Branches");
		}
	}
}