using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.TeachersEnrollment
{
    public class IndexModel : PageModel
    {
		private readonly IDatabaseConnection databaseConnection;
		public int empNo;
		public Guid Id;
		public string errorMsg = "";

		public IndexModel(IDatabaseConnection db)
		{
			databaseConnection = db;
		}

		public async Task OnPost()
		{
			if (!(int.TryParse(Request.Form["emp_no"], out empNo)))
			{
				errorMsg = "Invalid Employee No.";
				return;
			}

			try
			{
				using SqlConnection connection = databaseConnection.GetConnection();
				string sql1 = @"SELECT COUNT(*) FROM (SELECT ROW_NUMBER() OVER (ORDER BY CAST(Teachers.name AS varchar)) AS empNo, Teachers.id, Teachers.name, Teachers.branchCode FROM Teachers JOIN Branches ON branchCode = code WHERE Teachers.active_bit = 1 AND Branches.active_bit = 1) AS TEMP WHERE empNo = @empNo";
				string sql2 = @"SELECT id FROM (SELECT ROW_NUMBER() OVER (ORDER BY CAST(Teachers.name AS varchar)) AS empNo, Teachers.id, Teachers.name, Teachers.branchCode FROM Teachers JOIN Branches ON branchCode = code WHERE Teachers.active_bit = 1 AND Branches.active_bit = 1) AS TEMP WHERE empNo = @empNo";
				int cnt = await connection.QuerySingleAsync<int>(sql1, new { empNo });
				if (cnt < 1)
				{
					errorMsg = "Invalid Employee No.";
					return;
				}
				Id = await connection.QuerySingleAsync<Guid>(sql2, new { empNo });
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}
			Response.Redirect($"/TeachersEnrollment/Dashboard?id={Id}");
		}
	}
}
