namespace Business.Managers
{
    public interface IAddressTypesBusiness
    {
        int AddUpdate(AddressTypeDTO modelDTO);
        AddressTypeDTO GetById(int Id);
        IQueryable<AddressTypeDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class AddressTypesBusiness : IAddressTypesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public AddressTypesBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(AddressTypeDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<AddressType>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.AddressTypes.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.AddressTypes.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public AddressTypeDTO GetById(int Id)
        {
            var data = unitOfWork.AddressTypes.Get(c => c.Id == Id);
            return _mapper.Map<AddressTypeDTO>(data);
        }

        public IQueryable<AddressTypeDTO> GetAll()
        {
            IQueryable<AddressType> inputQuery = unitOfWork.AddressTypes.GetAll().AsQueryable();
            IQueryable<AddressTypeDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<AddressTypeDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.AddressTypes.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.AddressTypes.Remove(entity);
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
