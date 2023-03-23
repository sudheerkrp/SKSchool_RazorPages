using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SKSchool.DataClass;
using System.Data.SqlClient;

namespace SKSchool.Pages.TeachersEnrollment
{
    public class IndexModel : PageModel
    {
        private readonly IDatabaseConnection databaseConnection;
        public List<TeachersEnrollmentInfo> info = new();

        public IndexModel(IDatabaseConnection db)
        {
            databaseConnection = db;
        }

        public async Task OnGet()
        {
            using SqlConnection connection = databaseConnection.GetConnection();
            string sql = @"SELECT * FROM Teachers_Enrollment WHERE active_bit = 1";
            info = (await connection.QueryAsync<TeachersEnrollmentInfo>(sql)).ToList();
        }
    }
}
