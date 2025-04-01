using System.Text.Json;
using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using Business.Shard;
using Microsoft.EntityFrameworkCore;

namespace Slipperty.Areas.Admin.Controllers;

public partial class OrdersController
{
	
	// private readonly IGovernmentsBusiness _governmentsBusiness;
	// private readonly IRegionsBusiness _regionsBusiness;
        [HttpPost("OrderAction/{orderId}/{actionType}")]
        public IActionResult HandleOrderAction(int orderId, [FromBody] object data, ActionType actionType)
        {
             return _orderTransactionService.AddTransaction(new OrderTransactionVm<object>(){ OrderId = orderId, Data = data, ActionType = actionType},  actionType); 
        }

        [HttpPost("ModifiyOrderDetailProduct/{orderDetailId}")]
        public IActionResult ModifyOrderDetailProduct(int orderDetailId, [FromBody] OrderProductDto data)
        {
	        string Notes = ""; 
	        var orderId =  _orderContextService.ModifyOrderDetailProductCombination(orderDetailId,data, ref Notes);
	        return _orderTransactionService.AddTransaction(ActionType.Modified, orderId, Notes); 
        }
        
        
        [HttpPost("ModifiyOrderDetailChangeProduct/{orderId}")]
        public IActionResult ModifyOrderDetailChangeProduct(int orderId, [FromBody] object data)
        {
	        string Notes = ""; 
	         _orderContextService.ModifyOrderDetailChangeProduct(data, ref Notes);
	         _orderTransactionService.AddTransaction(ActionType.Modified, orderId, Notes);
	         _orderContextService.ReCalculateNetAmount(orderId);
	         _unitOfWork.Complete();
	         return Ok();
        }
        
        
        [HttpDelete("DeleteOrderDetailProduct/{OrderDetails}")]
        public IActionResult DeleteOrderDetailProduct(int OrderDetails)
        {
	        string Notes = ""; 
	        var orderId = _orderContextService.DeleteOrderDetailProduct(OrderDetails, ref Notes);
	        
	        _orderTransactionService.AddTransaction(ActionType.DeleteOrderDetail, orderId, Notes);
	        _orderContextService.ReCalculateNetAmount(orderId);
	        _unitOfWork.Complete();
	        return Ok();

        }


        [HttpGet("GetOrderSummary")]
        public IActionResult GetOrderSummary()
        {
	        return Ok(_orderTransactionService.GetOrderSummary());
        }

		[HttpPost("GetAllProduct")]
        public IActionResult GetAllProduct([FromBody] Page page)
        {
	        var productQuery = _productsBusiness.GetAll().Where(_ => _.StockCount > 0).Select(_ => new ProductVm()
	        {
		        Id = _.Id ,
		        MainImageUrl = ImagesPathes.Products + _.MainImageUrl,
		        Price = _.Price,
		        DiscountPrice = _.DiscountPrice,
		        ArbName = _.ArbName,
		        StockCount = _.StockCount,
                    
	        }).OrderBy(_ => _.Id).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize);
	        return Ok(productQuery);
        }

  
        [HttpGet("GetProductById/{productId}")]
        public IActionResult GetProductById(int productId)
        {
	        var productQuery = _productsBusiness.GetAll().Where(_ =>  _.Id == productId&& _.StockCount > 0).Select(_ => new 
	        {
		        Id = _.Id ,
		        MainImageUrl = ImagesPathes.Products + _.MainImageUrl,
		        Price = _.Price,
		        DiscountPrice = _.DiscountPrice,
		        ArbName = _.ArbName,
		        StockCount = _.StockCount,
		        ProductVariable = _.ProductVariables,
		        _.VariableCombinations
                    
	        }).FirstOrDefault();
	        return Ok(productQuery);
        }

        [HttpGet("GetOrderById/{OrderId}")]
        public IActionResult GetOrderById(int? OrderId)
        {
	        if(OrderId == null) return BadRequest();
	        
	        var orderDto =  
		        _ordersBusiness.GetAll() 
		        .Where(_=> _.Id == (int)OrderId)
		        .Select(_ => new
		        {
			        _.Address,
			        _.phoneNumber,
			         phoneNumber2 = _.phoneNumber2,
			        _.fullName,
			        _.Notes,
			        _.UserId,
			        _.regionId,
			        _.governmentId,
			        _.deliveryTimeTo,
			        _.deliveryTimeFrom
			        
		        }).FirstOrDefault();
	        
	        var userAddresses = _userAddressesBusiness.GetAll().Where(_ => _.UserId == orderDto.UserId).ToList();
	        
	        return Ok(new
	        {
		        OrderData = orderDto,
		        UserAddresses = userAddresses,
	        }); 
        }

		
        [HttpGet("GetUserOrders/{OrderId}")]
        public IActionResult GetUserOrders(int OrderId)
        {
	        var orderInfo = _ordersBusiness.GetAll().Where(_ => _.Id == OrderId).FirstOrDefault();
	        var userOrders = _ordersBusiness.GetAll().Where(_ => _.phoneNumber == orderInfo.phoneNumber || _.phoneNumber2 == orderInfo.phoneNumber2 || _.phoneNumber == orderInfo.phoneNumber2 || _.phoneNumber2 == orderInfo.phoneNumber).ToList();
	        return Ok(userOrders); 
        }

        [HttpPost("AddOrderProduct/{OrderId}")]
        public IActionResult AddOrderProduct(int OrderId , [FromBody] OrderProductDto orderProductDto)
        {
	        _orderContextService.AddOrderProduct(OrderId , orderProductDto);
	        _orderTransactionService.AddTransaction(ActionType.ModifiedProduct , OrderId,$"{orderProductDto.ProductId}اضافة منتج جديد");
	        _orderContextService.ReCalculateNetAmount(OrderId);
	        _unitOfWork.Complete();

	        return Ok(); 
        }
        
        [HttpPost("ChangeOrderProduct/{OrderDetailId}")]
        public IActionResult ChangeOrderProduct(int OrderDetailId , [FromBody] OrderProductDto orderProductDto)
        {
	        var orderId = _orderContextService.ChangeOrderProduct(OrderDetailId, orderProductDto);
	        _orderTransactionService.AddTransaction(ActionType.ModifiedProduct ,orderId,$"{orderProductDto.ProductId}تغير المنتج الى");
	        _orderContextService.ReCalculateNetAmount(orderId);
	        _unitOfWork.Complete();
	        return Ok(); 
        }

        [HttpGet("GetProductId/{OrderDetailId}")]
        public IActionResult GetProductId(int OrderDetailId)
        {
	        var productId = _ordersBusiness.GetOrderDetails(OrderDetailId).FirstOrDefault()?.ProductId??-1;
	        return Ok(productId);
        }
        
        // [HttpPost("ModifyOrderDetailData")]
        // public IActionResult ModifyOrderDetailData(ModifyOrderDetailDataDto modifyOrderDetailDataDto)
        // {
	       //  
	       //  return Ok();
        // }

}





