namespace Business.Managers
{
    public interface IOrdersBusiness
    {
        int AddUpdate(OrderHeadDTO modelDTO);
        OrderHeadDTO GetById(int Id);
        IQueryable<OrderHeadDTO> GetAll();
        
        IQueryable<OrderDetails> GetOrderDetails(int orderDetailsId);
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
         IMapper mapper { get; }
    }
    public class OrdersBusiness : IOrdersBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public OrdersBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public IMapper mapper
        {
            get { return _mapper; }
        }

        public int AddUpdate(OrderHeadDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<OrderHead>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.OrderHead.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.OrderHead.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public OrderHeadDTO GetById(int Id)
        {
            var data = unitOfWork.OrderHead.Get(c => c.Id == Id);
            return _mapper.Map<OrderHeadDTO>(data);
        }

        public IQueryable<OrderHeadDTO> GetAll()
        {
            IQueryable<OrderHead> inputQuery = unitOfWork.OrderHead.GetAll().AsSplitQuery().Include(c => c.Government).Include(c=>c.Region).Include(c => c.PaymentMethod).AsQueryable();
            IQueryable<OrderHeadDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<OrderHeadDTO>(_mapper.ConfigurationProvider);
            return outputQuery;
        }

        public IQueryable<OrderDetails> GetOrderDetails(int orderDetailsId)
        {
           return    unitOfWork.OrderDetails.GetAll(_ => _.Id == orderDetailsId);
         //   return  res.ProjectTo<OrderDetails>(_mapper.ConfigurationProvider);
        }

  

        public bool Delete(int Id, bool inRelation)
        {
            if (inRelation)
            {
                return false;
            }
            else
            {
                var entity = unitOfWork.OrderHead.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.OrderHead.Remove(entity);
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
