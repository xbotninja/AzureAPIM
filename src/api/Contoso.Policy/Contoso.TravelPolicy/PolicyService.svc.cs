using Contoso.TravelPolicy.Models;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.TravelPolicy
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class PolicyService : IPolicyService
    {
        private readonly IDbConnection _dbConnection;

        public PolicyService()
        {
            _dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public async Task<IEnumerable<Policy>> GetAll()
        {
            const string sql = @"SELECT * FROM TravelPolicy";
            return await _dbConnection.QueryAsync<Policy>(sql);
        }


        public async Task<Policy> GetById(int id)
        {
            string query = "select * from TravelPolicy where id = @id";
            var itms = await _dbConnection.QueryAsync<Policy>(query, new { id = id });
            return itms.SingleOrDefault();
        }


        public async Task<int> Create(Policy itm)
        {
            string query = @"Insert into TravelPolicy values (@CustomerId , @PolicyNo, @ProductName, @Status, @Amount, @CoverStarts, @CoverEnds);
            select Cast(Scope_Identity() as int)";
            var id = await _dbConnection.QueryAsync<int>(query, itm);
            return id.Single();
        }

        public async Task<Policy> Update(Models.Policy itm)
        {

            string query = @"UPDATE TravelPolicy SET CustomerId = @CustomerId, PolicyNumber = @PolicyNo, ProductName = @ProductName, @Status = @Status, Amount = @Amount, CoverStarts = @CoverStarts, CoverEnds = @CoverEnds WHERE Id = @Id";

            await _dbConnection.ExecuteAsync(query, new
            {
                itm.Id,
                itm.CustomerId,
                itm.PolicyNo,
                itm.ProductName,
                itm.Status,
                itm.Amount,
                itm.CoverStarts,
                itm.CoverEnds
            });

            return await GetById(itm.Id);
        }

    }
}
