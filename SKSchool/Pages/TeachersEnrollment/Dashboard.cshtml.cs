using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;
using Dapper;

namespace SKSchool.Pages.TeachersEnrollment
{
    public class DashboardModel : PageModel
    {
		private readonly IDatabaseConnection databaseConnection;
		public TeachersInfoJoinBranches info = new();
		public List<TeachersEnrollmentInfoJoinSubjects> enrollmentList = new();
		public List<SubjectsInfo> subjectsList = new();
		public Guid id;
		public int empNo;
		public string errorMsg = "";

		public DashboardModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnGet()
		{
			if (!(Guid.TryParse(Request.Query["id"], out id)))
			{
				errorMsg = "Invalid Employee No.";
				return;
			}
			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Teachers WHERE id = @id AND active_bit = 1";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { id });
				if (cnt < 0)
				{
					errorMsg = "Invalid Employee No.";
					return;
				}
				string sql2 = @"SELECT Teachers.id, Teachers.name AS name, branchCode, Teachers.update_on AS updatedOn, Branches.name AS branchName FROM Teachers JOIN Branches ON Teachers.branchCode = Branches.code WHERE Teachers.id = @id AND Teachers.active_bit = 1 AND Branches.active_bit = 1";
				info = await connection.QuerySingleAsync<TeachersInfoJoinBranches>(sql2, new { id });
				string sql3 = @"SELECT Teachers_Enrollment.empId AS id, Teachers_Enrollment.subjCode AS subjCode,
							   Teachers_Enrollment.update_on AS updatedOn, Subjects.name AS subjectName 
							   FROM Teachers_Enrollment JOIN Subjects
							   ON Subjects.code = Teachers_Enrollment.subjCode WHERE 
							   Teachers_Enrollment.empId = @id AND Teachers_Enrollment.active_bit = 1 AND 
							   Subjects.active_bit = 1";
				enrollmentList = new(await connection.QueryAsync<TeachersEnrollmentInfoJoinSubjects>(sql3, new { id }));
				string sql4 = @"SELECT code, name FROM Subjects JOIN Subjects_Enrollment ON Subjects.code = Subjects_Enrollment.subjCode WHERE Subjects_Enrollment.branchCode = @branchCode AND Subjects.active_bit = 1 AND Subjects_Enrollment.active_bit = 1";
				subjectsList = new(await connection.QueryAsync<SubjectsInfo>(sql4, new { branchCode = info.BranchCode }));
				string sql5 = @"SELECT empNo FROM (SELECT ROW_NUMBER() OVER (ORDER BY CAST(Teachers.name AS varchar)) AS empNo, id, Teachers.name, Teachers.branchCode FROM Teachers JOIN Branches ON branchCode = code WHERE Teachers.active_bit = 1 AND Branches.active_bit = 1) AS TEMP WHERE id = @id";
				empNo = await connection.QuerySingleAsync<int>(sql5, new { id });
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}

		public async Task OnPostDeleteSubjectEnrollment()
		{
			try
			{
				id = new(Request.Form["id"]);
				Guid subjectCode = new(Request.Form["subj_code"]);
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"UPDATE Teachers_Enrollment SET active_bit = 0, update_on = @currentDateTime WHERE subjCode = @subjectCode AND empId = @id";
				await connection.ExecuteAsync(sql, new { currentDateTime = DateTime.UtcNow, subjectCode, id });
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.Message);
			}
			Response.Redirect($"/TeachersEnrollment/Dashboard?id={id}");
		}

		public async Task OnPostEditSubjectEnrollment()
		{
			id = new(Request.Form["id"]);
			Guid oldSubjCode = new(Request.Form["old_subj_code"]);
			if (Request.Form["new_subj_code"] == "select")
			{
				errorMsg = "Branch Name is required!";
				return;
			}
			Guid newSubjCode = new(Request.Form["new_subj_code"]);

			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Teachers_Enrollment WHERE subjCode = @newSubjCode AND active_bit = 1";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { id, newSubjCode });
				if (cnt != 0)
				{
					errorMsg = "This Enrollent already exist!";
					return;
				}
				string sql2 = @"SELECT COUNT(*) FROM Teachers_Enrollment WHERE empId = @id AND subjCode = @newSubjCode AND active_bit = 0";
				cnt = await connection.QuerySingleAsync<int>(sql2, new { id, newSubjCode });
				if (cnt != 0)
				{
					string sql3 = @"UPDATE Teachers_Enrollment SET active_bit = 0, update_on = @currentDateTime WHERE empId = @id AND subjCode = @oldSubjCode";
					await connection.ExecuteAsync(sql3, new { currentDateTime = DateTime.UtcNow, id, oldSubjCode });
					string sql4 = @"UPDATE Teachers_Enrollment SET active_bit = 1, update_on = @currentDateTime WHERE empId = @id AND subjCode = @newSubjCode";
					await connection.ExecuteAsync(sql4, new { currentDateTime = DateTime.UtcNow, id, newSubjCode });
				}
				else
				{
					string sql3 = @"UPDATE Teachers_Enrollment SET subjCode = @newSubjCode, update_on = @currentDateTime WHERE empId = @id AND subjCode = @oldSubjCode";
					await connection.ExecuteAsync(sql3, new { newSubjCode, currentDateTime = DateTime.UtcNow, id, oldSubjCode });
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect($"/TeachersEnrollment/Dashboard?id={id}");
		}
	}
}
