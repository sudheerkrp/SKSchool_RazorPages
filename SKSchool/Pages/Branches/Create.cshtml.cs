using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SKSchool.Pages.Branches
{
    public class CreateModel : PageModel
    {
        public BranchesInfo info = new();
        public string errorMsg = "";

		public async Task OnPost()
		{
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
				string sql = @"INSERT INTO Branches(code, name, update_on) VALUES(newid(), @name, @currentDateTime)";
				await connection.ExecuteAsync(sql, new { name = info.Name, currentDateTime = info.UpdatedOn });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			info.Name = "";
			Response.Redirect("/Branches/Index");
		}


	}
}
