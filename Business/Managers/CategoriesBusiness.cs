namespace Business.Managers
{
    public interface ICategoriesBusiness
    {
        int AddUpdate(CategoryDTO modelDTO);
        CategoryDTO GetById(int Id);
        IQueryable<CategoryDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class CategoriesBusiness : ICategoriesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _CategoryRepository;

        public CategoriesBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository CategoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _CategoryRepository = CategoryRepository;
        }

        public int AddUpdate(CategoryDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Category>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Categories.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Categories.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public CategoryDTO GetById(int Id)
        {
            var data = unitOfWork.Categories.Get(c => c.Id == Id);
            return _mapper.Map<CategoryDTO>(data);
        }

        public IQueryable<CategoryDTO> GetAll()
        {
            IQueryable<Category> inputQuery = unitOfWork.Categories.GetAll().AsQueryable();
            IQueryable<CategoryDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Categories.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Categories.Remove(entity);
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
