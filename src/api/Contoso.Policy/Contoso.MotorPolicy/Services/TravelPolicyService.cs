using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Contoso.MotorPolicy.Models;
using Dapper;

namespace Contoso.MotorPolicy.Services
{

    public interface ITravelPolicyService
    {
        Task<IEnumerable<TravelPolicy>> GetAll();
        Task<TravelPolicy> GetById(int id);
        Task<int> Create(TravelPolicy itm);
        Task<TravelPolicy> Update(TravelPolicy itm);
    }

    public class TravelPolicyService : ITravelPolicyService
    {

        private readonly IDbConnection _dbConnection;

        public TravelPolicyService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

        }

        public async Task<IEnumerable<TravelPolicy>> GetAll()
        {
            const string sql = @"SELECT * FROM TravelPolicy";
            return await _dbConnection.QueryAsync<TravelPolicy> (sql);
        }


        public async Task<TravelPolicy> GetById(int id)
        {
            string query = "select * from TravelPolicy where id = @id";
            var itms = await _dbConnection.QueryAsync<TravelPolicy>(query, new { id = id });
            return itms.SingleOrDefault();
        }


        public async Task<int> Create(TravelPolicy itm)
        {
            string query = @"Insert into TravelPolicy values (@CustomerId , @PolicyNo, @ProductName, @Status, @Amount, @CoverStarts, @CoverEnds);
            select Cast(Scope_Identity() as int)";
            var id = await _dbConnection.QueryAsync<int>(query, itm);
            return id.Single();
        }

        public async Task<TravelPolicy> Update(Models.TravelPolicy itm)
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

