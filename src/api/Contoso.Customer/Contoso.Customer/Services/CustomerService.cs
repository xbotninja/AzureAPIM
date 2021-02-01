using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Contoso.Customers.Models;
using Dapper;

namespace Contoso.Customers.Services
{

    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetById(int id);
        Task<int> Create(Customer itm);
        Task<Customer> Update(Customer itm);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IDbConnection _dbConnection;

        public CustomerService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            const string sql = @"SELECT * FROM customers";
            return await _dbConnection.QueryAsync<Customer> (sql);
        }


        public async Task<Customer> GetById(int id)
        {
            string query = "select * from customers where id = @id";
            var itms = await _dbConnection.QueryAsync<Customer>(query, new { id = id });
            return itms.SingleOrDefault();
        }


        public async Task<int> Create(Customer itm)
        {
            string query = @"Insert into customers values (@FirstName , @LastName, @Email, @Title);
            select Cast(Scope_Identity() as int)";
            var id = await _dbConnection.QueryAsync<int>(query, itm);
            return id.Single();
        }

        public async Task<Customer> Update(Customer itm)
        {
            string query = @"UPDATE customers SET FirstName = @FirstName, LastName = @LastName, Title = @Title, @Email = @Email WHERE Id = @Id";

            await _dbConnection.ExecuteAsync(query, new
            {
                itm.Id,
                itm.FirstName,
                itm.LastName,
                itm.Title,
                itm.Email
            });

            return await GetById(itm.Id);
        }
       

    }
}


/*
 * dacpac
 * 
 * public void CreateTables()
    {
        using (var connection = GetSQLiteHandle())
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS statistics (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    name TEXT NOT NULL,
                    value TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS posts (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    content TEXT NOT NULL,
                    created TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS names (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    name TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    username TEXT NOT NULL,
                    email TEXT NOT NULL,
                    created TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS storage (
                    id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    key TEXT NOT NULL,
                    value TEXT NOT NULL
                );
            ";
            connection.Execute(sql);
        }
    }

*/