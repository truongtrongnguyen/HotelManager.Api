using AutoMapper;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.HotelDevice;
using HotelManager.Entities.Dto.Incoming.Order;
using HotelManager.Entities.Dto.Outgoing.Generic;
using HotelManager.Entities.Message;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    public class OrdersController : BaseController
    {
        public OrdersController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager,
                                IMapper _mapper
                                )
                                : base(unitOfWork, userManager, _mapper)
        {
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAll()
        {
            var result = new Result<IEnumerable<Order>>();
            result.IsSuccess = true;
            result.Content = await _unitOfWork.Orders.GetAll();
            return Ok(result);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequestDto request)
        {
            var result = new Result<CreateOrderRequestDto>();

            if (ModelState.IsValid)
            {
                
                var isCreate = await _unitOfWork.Orders.AddOrder(request);

                if (isCreate)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = request;
                    result.IsSuccess = true;

                    return Ok(result);
                }

                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.SomethingWentWrong,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
            else
            {
                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.InvalidRequest,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
        }

        [HttpPut("UpdatePayment")]
        public async Task<IActionResult> UpdatePayment(string request)
        {
            var result = new Result<string>();

            if (ModelState.IsValid)
            {
                var isUpdate = await _unitOfWork.Orders.UpdatePayment(request);

                if (isUpdate)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = "Update Order Success";
                    result.IsSuccess = true;

                    return Ok(result);
                }
                result.Error = PopulateError(400,
                                                 ErrorMessage.Generic.SomethingWentWrong,
                                                 ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
            else
            {
                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.InvalidRequest,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
        }

        [HttpPut("UpdateDateTimeOrder")]
        public async Task<IActionResult> UpdateDateTimeOrder(UpdateOrderDate request)
        {
            var result = new Result<string>();

            if (ModelState.IsValid)
            {
                var isUpdate = await _unitOfWork.Orders.UpdateOrderDateTime(request);

                if (isUpdate)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = "Update Order Success";
                    result.IsSuccess = true;

                    return Ok(result);
                }
                result.Error = PopulateError(400,
                                                 ErrorMessage.Generic.SomethingWentWrong,
                                                 ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
            else
            {
                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.InvalidRequest,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
        }

        [HttpPut("UpdateInformation")]
        public async Task<IActionResult> UpdateInformation(UpdateOrderCustomer request)
        {
            var result = new Result<string>();

            if (ModelState.IsValid)
            {
                var isUpdate = await _unitOfWork.Orders.UpdateOrder(request);

                if (isUpdate)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = "Update Order Success";
                    result.IsSuccess = true;

                    return Ok(result);
                }
                result.Error = PopulateError(400,
                                                 ErrorMessage.Generic.SomethingWentWrong,
                                                 ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
            else
            {
                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.InvalidRequest,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
        }

    }
}
