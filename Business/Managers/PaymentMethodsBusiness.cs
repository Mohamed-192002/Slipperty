namespace Business.Managers
{
    public interface IPaymentMethodsBusiness
    {
        int AddUpdate(PaymentMethodDTO modelDTO);
        PaymentMethodDTO GetById(int Id);
        IQueryable<PaymentMethodDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class PaymentMethodsBusiness : IPaymentMethodsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public PaymentMethodsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(PaymentMethodDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<PaymentMethod>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.PaymentMethods.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.PaymentMethods.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public PaymentMethodDTO GetById(int Id)
        {
            var data = unitOfWork.PaymentMethods.Get(c => c.Id == Id);
            return _mapper.Map<PaymentMethodDTO>(data);
        }

        public IQueryable<PaymentMethodDTO> GetAll()
        {
            IQueryable<PaymentMethod> inputQuery = unitOfWork.PaymentMethods.GetAll().AsQueryable();
            IQueryable<PaymentMethodDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<PaymentMethodDTO>(_mapper.ConfigurationProvider);
            return outputQuery;
        }

        public bool Delete(int Id, bool inRelation)
        {
            if (inRelation)
            {
                return false;
            }
            else
            {
                var entity = unitOfWork.PaymentMethods.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.PaymentMethods.Remove(entity);
                int? isDeleted = unitOfWork.Complete();
                if (isDeleted.HasValue && isDeleted.Value >= 0)
                    return true;
                else
                    return false;
            }
        }

        public bool hasRelation(string searchWord, string ColNameToSearch)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["SearchStr"] = searchWord;
            parameters["ColNameToSearch"] = ColNameToSearch;
            var result = _categoryRepository.Exec_SqlQueryDataSet("SearchAllTables", parameters, true);

            if (Convert.ToInt32(result.Tables[0].Rows[0]["Occurenrce"]) > 0)
                return true;
            return false;

        }
    }
}
