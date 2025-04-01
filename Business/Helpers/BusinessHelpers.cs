
namespace Business.Helpers
{
    public static class BusinessHelpers
    {
        public enum SignatureResponseMessage
        {
            SuccessInsert = 1,
            SuccessUpdate = 2,
            Quation = 0
        }

        public static class ResponseTypes
        {
            public const string Msg = "msg";
            public const string Error = "error";
        }

        public static class ResponseMessages
        {
            public const string SuccessInsert = "تم الحفظ";
            public const string SuccessUpdate = "تم التعديل";
            public const string SuccessDelete = "تم الحذف";
            public const string NotDeleted = "لا يمكن الحذف";
            public const string Error = "حدث خطأ";
        }
    }
}
