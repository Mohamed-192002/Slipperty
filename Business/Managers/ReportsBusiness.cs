using System.Data;

namespace Business.Managers
{
    public interface IReportsBusiness
    {
        DataTable GetBarcodeDT(string Text, string ProductName);
    }
    public class ReportsBusiness : IReportsBusiness
    {
        private readonly ICategoryRepository _categoryRepository;

        public ReportsBusiness(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public DataTable GetBarcodeDT(string Text, string ProductName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("ProductName");

            DataRow row = dt.NewRow();
            row["Text"] = Text;
            row["ProductName"] = ProductName;

            dt.Rows.Add(row);

            return dt;
        }


    }
}
