using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;
using Dapper;

namespace SKSchool.Pages.TeachersEnrollment
{
	public class CreateModel : PageModel
	{
		private readonly IDatabaseConnection databaseConnection;
		public List<SubjectsInfo> subjectsList = new();
		public TeachersEnrollmentInfo info = new();
		public string errorMsg = "";

		public CreateModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnGet()
		{
			try
			{
				Guid id;
				if (!(Guid.TryParse(Request.Query["id"], out id)))
				{
					errorMsg = "Invalid Data!";
					return;
				}
				info.Id = id;
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Teachers WHERE id = @id AND active_bit = 1";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { id });
				if (cnt != 1)
				{
					errorMsg = "Invalid Data!";
					return;
				}
				string sql2 = @"SELECT branchCode FROM Teachers WHERE id = @id AND active_bit = 1";
				Guid branchCode = await connection.QuerySingleAsync<Guid>(sql2, new { id });
				string sql3 = @"SELECT code, name FROM Subjects JOIN Subjects_Enrollment ON Subjects.code = Subjects_Enrollment.subjCode WHERE Subjects_Enrollment.branchCode = @branchCode AND Subjects.active_bit = 1 AND Subjects_Enrollment.active_bit = 1";
				subjectsList = new(await connection.QueryAsync<SubjectsInfo>(sql3, new { branchCode }));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}

		public async Task OnPost()
		{
			info.Id = new(Request.Form["id"]);
			if (Request.Form["subj_code"] == "select")
			{
				errorMsg = "Subject Name is Required!";
				return;
			}
			info.SubjCode = new(Request.Form["subj_code"]);
			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Teachers_Enrollment WHERE empId = @id AND subjCode = @subjectCode AND active_bit = 1";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { id = info.Id, subjectCode = info.SubjCode });
				if (cnt != 0)
				{
					errorMsg = "You are already enrolled in this Subject!";
					return;
				}
				string sql2 = @"SELECT COUNT(*) FROM Teachers_Enrollment WHERE subjCode = @subjectCode AND active_bit = 1";
				cnt = await connection.QuerySingleAsync<int>(sql2, new {subjectCode = info.SubjCode });
				if (cnt != 0)
				{
					errorMsg = "Someone already teaching this subject!";
					return;
				}
				string sql3 = @"SELECT COUNT(*) FROM Teachers_Enrollment WHERE empId = @id AND subjCode = @subjectCode AND active_bit = 0";
				cnt = await connection.QuerySingleAsync<int>(sql3, new { id = info.Id, subjectCode = info.SubjCode });
				if (cnt != 0)
				{
					string sql4 = @"UPDATE Teachers_Enrollment SET active_bit = 1, update_on = @currentDateTime WHERE empId = @id AND subjCode = @subjCode AND active_bit = 0";
					await connection.ExecuteAsync(sql4, new { currentDateTime = DateTime.UtcNow, id = info.Id, subjCode = info.SubjCode });
				}
				else
				{
					string sql4 = @"INSERT INTO Teachers_Enrollment(empId, subjCode, update_on) VALUES(@id, @subjCode, @currentDateTime)";
					await connection.ExecuteAsync(sql4, new { id = info.Id, subjCode = info.SubjCode, currentDateTime = DateTime.UtcNow });
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect($"/TeachersEnrollment/Dashboard?id={info.Id}");
		}
	}
}
