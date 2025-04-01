using System.Collections.Concurrent;
using Business.Managers;
using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Business.Helpers;

[Authorize(Roles = Roles.Role_Admin)]
public class NotifyIOrderTransaction : Hub
{
	// private static ConcurrentDictionary<int, string> LockedOrders = new();
	private readonly OrderLockService _orderLockService;

	public NotifyIOrderTransaction(OrderLockService orderLockService)
	{
		_orderLockService = orderLockService;
	}

	public async Task SendOrderTransactionNotification(int orderId)
	{
		await Clients.All.SendAsync("ReceiveTransactionNotification", orderId);
	}

	// public async Task LockOrder(int orderId)
	// {
	// 	if (!LockedOrders.ContainsKey(orderId))
	// 	{
	// 		LockedOrders[orderId] = Context.ConnectionId;
	// 		await Clients.All.SendAsync("OrderLocked", orderId, Context.ConnectionId);
	// 	}
	// 	else
	// 	{
	// 		await Clients.Caller.SendAsync("OrderAlreadyLocked", orderId);
	// 	}
	// }

	public async Task UnlockOrder(int orderId)
	{
		// if (LockedOrders.TryGetValue(orderId, out var connectionId) && connectionId == Context.ConnectionId)
		// {
		// 	LockedOrders.TryRemove(orderId, out _);
		// 	await Clients.All.SendAsync("OrderUnlocked", orderId);
		// }
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		
		_orderLockService.UnlockOrder(Context.ConnectionId);
		// foreach (var order in LockedOrders)
		// {
		// 	if (order.Value == Context.ConnectionId)
		// 	{
		// 		LockedOrders.TryRemove(order.Key, out _);
		// 		await Clients.All.SendAsync("OrderUnlocked", order.Key);
		// 	}
		// }
		await base.OnDisconnectedAsync(exception);
	}
}

public interface INotifyOrderTransactionService
{
	public void SendOrderTransactionNotification();
	public void ReceiveNewOrderNotification();
	public void LockOrder(int orderId);
	public void UnlockOrder(int orderId);

}

public class NotifyOrderTransactionService : INotifyOrderTransactionService
{
	private readonly IHubContext<NotifyIOrderTransaction> _hubContext;
	private readonly IUnitOfWork _unitOfWork;
	public NotifyOrderTransactionService(IHubContext<NotifyIOrderTransaction> hubContext, IUnitOfWork unitOfWork)
	{
		_hubContext = hubContext;
		_unitOfWork = unitOfWork;
	}
	private object GetOrderSummary()
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
	public void SendOrderTransactionNotification()
	{
		_hubContext.Clients.All.SendAsync("ReceiveOrderTransaction",GetOrderSummary());
	}

	public void ReceiveNewOrderNotification()
	{
		_hubContext.Clients.All.SendAsync("ReceiveNewOrderNotification");
		SendOrderTransactionNotification();
	}

	public void LockOrder(int orderId)
	{
		_hubContext.Clients.All.SendAsync("LockOrder");
	}

	public void UnlockOrder(int orderId)
	{
		_hubContext.Clients.All.SendAsync("UnlockOrder");
	}
}