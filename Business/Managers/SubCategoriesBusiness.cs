namespace Business.Managers
{
    public interface ISubCategoriesBusiness
    {
        int AddUpdate(SubCategoryDTO modelDTO);
        SubCategoryDTO GetById(int Id);
        IQueryable<SubCategoryDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class SubCategoriesBusiness : ISubCategoriesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _CategoryRepository;

        public SubCategoriesBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository CategoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _CategoryRepository = CategoryRepository;
        }

        public int AddUpdate(SubCategoryDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<SubCategory>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.SubCategories.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.SubCategories.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public SubCategoryDTO GetById(int Id)
        {
            var data = unitOfWork.SubCategories.Get(c => c.Id == Id);
            return _mapper.Map<SubCategoryDTO>(data);
        }

        public IQueryable<SubCategoryDTO> GetAll()
        {
            IQueryable<SubCategory> inputQuery = unitOfWork.SubCategories.GetAll().AsQueryable();
            IQueryable<SubCategoryDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<SubCategoryDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.SubCategories.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.SubCategories.Remove(entity);
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
            var result = _CategoryRepository.Exec_SqlQueryDataSet("SearchAllTables", parameters, true);

            if (Convert.ToInt32(result.Tables[0].Rows[0]["Occurenrce"]) > 0)
                return true;
            return false;

        }
    }
}
