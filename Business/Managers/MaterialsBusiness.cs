namespace Business.Managers
{
    public interface IMaterialsBusiness
    {
        int AddUpdate(MaterialDTO modelDTO);
        MaterialDTO GetById(int Id);
        IQueryable<MaterialDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class MaterialsBusiness : IMaterialsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public MaterialsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(MaterialDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Material>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Materials.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Materials.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public MaterialDTO GetById(int Id)
        {
            var data = unitOfWork.Materials.Get(c => c.Id == Id);
            return _mapper.Map<MaterialDTO>(data);
        }

        public IQueryable<MaterialDTO> GetAll()
        {
            IQueryable<Material> inputQuery = unitOfWork.Materials.GetAll().AsQueryable();
            IQueryable<MaterialDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<MaterialDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Materials.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Materials.Remove(entity);
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
