using Contoso.MotorPolicy.Models;
using Contoso.MotorPolicy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contoso.MotorPolicy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotorPolicyController : ControllerBase
    {
       
        private readonly IMotorPolicyService _policyService;

        public MotorPolicyController(IMotorPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpGet]
        public async Task<IEnumerable<Policy>> Get()
        {
            return await _policyService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Policy> GetById(int id)
        {
            return await _policyService.GetById(id);
        }

        [HttpPut("{id}")]
        public async Task<Policy> Update(int id, Policy policy)
        {
            return await _policyService.Update(policy);
        }

        [HttpPost]
        public async Task<int> Create(Policy policy)
        {
            return await _policyService.Create(policy);
        }

    }
}
