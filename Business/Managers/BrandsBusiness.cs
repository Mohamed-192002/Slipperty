namespace Business.Managers
{
    public interface IBrandsBusiness
    {
        int AddUpdate(BrandDTO modelDTO);
        BrandDTO GetById(int Id);
        IQueryable<BrandDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class BrandsBusiness : IBrandsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public BrandsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(BrandDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Brand>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Brands.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Brands.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public BrandDTO GetById(int Id)
        {
            var data = unitOfWork.Brands.Get(c => c.Id == Id);
            return _mapper.Map<BrandDTO>(data);
        }

        public IQueryable<BrandDTO> GetAll()
        {
            IQueryable<Brand> inputQuery = unitOfWork.Brands.GetAll().AsQueryable();
            IQueryable<BrandDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<BrandDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Brands.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Brands.Remove(entity);
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
