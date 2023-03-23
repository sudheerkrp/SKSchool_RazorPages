using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.Pages.Branches;
using System.Data.SqlClient;
using Dapper;
using SKSchool.Pages.Subjects;
using SKSchool.DataClass;

namespace SKSchool.Pages.SubjectsEnrollment
{
	public class EditModel : PageModel
	{

		public List<SubjectsInfo> subjectsList = new();
		public List<BranchesInfo> branchesList = new();
		public SubjectsEnrollmentInfo oldInfo = new();
		public SubjectsEnrollmentInfo newInfo = new();
		public string errorMsg = "";

		public async Task OnGet()
		{
			oldInfo.BranchCode = new(Request.Query["branch_code"]);
			oldInfo.SubjCode = new(Request.Query["subj_code"]);
			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql1 = @"SELECT * FROM Branches WHERE active_bit = 1";
				string sql2 = @"SELECT * FROM Subjects WHERE active_bit = 1";
				string sql3 = @"SELECT COUNT(*) FROM Subjects_Enrollment WHERE subjCode = @subjCode AND branchCode = @branchCode";
				branchesList = (List<BranchesInfo>)await connection.QueryAsync<BranchesInfo>(sql1);
				subjectsList = (List<SubjectsInfo>)await connection.QueryAsync<SubjectsInfo>(sql2);
				int cnt = await connection.QuerySingleAsync<int>(sql3, new { subjCode = oldInfo.SubjCode, branchCode = oldInfo.BranchCode });
				Console.WriteLine("Cnt : " + cnt);
				if(cnt < 1)
				{
					errorMsg = "Wrong Information.";
					return;
				}
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
			newInfo.SubjCode = new(Request.Form["subj_code"]);
			newInfo.BranchCode = new(Request.Form["branch_code"]);

			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"UPDATE Subjects_Enrollment SET subjCode = @newSubjCode, branchCode = @newBranchCode, update_on = @currentDateTime WHERE subjCode = @oldSubjCode AND branchCode = @oldBranchCode";
				await connection.ExecuteAsync(sql, new { newSubjCode = newInfo.SubjCode, newBranchCode = newInfo.BranchCode, currentDateTime = DateTime.Now, oldSubjCode = oldInfo.SubjCode, oldBranchCode = oldInfo.BranchCode });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect("/SubjectsEnrollment/Index");
		}
	}
}
