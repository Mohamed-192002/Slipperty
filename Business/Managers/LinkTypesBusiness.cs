namespace Business.Managers
{
    public interface ILinkTypesBusiness
    {
        int AddUpdate(LinkTypeDTO modelDTO);
        LinkTypeDTO GetById(int Id);
        IQueryable<LinkTypeDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class LinkTypesBusiness : ILinkTypesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _CategoryRepository;

        public LinkTypesBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository CategoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _CategoryRepository = CategoryRepository;
        }

        public int AddUpdate(LinkTypeDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<LinkType>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.LinkTypes.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.LinkTypes.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public LinkTypeDTO GetById(int Id)
        {
            var data = unitOfWork.LinkTypes.Get(c => c.Id == Id);
            return _mapper.Map<LinkTypeDTO>(data);
        }

        public IQueryable<LinkTypeDTO> GetAll()
        {
            IQueryable<LinkType> inputQuery = unitOfWork.LinkTypes.GetAll().AsQueryable();
            IQueryable<LinkTypeDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<LinkTypeDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.LinkTypes.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.LinkTypes.Remove(entity);
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
