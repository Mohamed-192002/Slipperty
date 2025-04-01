namespace Business.Managers
{
    public interface IUserAddressesBusiness
    {
        int AddUpdate(UserAddressDTO modelDTO);
        UserAddressDTO GetById(int Id);
        IQueryable<UserAddressDTO> GetAll();
        IQueryable<UserAddressDTO> GetUserAddresses(string userId);
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class UserAddressesBusiness : IUserAddressesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public UserAddressesBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(UserAddressDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<UserAddress>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.UserAddresses.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.UserAddresses.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public UserAddressDTO GetById(int Id)
        {
            var data = unitOfWork.UserAddresses.Get(c => c.Id == Id);
            return _mapper.Map<UserAddressDTO>(data);
        }

        public IQueryable<UserAddressDTO> GetAll()
        {
            IQueryable<UserAddress> inputQuery = unitOfWork.UserAddresses.GetAll().AsQueryable();
            IQueryable<UserAddressDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<UserAddressDTO>(_mapper.ConfigurationProvider);
            return outputQuery;
        }
        public IQueryable<UserAddressDTO> GetUserAddresses(string userId)
        {
            IQueryable<UserAddress> inputQuery = unitOfWork.UserAddresses.GetAll(c=>c.UserId == userId).AsQueryable();
            IQueryable<UserAddressDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<UserAddressDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.UserAddresses.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.UserAddresses.Remove(entity);
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
