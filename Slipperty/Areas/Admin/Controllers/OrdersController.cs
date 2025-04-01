using Business.Helpers.Extensions;
using ClosedXML.Excel;
using Infrastructure.Contracts;
using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public partial class OrdersController : BaseController
    {
        private readonly IOrdersBusiness _ordersBusiness;
        private readonly IGovernmentsBusiness _governmentsBusiness;
        private readonly IRegionsBusiness _regionsBusiness;

        private readonly IUserAddressesBusiness _userAddressesBusiness; 
        private readonly IProductsBusiness _productsBusiness;
        private readonly OrderTransactionService _orderTransactionService;
        private readonly IWhatsAppAPIBusiness _whatsAppApiBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OrderContextService _orderContextService;
        private readonly OrderLockService _orderLockService;
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(IOrdersBusiness ordersBusiness, ICategoriesBusiness categoriesBusiness, UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness, OrderTransactionService orderTransactionService, IWhatsAppAPIBusiness whatsAppApiBusiness, IProductsBusiness productsBusiness, OrderContextService orderContextService, IUserAddressesBusiness userAddressesBusiness, IUnitOfWork unitOfWork, OrderLockService orderLockService, IGovernmentsBusiness governmentsBusiness, IRegionsBusiness regionsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _ordersBusiness = ordersBusiness;
            _httpContextAccessor = httpContextAccessor;
            _orderTransactionService = orderTransactionService;
            _whatsAppApiBusiness = whatsAppApiBusiness;
            _productsBusiness = productsBusiness;
            _orderContextService = orderContextService;
            _userAddressesBusiness = userAddressesBusiness;
            _unitOfWork = unitOfWork;
            _orderLockService = orderLockService;
            _governmentsBusiness = governmentsBusiness;
            _regionsBusiness = regionsBusiness;
        }

        public IActionResult Index(int? page)
        {
            //List<OrderHeadDTO> DataList = _OrdersBusiness.GetAll().OrderBy(u => u.Id).ToList();

            ////Paginantion
            //const int pageSize = 25; // Number of items per page
            //int pageNumber = (page ?? 1);
            //// Calculate total number of pages
            //int totalPages = (int)Math.Ceiling((double)DataList.Count / pageSize);
            //totalPages = totalPages == 0 ? 1 : totalPages;

            //// Paginate the data
            //DataList = DataList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ////Get Displayed List Count
            //int DisplayedItemsCount = DataList.Count;

            //SetPagination(pageSize, pageNumber, totalPages, DisplayedItemsCount);

            return View();
        }


        public IActionResult ExportOrders()
        {
            
            IEnumerable<OrderHeadDTO> DataList = _ordersBusiness.GetAll().OrderBy(u => u.Id);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");
                var currentRow = 1;

               
                worksheet.Cell(currentRow, 2).Value = "اسم العميل";
                worksheet.Cell(currentRow, 3).Value = "تاريخ الاوردر";
                worksheet.Cell(currentRow, 4).Value = "رقم الاوردر";
                worksheet.Cell(currentRow, 5).Value = "طريقة الدفع";
                worksheet.Cell(currentRow, 6).Value = "المحافظة";
                worksheet.Cell(currentRow, 7).Value = "المنطقة";
                worksheet.Cell(currentRow, 8).Value = "العنوان";
                worksheet.Cell(currentRow, 9).Value = "الصنف";
                worksheet.Cell(currentRow, 10).Value = "الكمية";
                worksheet.Cell(currentRow, 11).Value = "السعر";
                worksheet.Cell(currentRow, 12).Value = "الاجمالي";

                currentRow++;  

             
                foreach (var order in DataList)
                {
                    // For each order, first add the order head data (once for each order)
                    var orderHeadRow = currentRow;
                    worksheet.Cell(currentRow, 2).Value = order?.fullName;  // Customer Name
                    worksheet.Cell(currentRow, 3).Value = order?.RegDate.ToString("yyyy-MM-dd");  // Order Date
                    worksheet.Cell(currentRow, 4).Value = order?.orderNo;  // Order No
                    worksheet.Cell(currentRow, 5).Value = order?.PaymentMethod?.PaymentMethod?.Name;  // Payment Method (if available)
                    worksheet.Cell(currentRow, 6).Value = order?.Government?.Name;  // Government (if available)
                    worksheet.Cell(currentRow, 7).Value = order?.Region?.Name;  
                    worksheet.Cell(currentRow, 8).Value = order?.Address;  
                    worksheet.Cell(currentRow, 12).Value = order?.NetAmount;  

                    currentRow++; // Move to the next row for the order details

                    // Now, loop through the order details (items) for the current order
                    if (order?.OrderDetails != null && order.OrderDetails.Any())
                    {
                        foreach (var detail in order.OrderDetails)
                        {
                            // If the detail is available, write it to the sheet
                            worksheet.Cell(currentRow, 9).Value = detail?.Product?.ArbName;  // Item Name (from ProductDTO)
                            worksheet.Cell(currentRow, 10).Value = detail?.Qty;  // Quantity
                            worksheet.Cell(currentRow, 11).Value = detail?.Product?.Price;  // Price (from ProductDTO)
                            worksheet.Cell(currentRow, 12).Value = detail?.Qty * detail?.Product?.Price;  // Total (Quantity * Price)
                            currentRow++;  // Move to the next row for the next item
                        }
                    }
                    else
                    {
                      
                        worksheet.Cell(currentRow, 8).Value = "No items";  // Item placeholder
                        currentRow++;
                    }

                    
                    currentRow++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
                }
            }
        }

        public  async Task<IActionResult> ExportOrdersToWhatsApp()
        {
            
            IEnumerable<OrderHeadDTO> DataList = _ordersBusiness.GetAll().OrderBy(u => u.Id);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");
                var currentRow = 1;


                worksheet.Cell(currentRow, 2).Value = "اسم العميل";
                worksheet.Cell(currentRow, 3).Value = "تاريخ الاوردر";
                worksheet.Cell(currentRow, 4).Value = "رقم الاوردر";
                worksheet.Cell(currentRow, 5).Value = "طريقة الدفع";
                worksheet.Cell(currentRow, 6).Value = "المحافظة";
                worksheet.Cell(currentRow, 7).Value = "المنطقة";
                worksheet.Cell(currentRow, 8).Value = "العنوان";
                worksheet.Cell(currentRow, 9).Value = "الصنف";
                worksheet.Cell(currentRow, 10).Value = "الكمية";
                worksheet.Cell(currentRow, 11).Value = "السعر";
                worksheet.Cell(currentRow, 12).Value = "الاجمالي";

                currentRow++;


                foreach (var order in DataList)
                {
                    // For each order, first add the order head data (once for each order)
                    var orderHeadRow = currentRow;
                    worksheet.Cell(currentRow, 2).Value = order?.fullName; // Customer Name
                    worksheet.Cell(currentRow, 3).Value = order?.RegDate.ToString("yyyy-MM-dd"); // Order Date
                    worksheet.Cell(currentRow, 4).Value = order?.orderNo; // Order No
                    worksheet.Cell(currentRow, 5).Value =
                        order?.PaymentMethod?.PaymentMethod?.Name; // Payment Method (if available)
                    worksheet.Cell(currentRow, 6).Value = order?.Government?.Name; // Government (if available)
                    worksheet.Cell(currentRow, 7).Value = order?.Region?.Name;
                    worksheet.Cell(currentRow, 8).Value = order?.Address;
                    worksheet.Cell(currentRow, 12).Value = order?.NetAmount;

                    currentRow++; // Move to the next row for the order details

                    // Now, loop through the order details (items) for the current order
                    if (order?.OrderDetails != null && order.OrderDetails.Any())
                    {
                        foreach (var detail in order.OrderDetails)
                        {
                            // If the detail is available, write it to the sheet
                            worksheet.Cell(currentRow, 9).Value =
                                detail?.Product?.ArbName; // Item Name (from ProductDTO)
                            worksheet.Cell(currentRow, 10).Value = detail?.Qty; // Quantity
                            worksheet.Cell(currentRow, 11).Value = detail?.Product?.Price; // Price (from ProductDTO)
                            worksheet.Cell(currentRow, 12).Value =
                                detail?.Qty * detail?.Product?.Price; // Total (Quantity * Price)
                            currentRow++; // Move to the next row for the next item
                        }
                    }
                    else
                    {

                        worksheet.Cell(currentRow, 8).Value = "No items"; // Item placeholder
                        currentRow++;
                    }


                    currentRow++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    var userPhone = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Name.Substring(1);
                    var res = await _whatsAppApiBusiness.SendOrdersExcelToLoginUserWhatsApp(content, userPhone);
                    return Ok();
                }
            }

            return Ok(); 
        }
       

        public IActionResult DisplayOrders(int? page){
            const int pageSize = 25; 
            int pageNumber = (page ?? 1);
            int totalPages = (int)Math.Ceiling((double)0 / pageSize);
            totalPages = totalPages == 0 ? 1 : totalPages;
            SetPagination(pageSize, pageNumber, totalPages, 25);
            return View(Enumerable.Empty<OrderHeadDTO>());
        }

        
        public async Task<IActionResult> GetOrdersDetailProduct(string? ActionMod , int? orderDetailId)
        {

		if ( orderDetailId is null || ActionMod is null) return NotFound();
            if (ActionMod.Equals("Edit"))
            {
               // var orderDetails = _ordersBusiness.mapper.ProjectTo<OrderDetailsDTO>(_ordersBusiness.GetOrderDetails((int)orderDetailId)).FirstOrDefault();
               var orderDetail = _ordersBusiness.GetOrderDetails((int)orderDetailId).Select(_ => new
                {
                   _.Id,
                   _.ProductId, 
                   _.Combination,
                   _.Qty,
                   _.OrderHeadId
               }).FirstOrDefault();
               OrderDetailsVm  orderDetails  = new(); 
               orderDetails.ProductDto = _productsBusiness.GetById(orderDetail.ProductId);
               orderDetails.CombinationDto = orderDetail.Combination;
               orderDetails.Qty = orderDetail.Qty;
               orderDetails.Id = orderDetail.Id;
               orderDetails.OrderId = orderDetail.OrderHeadId;

               //return NotFound(); 
                 return PartialView("OrdersTabs/Modal/OrderDetailModifiyProductModal", orderDetails);
            }
            if (ActionMod.Equals("Change"))
            {
                var productQuery = _productsBusiness.GetAll().Where(_ => _.StockCount > 0).Select(_ => new ProductVm()
                {
                    Id = _.Id ,
                    MainImageUrl = _.MainImageUrl,
                    Price = _.Price,
                    DiscountPrice = _.DiscountPrice,
                    ArbName = _.ArbName,
                    StockCount = _.StockCount,
                    OrderDetailId = orderDetailId
                    
		}).OrderBy(_ => _.Id).Take(5);

		return
			PartialView("OrdersTabs/Modal/OrderDetailChangeProductModal"
					, productQuery); 
            }

            if (ActionMod.Equals("Delete"))
            {
	            var orderDetail = _ordersBusiness.GetOrderDetails((int)orderDetailId).Select(_ => new
	            {
		            _.Id,
		            _.ProductId, 
		            _.Combination,
		            _.Qty,
		            _.OrderHeadId
	            }).FirstOrDefault();
	            OrderDetailsVm  orderDetails  = new(); 
	            orderDetails.ProductDto = _productsBusiness.GetById(orderDetail.ProductId);
	            orderDetails.CombinationDto = orderDetail.Combination;
	            orderDetails.Qty = orderDetail.Qty;
	            orderDetails.Id = orderDetail.Id;
	            orderDetails.OrderId = orderDetail.OrderHeadId;
	            return PartialView("OrdersTabs/Modal/DeleteOrderDetailConfirmation", orderDetails); 
            }

		return NotFound(); }

        

	public async Task<IActionResult> GetProduct(int? productId) { if
		(productId is null) return NotFound(); var product =
			_productsBusiness.GetById((int)productId); 
		return
	PartialView("OrdersTabs/Modal/OrderDetailNewProduct",
					product); 
		//return NotFound();
            }

  
	public async Task<IActionResult> GetOrders(string? status, int? page, [FromQuery]string? queryString) { 
		var OrderFilter  =
		JsonConvert.DeserializeObject<OrderFilter>(queryString??"");
		try
		{
			const int pageSize = 25; int pageNumber = (page ?? 1); var
				filter2 = ApplyOrdersFilter();//for order tabes var
			var newOrders = filter2(Convert.ToInt32(status)) .Where(_ =>
				OrderFilter == null || ( (
					                         string.IsNullOrWhiteSpace(OrderFilter.Phone)
					                         ||
					                         OrderFilter.Phone.Contains(_.phoneNumber)
					                         ||
					                         OrderFilter.Phone.Contains(_.phoneNumber2))
				                         && ( OrderFilter.OrderDate == null |
				                              OrderFilter.OrderDate ==
				                              _.RegDate) && (
					                         string.IsNullOrWhiteSpace(OrderFilter.Address)
					                         ||
					                         OrderFilter.Address.Contains(_.Address))
				                         && ( OrderFilter.ShipmentQty == null ||
				                              _.OrderDetails.Count() ==
				                              OrderFilter.ShipmentQty)
				                         &&(OrderFilter.OrderNos.Count() == 0  || 
					                         OrderFilter.OrderNos.Contains(_.orderNo.ToString())
					                         )
				                         ) 
            
			).OrderBy(_ => _.Id).Skip((pageNumber > 0 ? (pageNumber - 1) : pageNumber) *
			                           pageSize) .Take(pageSize).ToList();

			int totalPages = (int)Math.Ceiling((double) (await
				                                   filter2(Int32.Parse(status)).CountAsync())
			                                   / pageSize); totalPages = totalPages == 0 ? 1 :
				totalPages; SetPagination(pageSize, pageNumber,
				totalPages, newOrders.Count());
			_orderLockService.ReleaseLocksByUser(GetUserId());
			return PartialView("OrdersTabs/NewOrderTab", new OrderVm()
			{OrderStatus = status , Orders = newOrders,
				Filter = OrderFilter});


		}
		catch (Exception ex)
		{
			throw ex;

		}
		
		
	}
        
	public class OrderVm { public string OrderStatus { get; set; }
		= string.Empty; public IEnumerable<OrderHeadDTO> Orders
		{ get; set; } = Enumerable.Empty<OrderHeadDTO>();
		public OrderFilter Filter { get; set; } }
    
		 public class OrderFilter {
			 public OrderFilter()
			 {
				 OrderNos  =  new List<string>();
			 }
			public string? ProdName { get; set;} 
			public int? ShipmentQty { get; set; }
			public List<string>? OrderNos { get; set; } 
			public string? Phone { get; set; }  
			public string? Address { get; set; }
		    public DateTime? OrderDate { get; set; }  
		}


		public class OrderDetailsVm { 
			public ProductDTO? ProductDto {get; set; } 
			public VariableCombination? CombinationDto{ get; set; } 
			public int Qty { get; set; } 
			public int Id { get; set; } 
			public int OrderId { get; set; } }
        
		public class ProductVm { public int? Id { get; set; } public
			string? MainImageUrl { get; set; } public decimal?
				Price { get; set; } public decimal?
				DiscountPrice { get; set; } public string?
				ArbName { get; set; } public int StockCount {
					get; set; } public int? OrderDetailId {
						get; set; } } 
		public async Task<IActionResult> GetOrderCardDetail(int OrderId, string? status, int ActionStatus, [FromQuery]OrderCardFilterDto? queryString)
		{
			var filterObject = queryString; //JsonConvert.DeserializeObject<OrderCardFilterDto>(queryString ?? "");
			if (filterObject == null) filterObject = new OrderCardFilterDto();

			var query = GetOrder(UnlockedOrderForUser(), OrderId, status, ActionStatus,filterObject,filterObject.SortType)
				.ApplyFilters(filterObject);
			
			OrderHeadDTO orderData = query.FirstOrDefault(); 
			
			ViewBag.PageSize = await _ordersBusiness.GetAll().Where(_ => _.StatusId == orderData.StatusId).ApplyFilters(filterObject).CountAsync();
			ViewBag.PageNumber =  (await _ordersBusiness.GetAll().Where(_ =>  orderData.Id > _.Id && _.StatusId == orderData.StatusId).ApplyFilters(filterObject).CountAsync()) + 1;
			ViewBag.Governments = (await _governmentsBusiness.GetAll().ToListAsync())
				.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
				.ToList();

			ViewBag.Regions = (await _regionsBusiness.GetAll().ToListAsync())
				.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
				.ToList();
			orderData.IsHaveMoreThanOneOrder =
				await (_ordersBusiness.GetAll().Where(_ => _.phoneNumber == orderData.phoneNumber || _.phoneNumber2 == orderData.phoneNumber2 || _.phoneNumber == orderData.phoneNumber2 || _.phoneNumber2 == orderData.phoneNumber).CountAsync()) > 1;
			_orderLockService.ReleaseLocksByUser(GetUserId());
			if(orderData is not null && orderData.Id is not null)
			  _orderLockService.TryLockOrder((int)orderData.Id, GetUserId());
			orderData.OrderCardFilter = filterObject;
			return PartialView("OrdersTabs/OrderCardDetail", orderData);
	    }

		private IQueryable<OrderHeadDTO> UnlockedOrderForUser()
		{
			var LookedOrderId = _orderLockService.GetAllLockedOrderIds(GetUserId());
			return _ordersBusiness.GetAll().Where(_ =>  (!LookedOrderId.Contains(_.Id ?? 0))); 
		}
		
		private IQueryable<OrderHeadDTO> GetOrder(IQueryable<OrderHeadDTO> orderHeadDtos, int orderId, string? status, int actionStatus,  OrderCardFilterDto filterObject = null, string? sortType = "AES" )
		{
			IQueryable<OrderHeadDTO> query = orderHeadDtos; 
			if (filterObject?.HoursSinceLastTransaction != null)
			{
				var maxTransactions = from tran in _unitOfWork.OrderTransaction.GetAll()
					join lastTranOnOrder in _unitOfWork.OrderTransaction.GetAll().GroupBy(_ => _.OrderID)
							.Select(g => g.Max(_ => _.TransactionID))
						on tran.TransactionID equals lastTranOnOrder
					select tran;
				
				query = from orderHead in query
					join maxTran in maxTransactions
						on orderHead.Id equals maxTran.OrderID
						where EF.Functions.DateDiffHour(maxTran.CreatedAt,DateTime.Now) <= (filterObject.HoursSinceLastTransaction)
					select orderHead;
			}

			int currentOrderIdStatus = _ordersBusiness.GetAll().Where(_ => _.Id == orderId).FirstOrDefault()?.StatusId??-1;
		    if (status == "next" && currentOrderIdStatus == actionStatus)
		    {
			
		        return query.Where(o => o.Id > orderId && o.StatusId == actionStatus).ApplySort(sortType ?? "");
		    }else if (string.IsNullOrEmpty(status) && currentOrderIdStatus == actionStatus)
		    {
			    return query.Where(o => o.StatusId == actionStatus).ApplySort(sortType ?? ""); 
		    }

		    return status switch
		    {
		        "next" => query.Where(o => o.StatusId == actionStatus).ApplySort(sortType ?? ""),
		        "prev" => query.Where(o => o.Id < orderId && o.StatusId == actionStatus).ApplySort(sortType ?? "").OrderByDescending(o => o.Id),
		           _   => query.Where(o => o.Id == orderId).ApplySort(sortType ?? "")
		    };
		}
        
		
		// private IQueryable<OrderHeadDTO>? GetOrder(IQueryable<OrderHeadDTO> ordersQuery, int orderId, string? status, int actionStatus)
		// {
		// 	var filters = new Dictionary<string, object> { { "StatusId", actionStatus } };
		// 	ordersQuery = ordersQuery.ApplyFilters(filters); 
		//
		// 	return status switch
		// 	{
		// 		"next" => ordersQuery.FirstOrDefault(o => o.Id > orderId) ?? ordersQuery,
		// 		"prev" => ordersQuery.Where(o => o.Id < orderId).OrderByDescending(o => o.Id),
		// 		_ => ordersQuery.FirstOrDefault(o => o.Id == orderId)
		// 	};
		// }
		
		
		private Func<int, IQueryable<OrderHeadDTO>>? ApplyOrdersFilter() {
			return (status) =>  status == Convert.ToInt32(ActionType.New) ? _ordersBusiness.GetAll().Where(_ => _.StatusId == null || _.StatusId == status) : status == Convert.ToInt32(ActionType.NoAttamptYeasterday) ? _ordersBusiness.GetAll().Where(_ => _.RegDate >= DateTime.Now.AddDays(-3) && _.RegDate <= DateTime.Today && (_.StatusId == null || _.StatusId == (int)ActionType.New) ) : _ordersBusiness.GetAll().Where(_ => _.StatusId == status);
	    }
	    class OrderTransactionDto
	    {

		    public int OrderID;
		    public DateTime CreatedAt;

	    }
    }
}
