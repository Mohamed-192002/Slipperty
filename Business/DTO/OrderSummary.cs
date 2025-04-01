namespace Business.DTO;

public class OrderSummary
{
	public int NewOrdersCount { get; set; }            // اوردرات جديدة
	public int AttemptedOrdersCount { get; set; }      // تم المحاولة
	public int WhatsappOrdersCount { get; set; }       // واتساب
	public int HoldOrdersCount { get; set; }           // تأجيلات
	public int RecallOrdersCount { get; set; }         // اعادة اتصال
	public int StoppedOrdersCount { get; set; }        // وقف التشغيل
	public int NoAttemptYesterdayCount { get; set; }   // لم يتم المحاولة امس
	public int ConfirmedOrdersCount { get; set; }      // طلبات تم التأكيد
    public int Cancel { get; set; }
    public int TriedOrderCount {get; set;}
	public OrderSummary()
	{
		NewOrdersCount = 0;
		AttemptedOrdersCount = 0;
		WhatsappOrdersCount = 0;
		HoldOrdersCount = 0;
		RecallOrdersCount = 0;
		StoppedOrdersCount = 0;
		NoAttemptYesterdayCount = 0;
		ConfirmedOrdersCount = 0;
		Cancel = 0;
		TriedOrderCount = 0; 
	}
}
