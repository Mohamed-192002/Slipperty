using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Business.Managers
{
    public class OrderTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<OrderTransactionService> _logger;
        private readonly OrderTransaction _orderTransaction;
        private readonly OrderContextService _orderContextService;
        private readonly INotifyOrderTransactionService _notifyOrderTransactionService;
        public OrderTransactionService(IUnitOfWork unitOfWork
            , IHttpContextAccessor httpContextAccessor
            , ILogger<OrderTransactionService> logger
            , OrderContextService orderContextService, IHubContext<NotifyIOrderTransaction> hubContext, INotifyOrderTransactionService notifyOrderTransactionService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _orderContextService = orderContextService;
            _notifyOrderTransactionService = notifyOrderTransactionService;
            _orderTransaction = new OrderTransaction()
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
            };
        }


        public object GetOrderSummary()
        {
            var NewOrdersCount =  _unitOfWork.OrderHead.GetAll(_ => _.StatusId == null || _.StatusId == (int)ActionType.New).Count();
            var HoldeOrdersCount =  _unitOfWork.OrderHead.GetAll(_ =>  _.StatusId == (int)ActionType.Holding).Count();
            var StoppOrdersCount = _unitOfWork.OrderHead.GetAll(_ => _.StatusId == (int)ActionType.StoppedConfirmed).Count();
            var ConfirmOrdersCount = _unitOfWork.OrderHead.GetAll(_ =>  _.StatusId == (int)ActionType.Confirmed).Count();
            var WhatsappOrdersCount = _unitOfWork.OrderHead.GetAll(_ => _.StatusId == (int)ActionType.FollowUps && _.OrderFollowUps.OrderByDescending(_ => _.FollowUpID).LastOrDefault().ContactMethodId == 2).Count();
            var RecallOrdersCount = _unitOfWork.OrderHead.GetAll(_ =>
                _.StatusId == (int)ActionType.FollowUps && _.OrderFollowUps.OrderByDescending(_ => _.FollowUpID)
                    .LastOrDefault().ContactMethodId != 2).Count();
            var TriedOrderCount = _unitOfWork.OrderHead.GetAll(_ => _.StatusId == (int)ActionType.FollowUps && !_.OrderFollowUps.OrderByDescending(_ => _.FollowUpID).Any( _ => _.ContactMethodId == 2)).Count();
            var Cancel = _unitOfWork.OrderHead.GetAll(_ => _.StatusId == (int)ActionType.Cancel).Count(); 

           
            return new OrderSummary()
            {
                NewOrdersCount =NewOrdersCount,
                HoldOrdersCount  = HoldeOrdersCount, 
                StoppedOrdersCount  = StoppOrdersCount,
                ConfirmedOrdersCount  = ConfirmOrdersCount,
                WhatsappOrdersCount = WhatsappOrdersCount,
                TriedOrderCount = TriedOrderCount,
                RecallOrdersCount = RecallOrdersCount,
                Cancel = Cancel
            };
        }


        public IActionResult AddTransaction<T>(OrderTransactionVm<T> values, ActionType actionType)
        {
            try
            {
                _orderContextService.ExecuteAction(actionType, values);
                _orderTransaction.OperationType = (int)actionType;
                _orderTransaction.OrderID = values.OrderId; //todo: add detials
                _unitOfWork.OrderTransaction.Insert(_orderTransaction);
                _unitOfWork.Complete();
                _notifyOrderTransactionService.SendOrderTransactionNotification();
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the transaction.");
                return new BadRequestResult();
            }
        }

        public IActionResult AddTransaction(ActionType ActionType , int OrderId, string Notes = "" )
        {
            _orderTransaction.OperationType = (int)ActionType;
            _orderTransaction.OrderID = OrderId;
            _orderTransaction.Details = Notes; 
            _unitOfWork.OrderTransaction.Insert(_orderTransaction);
            _unitOfWork.Complete();
            return new OkResult();
        }
    }
}
