using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.Branches
{
    public class CreateModel : PageModel
    {
		private readonly IDatabaseConnection databaseConnection;
		public BranchesInfo info = new();
        public string errorMsg = "";

		public CreateModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

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
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"INSERT INTO Branches(code, name, update_on) VALUES(newid(), @name, @currentDateTime)";
				await connection.ExecuteAsync(sql, new { name = info.Name, currentDateTime = info.UpdatedOn });
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
