using Dapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System;
using System.Data.SqlClient;
using System.Web;

namespace SKSchool.Pages.StudentsEnrollment
{
	public class DashboardModel : PageModel
	{
		public List<string> subjectsList = new();
		public int idx = 1;
		public Guid rollNo;
		public int getIdx()
		{
			return idx++;
		}

		//[Inject]
		//public IDatabaseConnection Dbc { get; set; }

		public async Task OnGet()
		{
			try
			{
				/*using var Connection = Dbc.GetConnection();
				string Sql = @"SELECT * FROM Branches WHERE active_bit = 1";
				BranchesList = (List<BranchesInfo>)await Connection.QueryAsync<BranchesInfo>(Sql);*/

				rollNo = new(Request.Query["roll_no"]);
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				string sql = @"SELECT name FROM Subjects WHERE code In(SELECT subjCode FROM Students_Enrollment WHERE rollNo = @_rollNo)";
				subjectsList = new(await connection.QueryAsync<string>(sql, new {_rollNo = rollNo}));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}
	}
}
