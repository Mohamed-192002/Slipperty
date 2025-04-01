using System.Security.Claims;
using Infrastructure.Migrations.Models;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Business.Managers
{
    public class OrderContextService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderContextService> _logger;
        public readonly Dictionary<ActionType, Delegate> ActionMethods = new();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public OrderContextService(IUnitOfWork unitOfWork, ILogger<OrderContextService> logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            ActionMethods = new Dictionary<ActionType, Delegate>
            {
                { ActionType.Shipped, new Action<object>(ShipOrder) },
                { ActionType.Delivered, new Action<object>(DeliverOrder) },
                { ActionType.Holding, new Action<object>(HoldOrder) },
                { ActionType.Cancel, new Action<object>(CancelOrder) },
                { ActionType.FollowUps, new Action<object>(AddFollowUp) },
                { ActionType.Modified, new Action<object>(ModifyOrder) },
                { ActionType.Confirmed, new Action<object>(ConfirmOrder) },
                { ActionType.Prepared, new Action<object>(PrepareOrder) },
                { ActionType.Urgent, new Action<object>(UrgentOrder) },
                {ActionType.StoppedConfirmed , new Action<object>(StoppedConfirmed)},
                {ActionType.ModificationDeclined , new Action<object>(ModificationDeclined) }
            };
        }

        public void ExecuteAction<T>(ActionType actionType, T values)
        {

            if (ActionMethods.TryGetValue(actionType, out var actionMethod))
            {

                var stronglyTypedAction = actionMethod as Action<T>;
                if (stronglyTypedAction != null)
                {
                    stronglyTypedAction.Invoke(values); 
                }
                else
                {
                     _logger.LogError($"Action method for {actionType} is not compatible with the provided data type.");
                }
            }
            else
            {
                _logger.LogError($"No method found for action type {actionType}");
            }
        }

        public void ModificationDeclined(object orderTransactionVm)
        {
            var orderTransactionObject = orderTransactionVm as OrderTransactionVm<object>;
            var order = _unitOfWork.OrderHead.GetAll(_ => _.Id == orderTransactionObject.OrderId).FirstOrDefault();
            _unitOfWork.OrderModificationDeclinedRepository.Insert(new OrderModificationDeclined()
            {
                CreatedBy = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedOn = DateTime.Now,
                Reason = "رفض العميل التعديل على المنتج",
                OrderId = orderTransactionObject.OrderId
            });
            order.StatusId = (int)ActionType.Confirmed;
        }

        public void ShipOrder( object  orderTransactionVm)
        {  
            var res = orderTransactionVm as OrderTransactionVm<object>;
            if(res  != null)
            {
                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null)
                    order.StatusId = (int)res.ActionType;
                else 
                    _logger.LogInformation($"While Shipping order not found");


            }
            else
                _logger.LogInformation($"Shipping order with OrderId: {res?.OrderId}");
          
           

        }
        public void DeliverOrder(object  orderTransactionVm)
        {
            var res = orderTransactionVm as OrderTransactionVm<object>;
            if (res != null)
            {
                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null)
                    order.StatusId = (int)res.ActionType;
                else
                    _logger.LogInformation($"While DeliverOrder order not found");


            }
            else
                _logger.LogInformation($"DeliverOrder order with OrderId: {res?.OrderId}");


        }
        public void HoldOrder(object  orderTransactionVm)
        {
            var res = orderTransactionVm as OrderTransactionVm<object>;
            if (res != null)
            {
                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null)
                {
                    order.StatusId = (int)res.ActionType;
                    var settings = new JsonSerializerSettings
                    {
                        DateFormatString = "dd/MM/yyyy"  
                    };
                    var orderHolding= JsonConvert.DeserializeObject<HoldingDetails>(res.Data.ToString(),settings);
                    if (orderHolding != null)
                    {
                        OrderHoldingDetails recored = new();
                        recored.OrderID = res.OrderId;
                        recored.CreatedOn = DateTime.Now;
                        recored.HoldingDate = orderHolding.PostponementDate;
                        recored.AdditionalPrice = orderHolding.ShipmentPrice;
                        recored.CreatedBy = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                        _unitOfWork.OrderHoldingDetailsRepository.Insert(recored);
                    }
                    else
                        _logger.LogInformation("While Holding order not found");

                }
                else
                    _logger.LogInformation($"While HoldOrder order not found");


            }
            else
                _logger.LogInformation($"HoldOrder order with OrderId: {res?.OrderId}");
        }
        public void UrgentOrder(object orderTransactionVm)
        {
            var res = orderTransactionVm as OrderTransactionVm<object>;
            if (res != null)
            {
                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null)
                {
                    order.StatusId = (int)ActionType.Confirmed;
                    var settings = new JsonSerializerSettings
                    {
                        DateFormatString = "dd/MM/yyyy"  
                    };
                    var orderUrgent = JsonConvert.DeserializeObject<UrgentDetails>(res.Data.ToString(), settings);
                    if (orderUrgent != null)
                    {
                        OrderUrgentDetails recored = new();
                        recored.OrderID = res.OrderId;
                        recored.CreatedOn = DateTime.Now;
                        recored.UrgentDate = orderUrgent.UrgentDate;
                        recored.AdditionalPrice = orderUrgent.UrgentAdditionalPrice;
                        recored.CreatedBy = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                        _unitOfWork.OrderUrgentDetailsRepository.Insert(recored);
                    }
                    else
                        _logger.LogInformation("While Urgent order not found");

                }
                else
                    _logger.LogInformation($"While HoldOrder order not found");


            }
            else
                _logger.LogInformation($"HoldOrder order with OrderId: {res?.OrderId}");
        }
        public void CancelOrder(object  orderTransactionVm)
        {
            var res = orderTransactionVm as OrderTransactionVm<object>;
            if (res != null)
            {
                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null) {
                    order.StatusId = (int)res.ActionType;
                    var orderCancelationDetails = JsonConvert.DeserializeObject<CancelationDetails>(res.Data.ToString());
                    if (orderCancelationDetails != null)
                    {
                        OrderCancelationDetails recored = new ();
                        recored.Notes = orderCancelationDetails.Notes;
                        recored.OrderID = res.OrderId;
                        recored.RejectionReasoneId = orderCancelationDetails.Id; 
                        recored.CreatedOn = DateTime.Now;
                        recored.CreatedBy =
                            _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                        _unitOfWork.OrderCancelationDetails.Insert(recored);

                    }
                    else
                        _logger.LogInformation($"While AddFollowUp order not found");


                }
                else
                    _logger.LogInformation($"While CancelOrder order not found");


            }
            else
                _logger.LogInformation($"CancelOrder order with OrderId: {res?.OrderId}");
        }
        public void AddFollowUp(object  followUpDetails)
        {
            var res = followUpDetails as OrderTransactionVm<object>;
            if (res != null)
            {

                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null)
                {
                    order.StatusId = (int)res.ActionType;
                    var orderFollowUp =  JsonConvert.DeserializeObject<FollowUpDetails>(res.Data.ToString());
                    if (orderFollowUp != null)
                    {
                        OrderFollowUps recored = new OrderFollowUps();
                        recored.Notes = orderFollowUp.FollowUpNote??orderFollowUp.Notes??"";
                        recored.OrderID = res.OrderId;
                        recored.ContactStatusId = orderFollowUp?.ContactStatusId;
                        recored.ContactMethodId = orderFollowUp?.ContactMethodId;
                        recored.CreatedOn = DateTime.Now;
                        recored.CreatedBy =
                            _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                        _unitOfWork.OrderFollowUps.Insert(recored);

                    }
                    else
                        _logger.LogInformation($"While AddFollowUp order not found");

                }
                else
                    _logger.LogInformation($"While Shipping order not found");


            }
            else
                _logger.LogInformation($"Shipping order with OrderId: {res?.OrderId}");

        }
        public int ModifyOrderDetailProductCombination(int orderDetailId, OrderProductDto modifiedDetails, ref string Notes)  
        {
            var OrderDetail = _unitOfWork.OrderDetails.GetAll(_ => _.Id == orderDetailId)
                .FirstOrDefault();
            OrderDetail.Qty = modifiedDetails.Quantity;
            OrderDetail.CombinationId = modifiedDetails.CombinationId;
            Notes =  "تم تعديل الطلب";
            return OrderDetail.OrderHeadId;
            // variableCombination.Text = orderCombinationData.Text;
        }
        
        public void ModifyOrderDetailChangeProduct(object modifiedDetails, ref string Notes)  
        {
            var orderCombinationData = JsonConvert.DeserializeObject<OrderCombinationDto>(modifiedDetails.ToString());
          
            var OrderDetail = _unitOfWork.OrderDetails.GetAll(_ => _.Id == orderCombinationData.OrderDetailId)
                .FirstOrDefault();
            var order = _unitOfWork.OrderHead.GetAll(_ => _.Id == OrderDetail.OrderHeadId).FirstOrDefault();
            OrderDetail.Qty = orderCombinationData.Qty; 
            var variableCombination =  _unitOfWork.VariableCombinations.GetAll(_ => _.Id == OrderDetail.CombinationId).FirstOrDefault();
            Notes =  string.Copy(variableCombination.Text);
            variableCombination.Text = orderCombinationData.Text;
            variableCombination.ProductId = orderCombinationData.ProductId;
            OrderDetail.ProductId = orderCombinationData.ProductId;
            OrderDetail.Qty = orderCombinationData.Qty;
             
        }

        public void ReCalculateNetAmount(int orderId)
        {
            var orderHead = _unitOfWork.OrderHead.GetAll(_ => _.Id == orderId).Include(_=> _.OrderDetails).ThenInclude(_ => _.Product).FirstOrDefault();
            orderHead.NetAmount = 0; 
            // foreach (var detail in orderHead.OrderDetails)
            // {
            //     orderHead.NetAmount += (detail.Product.Price * detail.Qty);
            // }
            
            var total = orderHead?.OrderDetails?.Sum(c => c.Product?.Price);
            var discount =
                // Total regular price of all products
                orderHead?.OrderDetails?.Sum(c => c.Product?.Price ?? 0)

                // Subtracting the discount prices for products without an offer
                - orderHead?.OrderDetails?.Where(c => c.offerId == null)?.Sum(c => c.Product?.DiscountPrice ?? 0)

                // Subtracting the total offer prices, considering each offer once per RegDate
                - orderHead?.OrderDetails?.Where(c => c.offerId != null)
                    .GroupBy(c => new { c.offerId, c?.RegDate })  // Group by both offerId and RegDate
                    .Sum(g => g.Select(c => c.ProductCountsOffer?.Price ?? 0)        // Get the price for each grouped offer
                        .FirstOrDefault());                                      // Sum the offer price only once per offerId and RegDate

            orderHead.NetAmount = ((decimal)total == (decimal)discount ? total :  (total - discount))??0;
        }
        
        
        
        public int DeleteOrderDetailProduct(int orderDetailId, ref string Notes)
        {
                var orderDetails = _unitOfWork.OrderDetails.GetAll().Where(_ => _.Id == orderDetailId).FirstOrDefault();
                _unitOfWork.OrderDetails.Remove(orderDetails);
                return orderDetails.OrderHeadId;
        }

        public void ModifyOrder(object modifiedDetails)
        {
            var res = modifiedDetails as OrderTransactionVm<object>;
            var orderDetailModifiedDto =
                JsonConvert.DeserializeObject<ModifyOrderDetailDataDto>(res.Data.ToString());
            if (orderDetailModifiedDto is null) throw new Exception("Invalid Data"); 
            var order = _unitOfWork.OrderHead.GetAll(_ => _.Id == res.OrderId).FirstOrDefault();
            _mapper.Map(orderDetailModifiedDto, order);

            // if (!string.IsNullOrEmpty(orderDetailModifiedDto.Address) )
            // {
            //     order.Address = orderDetailModifiedDto.Address; 
            // }
            //
            // if (orderDetailModifiedDto.PhoneNumber is not null && order.phoneNumber == null)
            // {
            //     order.phoneNumber = orderDetailModifiedDto.PhoneNumber;
            // }else if (orderDetailModifiedDto.SecondPhoneNumber is not null && order.phoneNumber2 == null)
            // {
            //     order.phoneNumber2 = orderDetailModifiedDto.SecondPhoneNumber;
            // }

        }
        public void ConfirmOrder(object  orderTransactionVm )
        {
            var res = orderTransactionVm as OrderTransactionVm<object>;
            if (res != null)
            {
                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null)
                    order.StatusId = (int)res.ActionType;
                else
                    _logger.LogInformation($"While ConfirmOrder order not found");


            }
            else
                _logger.LogInformation($"ConfirmOrder order with OrderId: {res?.OrderId}");
        }
        public void PrepareOrder(object  orderTransactionVm)
        {
            var res = orderTransactionVm as OrderTransactionVm<object>;
            if (res != null)
            {
                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null)
                    order.StatusId = (int)res.ActionType;
                else
                    _logger.LogInformation($"While PrepareOrder order not found");


            }
            else
                _logger.LogInformation($"PrepareOrder order with OrderId: {res?.OrderId}");
        }

        public void StoppedConfirmed(object orderTransactionVm)
        {
            var res = orderTransactionVm as OrderTransactionVm<object>;
            if (res != null)
            {
                var order = _unitOfWork.OrderHead.GetAll().Where(_ => _.Id == res.OrderId).FirstOrDefault();
                if (order != null) {
                    order.StatusId = (int)res.ActionType;
                }
                else
                    _logger.LogInformation($"While Stopped Confirmed Order, order not found");
                
            }
            else
                _logger.LogInformation($"Stopped Confirmed Order, order with OrderId: {res?.OrderId}");
            
        }

        public void AddOrderProduct(int OrderId, OrderProductDto orderProduct)
        {
            var userId = _unitOfWork.OrderHead.GetAll(_ => _.Id == OrderId).FirstOrDefault().UserId;
            if (userId is null) throw new Exception("Invalid Order User Data");
            var orderDetail = new OrderDetails()
            {
                OrderHeadId = OrderId,
                ProductId = orderProduct.ProductId,
                CombinationId = orderProduct.CombinationId,
                Qty = orderProduct.Quantity,
                RegDate = DateTime.Now,
                UserId = userId
            }; 
            _unitOfWork.OrderDetails.Insert(orderDetail);
        }
        
        public int ChangeOrderProduct(int OrderDetailId , OrderProductDto orderProduct)
        {
               var orderDetail = _unitOfWork.OrderDetails.GetAll(_ => _.Id == OrderDetailId).FirstOrDefault();
               orderDetail.ProductId = orderProduct.ProductId;
               orderDetail.Qty = orderProduct.Quantity;
               orderDetail.CombinationId = orderProduct.CombinationId;
               return orderDetail.OrderHeadId;
        }
       
    }


    public class OrderProductDto
    {
        public int ProductId { get; set; } 
        public int Quantity { get; set; } 
        public int CombinationId { get; set; }  
    }
    public class OrderCombinationDto
    {
        public string Text { get; set; } 
        public int Qty { get; set; } 
        public int Id { get; set; } 
        public int ProductId { get; set; }
        public int OrderDetailId { get; set; }
        public int CombinationId { get; set; } 
    }


    public class OrderDetailModifiedDto
    {
        public string? OrderUserPhone { get; set; }
        public string? OrderUserAddress { get; set; }
    }

}
