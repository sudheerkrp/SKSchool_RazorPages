using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.SubjectsEnrollment
{
	public class IndexModel : PageModel
	{
		private readonly IDatabaseConnection databaseConnection;
		public List<SubjectsEnrollmentInfoJoinBranchesSubjects> enrollmentList = new();
		public List<SubjectsInfo> subjectsList = new();
		public List<BranchesInfo> branchesList = new();
		public SubjectsEnrollmentInfo oldInfo = new();
		public SubjectsEnrollmentInfo newInfo = new();
		public string errorMsg = "";

		public IndexModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnGet()
		{
			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT subjCode, branchCode, Subjects_Enrollment.update_on AS
								updatedOn, Branches.name AS branchName, Subjects.name As 
								subjectName FROM Branches JOIN Subjects_Enrollment ON 
								Branches.code = branchCode JOIN Subjects ON 
								subjCode = Subjects.code WHERE Branches.active_bit = 1 AND 
								Subjects.active_bit = 1 AND Subjects_Enrollment.active_bit = 1";
				string sql2 = @"SELECT * FROM Branches WHERE active_bit = 1";
				string sql3 = @"SELECT * FROM Subjects WHERE active_bit = 1";
				enrollmentList = new(await connection.QueryAsync<SubjectsEnrollmentInfoJoinBranchesSubjects>(sql1));
				branchesList = new(await connection.QueryAsync<BranchesInfo>(sql2));
				subjectsList = new(await connection.QueryAsync<SubjectsInfo>(sql3));
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
				oldInfo.SubjCode = new(Request.Form["subj_code"]);
				oldInfo.BranchCode = new(Request.Form["branch_code"]);
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql = @"UPDATE Subjects_Enrollment SET active_bit = 0, update_on = @currentDateTime WHERE subjCode = @subjCode AND branchCode = @branchCode";
				await connection.ExecuteAsync(sql, new { currentDateTime = DateTime.UtcNow, subjCode = oldInfo.SubjCode , branchCode = oldInfo.BranchCode});
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.Message);
			}
			Response.Redirect("/SubjectsEnrollment");
		}

		public async Task OnPostEditSubjectEnrollment()
		{
			oldInfo.SubjCode = new(Request.Form["old_subj_code"]);
			oldInfo.BranchCode = new(Request.Form["old_branch_code"]);
			if (Request.Form["new_branch_code"] == "select" || Request.Form["new_subj_code"] == "select")
			{
				errorMsg = "Subject and Branch Name are required!";
				return;
			}
			newInfo.SubjCode = new(Request.Form["new_subj_code"]);
			newInfo.BranchCode = new(Request.Form["new_branch_code"]);

			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Subjects_Enrollment WHERE subjCode = @subjCode AND branchCode = @branchCode AND active_bit = 1";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { subjCode = newInfo.SubjCode, branchCode = newInfo.BranchCode });
				if (cnt != 0)
				{
					errorMsg = "This Subject & Branch Pair Already Enrolled.";
					return;
				}
				string sql2 = @"SELECT COUNT(*) FROM Subjects_Enrollment WHERE subjCode = @subjCode AND branchCode = @branchCode AND active_bit = 0";
				cnt = await connection.QuerySingleAsync<int>(sql2, new { subjCode = newInfo.SubjCode, branchCode = newInfo.BranchCode });
				if (cnt != 0)
				{

					string sql3 = @"UPDATE Subjects_Enrollment SET active_bit = 0, update_on = @currentDateTime WHERE subjCode = @oldSubjCode AND branchCode = @oldBranchCode";
					await connection.ExecuteAsync(sql3, new { currentDateTime = DateTime.UtcNow, oldSubjCode = oldInfo.SubjCode, oldBranchCode = oldInfo.BranchCode });
					string sql4 = @"UPDATE Subjects_Enrollment SET active_bit = 1, update_on = @currentDateTime WHERE subjCode = @newSubjCode AND branchCode = @newBranchCode";
					await connection.ExecuteAsync(sql4, new { currentDateTime = DateTime.UtcNow, newSubjCode = newInfo.SubjCode, newBranchCode = newInfo.BranchCode});
				}
				else
				{
					string sql3 = @"UPDATE Subjects_Enrollment SET subjCode = @newSubjCode, branchCode = @newBranchCode, update_on = @currentDateTime WHERE subjCode = @oldSubjCode AND branchCode = @oldBranchCode";
					await connection.ExecuteAsync(sql3, new { newSubjCode = newInfo.SubjCode, newBranchCode = newInfo.BranchCode, currentDateTime = DateTime.UtcNow, oldSubjCode = oldInfo.SubjCode, oldBranchCode = oldInfo.BranchCode });
				}

			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect("/SubjectsEnrollment");
		}
	}
}