namespace Business.Managers
{
    public interface IUserPaymentMethodsBusiness
    {
        int AddUpdate(UserPaymentMethodDTO modelDTO);
        int AddUpdateRange(List<UserPaymentMethodDTO> modelDTOs);
        UserPaymentMethodDTO GetById(int Id);
        IQueryable<UserPaymentMethodDTO> GetAll();
        IQueryable<UserPaymentMethodDTO> GetUserPaymentMethods(string userId);
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class UserPaymentMethodsBusiness : IUserPaymentMethodsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public UserPaymentMethodsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(UserPaymentMethodDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<UserPaymentMethod>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.UserPaymentMethods.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.UserPaymentMethods.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }
        public int AddUpdateRange(List<UserPaymentMethodDTO> objectDTOs)
        {
            int result = 0;


            var data = _mapper.Map<List<UserPaymentMethod>>(objectDTOs);

            var dataToUpdate = data.Where(c => c.Id > 0);
            var dataToAdd = data.Where(c => c.Id == 0);

            if(dataToUpdate.Count() > 0) 
                unitOfWork.UserPaymentMethods.UpdateRange(dataToUpdate);

            if(dataToAdd.Count() > 0)
                unitOfWork.UserPaymentMethods.InsertRange(dataToAdd);


            unitOfWork.Complete();
            return result;
        }

        public UserPaymentMethodDTO GetById(int Id)
        {
            var data = unitOfWork.UserPaymentMethods.Get(c => c.Id == Id);
            return _mapper.Map<UserPaymentMethodDTO>(data);
        }

        public IQueryable<UserPaymentMethodDTO> GetAll()
        {
            IQueryable<UserPaymentMethod> inputQuery = unitOfWork.UserPaymentMethods.GetAll().AsQueryable();
            IQueryable<UserPaymentMethodDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<UserPaymentMethodDTO>(_mapper.ConfigurationProvider);
            return outputQuery;
        }
        public IQueryable<UserPaymentMethodDTO> GetUserPaymentMethods(string userId)
        {
            IQueryable<UserPaymentMethod> inputQuery = unitOfWork.UserPaymentMethods.GetAll(c=>c.userId == userId).AsQueryable();
            IQueryable<UserPaymentMethodDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<UserPaymentMethodDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.UserPaymentMethods.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.UserPaymentMethods.Remove(entity);
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
