using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace SKSchool.Pages.StudentsEnrollment
{
	public class IndexModel : PageModel
	{
		private static bool IsGuid(string guidString)
		{
			bool IsGuid = false;
			if (!string.IsNullOrEmpty(guidString))
			{
				Regex guidRegExp = new(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
				IsGuid = guidRegExp.IsMatch(guidString);
			}
			return IsGuid;
		}
		public string errorMsg = "";
		public Guid rollNo;

		public async Task OnPost()
		{
			
			if (!IsGuid(Request.Form["roll_no"]))
			{
				errorMsg = "Please Enter Valid Roll No.";
				return;
			}
			rollNo = new(Request.Form["roll_no"]);
			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"SELECT COUNT(*) FROM Students WHERE rollNo = @_rollNo";
				int cnt = await connection.QuerySingleAsync<int>(sql, new { _rollNo = rollNo });
				if(cnt == 0)
				{
					errorMsg = "Invalid Roll No.";
					return;
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
