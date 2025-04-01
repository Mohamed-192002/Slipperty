namespace Business.Managers
{
    public interface IGovernmentsBusiness
    {
        int AddUpdate(GovernmentDTO modelDTO);
        GovernmentDTO GetById(int Id);
        GovernmentDTO GetByName(string Name);
        IQueryable<GovernmentDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class GovernmentsBusiness : IGovernmentsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public GovernmentsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(GovernmentDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Government>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Governments.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Governments.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public GovernmentDTO GetById(int Id)
        {
            var data = unitOfWork.Governments.Get(c => c.Id == Id);
            return _mapper.Map<GovernmentDTO>(data);
        }
        public GovernmentDTO GetByName(string Name)
        {
            var data = unitOfWork.Governments.Get(c => c.Name == Name);
            return _mapper.Map<GovernmentDTO>(data);
        }

        public IQueryable<GovernmentDTO> GetAll()
        {
            IQueryable<Government> inputQuery = unitOfWork.Governments.GetAll().AsQueryable();
            IQueryable<GovernmentDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<GovernmentDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Governments.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Governments.Remove(entity);
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
