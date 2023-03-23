using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.Subjects
{
	public class EditModel : PageModel
	{
		public SubjectsInfo info = new();
		public string errorMsg = "";

		public async Task OnGet()
		{
			Guid reqCode = new(Request.Query["code"]);
			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"SELECT * FROM Subjects WHERE code = @code";
				info = (SubjectsInfo)await connection.QuerySingleAsync<SubjectsInfo>(sql, new { code = reqCode });
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
				errorMsg = "Subject Name is required!";
				return;
			}

			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string Sql = @"UPDATE Subjects SET name = @name, update_on = @currentDateTime WHERE code = @code";
				await connection.ExecuteAsync(Sql, new { name = info.Name, currentDateTime = DateTime.Now, code = info.Code });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}

			Response.Redirect("/Subjects/Index");
		}
	}
}
