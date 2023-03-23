using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;
using System.Globalization;

namespace SKSchool.Pages.StudentsEnrollment
{
	public class DashboardModel : PageModel
	{
		private readonly IDatabaseConnection databaseConnection;
		public StudentsInfoJoinBranches info = new();
		public List<StudentsEnrollmentInfoJoinSubjectsTeachers> enrollmentList = new();
		public List<SubjectsInfo> subjectsList = new();
		public Guid rollNo;
		public int regNo;
		public string errorMsg = "";

		public DashboardModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnGet()
		{
			if(!(Guid.TryParse(Request.Query["roll_no"], out rollNo)))
			{
				errorMsg = "Invalid Roll No.";
				return;
			}
			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Students WHERE rollNo = @rollNo AND active_bit = 1";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { rollNo });
				if(cnt < 0)
				{
					errorMsg = "Invalid Roll No.";
					return;
				}
				string sql2 = @"SELECT rollNo, Students.name AS name, branchCode, Students.update_on AS updatedOn, Branches.name AS branchName FROM Students JOIN Branches ON Students.branchCode = Branches.code WHERE Students.rollNo = @rollNo AND Students.active_bit = 1 AND Branches.active_bit = 1";
				info = await connection.QuerySingleAsync<StudentsInfoJoinBranches>(sql2, new { rollNo });
				string sql3 = @"SELECT Students_Enrollment.rollNo AS rollNo, Students_Enrollment.subjCode AS subjCode,
							   Students_Enrollment.update_on AS updatedOn,Subjects.name AS subjectName, 
							   Teachers.name AS teacherName FROM Subjects JOIN Students_Enrollment
							   ON Subjects.code = Students_Enrollment.subjCode JOIN Teachers_Enrollment ON 
							   Students_Enrollment.subjCode = Teachers_Enrollment.subjCode JOIN Teachers ON Teachers_Enrollment.empId = Teachers.id WHERE 
							   rollNo = @rollNo AND Students_Enrollment.active_bit = 1 AND 
							   Teachers.active_bit = 1 AND Subjects.active_bit = 1 AND Teachers_Enrollment.active_bit = 1";
				enrollmentList = new(await connection.QueryAsync<StudentsEnrollmentInfoJoinSubjectsTeachers>(sql3, new {rollNo}));
				string sql4 = @"SELECT code, name FROM Subjects JOIN Subjects_Enrollment ON Subjects.code = Subjects_Enrollment.subjCode WHERE Subjects_Enrollment.branchCode = @branchCode AND Subjects.active_bit = 1 AND Subjects_Enrollment.active_bit = 1";
				subjectsList = new(await connection.QueryAsync<SubjectsInfo>(sql4, new {branchCode = info.BranchCode}));
				string sql5 = @"SELECT regNo FROM (SELECT ROW_NUMBER() OVER (ORDER BY CAST(Students.name AS varchar)) AS regNo, rollNo, Students.name, Students.branchCode FROM Students JOIN Branches ON branchCode = code WHERE Students.active_bit = 1 AND Branches.active_bit = 1) AS TEMP WHERE rollNo = @rollNo";
				regNo = await connection.QuerySingleAsync<int>(sql5, new { rollNo });
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
				rollNo = new(Request.Form["roll_no"]);
				Guid subjectCode = new(Request.Form["subj_code"]);
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"UPDATE Students_Enrollment SET active_bit = 0, update_on = @currentDateTime WHERE subjCode = @subjectCode AND rollNo = @rollNo";
				await connection.ExecuteAsync(sql, new { currentDateTime = DateTime.UtcNow, subjectCode, rollNo });
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.Message);
			}
			Response.Redirect($"/StudentsEnrollment/Dashboard?roll_no={rollNo}");
		}

		public async Task OnPostEditSubjectEnrollment()
		{
			rollNo = new(Request.Form["roll_no"]);
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
				string sql1 = @"SELECT COUNT(*) FROM Students_Enrollment WHERE rollNo = @rollNo AND subjCode = @newSubjCode AND active_bit = 1";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { rollNo, newSubjCode });
				if(cnt != 0)
				{
					errorMsg = "This Enrollent already exist!";
					return;
				}
				string sql2 = @"SELECT COUNT(*) FROM Students_Enrollment WHERE rollNo = @rollNo AND subjCode = @newSubjCode AND active_bit = 0";
				cnt = await connection.QuerySingleAsync<int>(sql2, new { rollNo, newSubjCode });
				if(cnt != 0)
				{
					string sql3 = @"UPDATE Students_Enrollment SET active_bit = 1, update_on = @currentDateTime WHERE rollNo = @rollNo AND subjCode = @newSubjCode";
					await connection.ExecuteAsync(sql3, new { currentDateTime = DateTime.UtcNow, rollNo, newSubjCode });
					string sql4 = @"UPDATE Students_Enrollment SET active_bit = 0, update_on = @currentDateTime WHERE rollNo = @rollNo AND subjCode = @oldSubjCode";
					await connection.ExecuteAsync(sql4, new { currentDateTime = DateTime.UtcNow, rollNo, oldSubjCode });
				}
				else
				{
					string sql3 = @"UPDATE Students_Enrollment SET subjCode = @newSubjCode, update_on = @currentDateTime WHERE rollNo = @rollNo AND subjCode = @oldSubjCode";
					await connection.ExecuteAsync(sql3, new { newSubjCode, currentDateTime = DateTime.UtcNow, rollNo, oldSubjCode });
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect($"/StudentsEnrollment/Dashboard?roll_no={rollNo}");
		}
	}
}
