using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SKSchool.Pages.Branches
{
    public class EditModel : PageModel
    {
		public BranchesInfo info = new();
		public string errorMsg = "";

		public async Task OnGet()
		{
			Guid reqCode = new(Request.Query["code"]);
			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"SELECT * FROM Branches WHERE code = @code";
				info = (BranchesInfo)await connection.QuerySingleAsync<BranchesInfo>(sql, new { code = reqCode });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
			}
		}

		public async Task OnPost()
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
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"UPDATE Branches SET name = @name, update_on = @currentDateTime WHERE code = @code";
				await connection.ExecuteAsync(sql, new { name = info.Name, currentDateTime = DateTime.Now, code = info.Code });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}

			Response.Redirect("/Branches/Index");
		}
	}
}
