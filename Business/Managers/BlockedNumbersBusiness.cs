namespace Business.Managers
{
    public interface IBlockedNumbersBusiness
    {
        int AddUpdate(BlockedNumberDTO modelDTO);
        BlockedNumberDTO GetById(int Id);
        IQueryable<BlockedNumberDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class BlockedNumbersBusiness : IBlockedNumbersBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public BlockedNumbersBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(BlockedNumberDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<BlockedNumber>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.BlockedNumbers.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.BlockedNumbers.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public BlockedNumberDTO GetById(int Id)
        {
            var data = unitOfWork.BlockedNumbers.Get(c => c.Id == Id);
            return _mapper.Map<BlockedNumberDTO>(data);
        }

        public IQueryable<BlockedNumberDTO> GetAll()
        {
            IQueryable<BlockedNumber> inputQuery = unitOfWork.BlockedNumbers.GetAll().AsQueryable();
            IQueryable<BlockedNumberDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<BlockedNumberDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.BlockedNumbers.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.BlockedNumbers.Remove(entity);
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
