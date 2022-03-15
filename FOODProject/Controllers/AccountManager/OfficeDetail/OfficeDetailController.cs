using FOODProject.Core.AccountManager.OfficeDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Controllers.AccountManager.OfficeDetail
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeDetailController : ControllerBase
    {
        private readonly OfficeDetails _officeDetail;

        public OfficeDetailController(OfficeDetails officeDetail)
        {
            _officeDetail = officeDetail;
        }
        [HttpPost("Add")]
        public async Task<IActionResult>AddOfficeDetail(Model.AccountManager.OfficeDetail.OfficeDetail value)
        {
            var result = _officeDetail.AddOfficeDetail(value);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllOffice()
        {
            var result = _officeDetail.GetAllOfficeDetail();
            return Ok(result);
        }
    }
}
