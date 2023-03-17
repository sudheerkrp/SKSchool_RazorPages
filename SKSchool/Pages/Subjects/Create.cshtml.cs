using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SKSchool.Pages.Subjects
{
	public class CreateModel : PageModel
	{
		public SubjectsInfo info = new();
		public string errorMsg = "";

		public async Task OnPost()
		{
			info.Name = Request.Form["name"];
			if (info.Name.Length == 0)
			{
				errorMsg = "Subject Name is required!";
				return;
			}

			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"INSERT INTO Subjects(code, name, update_on) VALUES(newid(), @name, @currentDateTime)";
				await connection.ExecuteAsync(sql, new { name = info.Name, currentDateTime = info.UpdatedOn });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			info.Name = "";
			Response.Redirect("/Subjects/Index");
		}


	}
}
