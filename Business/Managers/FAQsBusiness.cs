namespace Business.Managers
{
    public interface IFAQsBusiness
    {
        int AddUpdate(FAQDTO modelDTO);
        FAQDTO GetById(int Id);
        IQueryable<FAQDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class FAQsBusiness : IFAQsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public FAQsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(FAQDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<FAQ>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.FAQs.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.FAQs.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public FAQDTO GetById(int Id)
        {
            var data = unitOfWork.FAQs.Get(c => c.Id == Id);
            return _mapper.Map<FAQDTO>(data);
        }

        public IQueryable<FAQDTO> GetAll()
        {
            IQueryable<FAQ> inputQuery = unitOfWork.FAQs.GetAll().AsQueryable();
            IQueryable<FAQDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<FAQDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.FAQs.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.FAQs.Remove(entity);
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
