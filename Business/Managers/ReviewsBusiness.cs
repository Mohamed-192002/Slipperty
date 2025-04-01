namespace Business.Managers
{
    public interface IReviewsBusiness
    {
        int AddUpdate(ReviewDTO modelDTO);
        ReviewDTO GetById(int Id);
        IQueryable<ReviewDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class ReviewsBusiness : IReviewsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public ReviewsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(ReviewDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Review>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Reviews.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Reviews.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public ReviewDTO GetById(int Id)
        {
            var data = unitOfWork.Reviews.Get(c => c.Id == Id);
            return _mapper.Map<ReviewDTO>(data);
        }

        public IQueryable<ReviewDTO> GetAll()
        {
            IQueryable<Review> inputQuery = unitOfWork.Reviews.GetAll().AsQueryable();
            IQueryable<ReviewDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<ReviewDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Reviews.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Reviews.Remove(entity);
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
