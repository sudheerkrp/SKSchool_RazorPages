using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.StudentsEnrollment
{
	public class IndexModel : PageModel
	{
		private readonly IDatabaseConnection databaseConnection;
		public int regNo;
		public Guid rollNo;
		public string errorMsg = "";

		public IndexModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnPost()
		{
			if(!(int.TryParse(Request.Form["reg_no"], out regNo)))
			{
				errorMsg = "Invalid Roll No.";
				return;
			}

			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM (SELECT ROW_NUMBER() OVER (ORDER BY CAST(Students.name AS varchar)) AS regNo, rollNo, Students.name, Students.branchCode FROM Students JOIN Branches ON branchCode = code WHERE Students.active_bit = 1 AND Branches.active_bit = 1) AS TEMP WHERE regNo = @regNo";
				string sql2 = @"SELECT rollNo FROM (SELECT ROW_NUMBER() OVER (ORDER BY CAST(Students.name AS varchar)) AS regNo, rollNo, Students.name, Students.branchCode FROM Students JOIN Branches ON branchCode = code WHERE Students.active_bit = 1 AND Branches.active_bit = 1) AS TEMP WHERE regNo = @regNo";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { regNo });
				if(cnt < 1)
				{
					errorMsg = "Invalid Roll No.";
					return;
				}
				rollNo = await connection.QuerySingleAsync<Guid>(sql2, new { regNo });
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