using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Contoso.MotorPolicy.Models;
using Dapper;

namespace Contoso.MotorPolicy.Services
{

    public interface IMotorPolicyService
    {
        Task<IEnumerable<Policy>> GetAll();
        Task<Policy> GetById(int id);
        Task<int> Create(Policy itm);
        Task<Policy> Update(Policy itm);
    }

    public class MotorPolicyService : IMotorPolicyService
    {
        private readonly IDbConnection _dbConnection;

        public MotorPolicyService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Policy>> GetAll()
        {
            const string sql = @"SELECT * FROM MotorPolicy";
            return await _dbConnection.QueryAsync<Policy> (sql);
        }


        public async Task<Models.Policy> GetById(int id)
        {
            string query = "select * from MotorPolicy where id = @id";
            var itms = await _dbConnection.QueryAsync<Policy>(query, new { id = id });
            return itms.SingleOrDefault();
        }


        public async Task<int> Create(Policy itm)
        {
            string query = @"Insert into MotorPolicy values (@CustomerId , @PolicyNumber, @ProductName, @Status, @Premium, @EffectiveFrom, @RenewalDate);
            select Cast(Scope_Identity() as int)";
            var id = await _dbConnection.QueryAsync<int>(query, itm);
            return id.Single();
        }

        public async Task<Policy> Update(Policy itm)
        {

            string query = @"UPDATE MotorPolicy SET CustomerId = @CustomerId, PolicyNumber = @PolicyNumber, ProductName = @ProductName, @Status = @Status, Premium = @Premium, EffectiveFrom = @EffectiveFrom, RenewalDate = @RenewalDate WHERE Id = @Id";

            await _dbConnection.ExecuteAsync(query, new
            {
                itm.Id,
                itm.CustomerId,
                itm.PolicyNumber,
                itm.ProductName,
                itm.Status,
                itm.Premium,
                itm.EffectiveFrom,
                itm.RenewalDate
            });

            return await GetById(itm.Id);
        }
       

    }
}

