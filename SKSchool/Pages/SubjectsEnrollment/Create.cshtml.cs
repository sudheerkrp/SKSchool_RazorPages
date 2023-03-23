using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Dapper;
using SKSchool.DataClass;

namespace SKSchool.Pages.SubjectsEnrollment
{
    public class CreateModel : PageModel
    {
		private readonly IDatabaseConnection databaseConnection;
		public List<SubjectsInfo> subjectsList = new();
		public List<BranchesInfo> branchesList = new();
		public SubjectsEnrollmentInfo info = new();
		public string errorMsg = "";

		public CreateModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnGet()
		{
			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT * FROM Branches WHERE active_bit = 1";
				string sql2 = @"SELECT * FROM Subjects WHERE active_bit = 1";
				branchesList = (List<BranchesInfo>)await connection.QueryAsync<BranchesInfo>(sql1);
				subjectsList = (List<SubjectsInfo>)await connection.QueryAsync<SubjectsInfo>(sql2);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}

		public async Task OnPost()
		{
			if (Request.Form["branch_code"] == "select" || Request.Form["subj_code"] == "select")
			{
				errorMsg = "Subject and Branch Name are required!";
				return;
			}
			info.SubjCode = new(Request.Form["subj_code"]);
			info.BranchCode = new(Request.Form["branch_code"]);

			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM Subjects_Enrollment WHERE subjCode = @subjCode AND branchCode = @branchCode";
				string sql2 = @"INSERT INTO Subjects_Enrollment(subjCode, branchCode, update_on) VALUES(@subjCode, @branchCode, @currentDateTime)";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { subjCode = info.SubjCode, branchCode = info.BranchCode });
				if (cnt >= 1)
				{
					errorMsg = "This Subject & Branch Pair Already Enrolled.";
					return;
				}
				await connection.ExecuteAsync(sql2, new { subjCode = info.SubjCode, branchCode = info.BranchCode, currentDateTime = info.UpdatedOn});
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