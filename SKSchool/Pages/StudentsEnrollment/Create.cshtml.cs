using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using System.Data.SqlClient;
using SKSchool.DataClass;

namespace SKSchool.Pages.StudentsEnrollment
{
    public class CreateModel : PageModel
    {
		private readonly IDatabaseConnection databaseConnection;
		private readonly int maxEnrollmentLimit = 6;
		public List<SubjectsInfo> subjectsList = new();
		public StudentsEnrollmentInfo info = new();
		public string errorMsg = "";

		public CreateModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnGet()
		{
			try
			{
				Guid rollNo;
				if (!(Guid.TryParse(Request.Query["roll_no"], out rollNo)))
				{
					errorMsg = "Invalid Data!";
					return;
				}
				info.RollNo = rollNo;
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Students WHERE rollNo = @rollNo AND active_bit = 1";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { rollNo });
				if(cnt != 1)
				{
					errorMsg = "Invalid Data!";
					return;
				}
				string sql2 = @"SELECT branchCode FROM Students WHERE rollNo = @rollNo AND active_bit = 1";
				Guid branchCode = await connection.QuerySingleAsync<Guid>(sql2, new { rollNo });
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
			info.RollNo = new(Request.Form["roll_no"]);
			if(Request.Form["subj_code"] == "select")
			{
				errorMsg = "Subject Name is Required!";
				return;
			}
			info.SubjCode = new(Request.Form["subj_code"]);
			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Students_Enrollment WHERE rollNo = @rollNo AND active_bit = 1 AND subjCode = @subjectCode";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { rollNo = info.RollNo, subjectCode = info.SubjCode });
				if (cnt != 0)
				{
					errorMsg = "You are already enrolled in this Subject!";
					return;
				}
				string sql2 = @"SELECT COUNT(*) FROM Students_Enrollment WHERE rollNo = @rollNo AND active_bit = 1";
				cnt = await connection.QuerySingleAsync<int>(sql2, new {rollNo = info.RollNo});
				if(cnt >= maxEnrollmentLimit)
				{
					errorMsg = "Maximum Enrollment Limit Exceeded!";
					return;
				}
				string sql3 = @"SELECT COUNT(*) FROM Students_Enrollment WHERE rollNo = @rollNo AND subjCode = @subjectCode AND active_bit = 0";
				cnt = await connection.QuerySingleAsync<int>(sql3, new { rollNo = info.RollNo, subjectCode = info.SubjCode });
				if(cnt != 0)
				{
					string sql4 = @"UPDATE Students_Enrollment SET active_bit = 1, update_on = @currentDateTime WHERE rollNo = @rollNo AND subjCode = @subjCode AND active_bit = 0";
					await connection.ExecuteAsync(sql4, new { currentDateTime = DateTime.UtcNow, rollNo = info.RollNo, subjCode = info.SubjCode });
				}
				else
				{
					string sql4 = @"INSERT INTO Students_Enrollment(rollNo, subjCode, update_on) VALUES(@rollNo, @subjCode, @currentDateTime)";
					await connection.ExecuteAsync(sql4, new {rollNo = info.RollNo, subjCode = info.SubjCode, currentDateTime = DateTime.UtcNow});
				}
			}
			catch(Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect($"/StudentsEnrollment/Dashboard?roll_no={info.RollNo}");
		}
	}
}
