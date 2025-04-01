namespace Business.Managers
{
    public interface ILinksBusiness
    {
        int AddUpdate(LinkDTO modelDTO);
        LinkDTO GetById(int Id);
        IQueryable<LinkDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class LinksBusiness : ILinksBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _CategoryRepository;

        public LinksBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository CategoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _CategoryRepository = CategoryRepository;
        }

        public int AddUpdate(LinkDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Link>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.Links.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Links.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public LinkDTO GetById(int Id)
        {
            var data = unitOfWork.Links.Get(c => c.Id == Id);
            return _mapper.Map<LinkDTO>(data);
        }

        public IQueryable<LinkDTO> GetAll()
        {
            IQueryable<Link> inputQuery = unitOfWork.Links.GetAll().AsQueryable();
            IQueryable<LinkDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<LinkDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.Links.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.Links.Remove(entity);
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
