namespace Business.Managers
{
    public interface IRegionsBusiness
    {
        int AddUpdate(RegionDTO modelDTO);
        RegionDTO GetById(int Id);
        RegionDTO GetByName(string Name);
        IQueryable<RegionDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class RegionsBusiness : IRegionsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public RegionsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(RegionDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Region>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Regions.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Regions.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public RegionDTO GetById(int Id)
        {
            var data = unitOfWork.Regions.Get(c => c.Id == Id);
            return _mapper.Map<RegionDTO>(data);
        }

        public RegionDTO GetByName(string Name)
        {
            var data = unitOfWork.Regions.Get(c => c.Name == Name);
            return _mapper.Map<RegionDTO>(data);
        }

        public IQueryable<RegionDTO> GetAll()
        {
            IQueryable<Region> inputQuery = unitOfWork.Regions.GetAll().AsQueryable();
            IQueryable<RegionDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<RegionDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Regions.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Regions.Remove(entity);
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
