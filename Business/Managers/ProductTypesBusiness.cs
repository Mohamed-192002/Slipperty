namespace Business.Managers
{
    public interface IProductTypesBusiness
    {
        int AddUpdate(ProductTypeDTO modelDTO);
        ProductTypeDTO GetById(int Id);
        IQueryable<ProductTypeDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class ProductTypesBusiness : IProductTypesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public ProductTypesBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(ProductTypeDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<ProductType>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.ProductTypes.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.ProductTypes.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public ProductTypeDTO GetById(int Id)
        {
            var data = unitOfWork.ProductTypes.Get(c => c.Id == Id);
            return _mapper.Map<ProductTypeDTO>(data);
        }

        public IQueryable<ProductTypeDTO> GetAll()
        {
            IQueryable<ProductType> inputQuery = unitOfWork.ProductTypes.GetAll().AsQueryable();
            IQueryable<ProductTypeDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<ProductTypeDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.ProductTypes.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.ProductTypes.Remove(entity);
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
