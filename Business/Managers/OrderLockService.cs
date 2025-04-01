namespace Business.Managers;

public class OrderLockService
{
	private readonly Dictionary<string, int> _lockedOrders = new(); // OrderId -> ConnectionId mapping
	private readonly object _lock = new();

	public bool TryLockOrder(int orderId, string userId)
	{
		lock (_lock)
		{
			if (_lockedOrders.ContainsKey(userId)) return false; // Order is already locked
			_lockedOrders[userId] = orderId;
			return true;
		}
	}

	public void UnlockOrder(string userId)
	{
		lock (_lock)
		{
			if (_lockedOrders.ContainsKey(userId))
			{
				_lockedOrders.Remove(userId);
			}
		}
	}

	public int GetUserOrderLocked(string userId)
	{
		lock (_lock)
		{
			return _lockedOrders.ContainsKey(userId) ? _lockedOrders.GetValueOrDefault(userId) : 0;
		}
	}

	public List<int> GetAllLockedOrderIds(string userId)
	{
		lock (_lock)
		{
			return   _lockedOrders.Where(_ => _.Key != userId).Select(_ => _.Value).ToList();
		}
	}

	public void ReleaseLocksByUser(string userId)
	{
		lock (_lock)
		{
			// var lockedOrders = _lockedOrders.Where(x => x.Key == userId).Select(x => x.Key).ToList();
			// foreach (var orderId in lockedOrders)
			// {
			   if(_lockedOrders.ContainsKey(userId))
				_lockedOrders.Remove(userId);
			// }
		}
	}
}