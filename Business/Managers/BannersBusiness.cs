namespace Business.Managers
{
    public interface IBannersBusiness
    {
        int AddUpdate(BannerDTO modelDTO);
        BannerDTO GetById(int Id);
        IQueryable<BannerDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class BannersBusiness : IBannersBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public BannersBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(BannerDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Banner>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Banners.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Banners.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public BannerDTO GetById(int Id)
        {
            var data = unitOfWork.Banners.Get(c => c.Id == Id);
            return _mapper.Map<BannerDTO>(data);
        }

        public IQueryable<BannerDTO> GetAll()
        {
            IQueryable<Banner> inputQuery = unitOfWork.Banners.GetAll().AsQueryable();
            IQueryable<BannerDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<BannerDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Banners.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Banners.Remove(entity);
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
