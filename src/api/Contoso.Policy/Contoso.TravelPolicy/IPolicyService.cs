using Contoso.TravelPolicy.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Contoso.TravelPolicy
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPolicyService
    {

        [OperationContract]
        Task<IEnumerable<Policy>> GetAll();

        [OperationContract]
        Task<Policy> GetById(int id);

        [OperationContract]
        Task<int> Create(Policy itm);

        [OperationContract]
        Task<Policy> Update(Policy itm);
    }
}
