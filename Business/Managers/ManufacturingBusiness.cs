namespace Business.Managers
{
    public interface IManufacturingBusiness
    {
        int AddUpdate(ManufacturingDTO modelDTO);
        ManufacturingDTO GetById(int Id);
        IQueryable<ManufacturingDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class ManufacturingBusiness : IManufacturingBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public ManufacturingBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(ManufacturingDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Manufacturing>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Manufacturing.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Manufacturing.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public ManufacturingDTO GetById(int Id)
        {
            var data = unitOfWork.Manufacturing.Get(c => c.Id == Id);
            return _mapper.Map<ManufacturingDTO>(data);
        }

        public IQueryable<ManufacturingDTO> GetAll()
        {
            IQueryable<Manufacturing> inputQuery = unitOfWork.Manufacturing.GetAll().AsQueryable();
            IQueryable<ManufacturingDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<ManufacturingDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Manufacturing.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Manufacturing.Remove(entity);
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
