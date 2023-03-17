using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using SKSchool.Pages.Subjects;
using System.Data.SqlClient;

namespace SKSchool.Pages.StudentsEnrollment
{
    public class CreateModel : PageModel
    {
		public List<SubjectsInfo> subjectsList = new();
		public async Task OnGet()
		{
			try
			{
				/*using var Connection = Dbc.GetConnection();
				string Sql = @"SELECT * FROM Branches WHERE active_bit = 1";
				BranchesList = (List<BranchesInfo>)await Connection.QueryAsync<BranchesInfo>(Sql);*/

				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SK_School_DB;Integrated Security=True";
				using SqlConnection connection = new(connectionString);
				//string sql = @"SELECT * FROM Branches WHERE active_bit = 1";
				//subjectsList = new(await connection.QueryAsync<SubjectsInfo>(sql));

			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception : " + ex.ToString());
			}
		}
	}
}
