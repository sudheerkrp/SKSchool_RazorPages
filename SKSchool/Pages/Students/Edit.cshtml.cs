using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using SKSchool.Pages.Branches;
using SKSchool.Pages.Teachers;
using System.Data.SqlClient;

namespace SKSchool.Pages.Students
{
	public class EditModel : PageModel
	{
		public List<BranchesInfo> branchesList = new();
		public StudentsInfo info = new();
		public string errorMsg = "";

		public async Task OnGet()
		{
			Guid reqRollNo = new(Request.Query["roll_no"]);
			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql1 = @"SELECT * FROM Students WHERE rollNo = @rollNo";
				string sql2 = @"SELECT * FROM Branches WHERE active_bit = 1";
				info = (StudentsInfo)await connection.QuerySingleAsync<StudentsInfo>(sql1, new { rollNo = reqRollNo });
				branchesList = new(await connection.QueryAsync<BranchesInfo>(sql2));
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
			}
		}

		public async Task OnPost()
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
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string Sql = @"UPDATE Students SET name = @name, branchCode = @branchCode, update_on = @currentDateTime WHERE rollNo = @rollNo";
				await connection.ExecuteAsync(Sql, new { name = info.Name, branchCode = info.BranchCode, currentDateTime = DateTime.Now, rollNo = info.RollNo });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}

			Response.Redirect("/Students/Index");
		}
	}
}
