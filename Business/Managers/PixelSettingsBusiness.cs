namespace Business.Managers
{
    public interface IPixelSettingsBusiness
    {
        int AddUpdate(PixelSettingDTO modelDTO);
        PixelSettingDTO GetById(int Id);
        IQueryable<PixelSettingDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class PixelSettingsBusiness : IPixelSettingsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public PixelSettingsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(PixelSettingDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<PixelSetting>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.PixelSettings.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.PixelSettings.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public PixelSettingDTO GetById(int Id)
        {
            var data = unitOfWork.PixelSettings.Get(c => c.Id == Id);
            return _mapper.Map<PixelSettingDTO>(data);
        }

        public IQueryable<PixelSettingDTO> GetAll()
        {
            IQueryable<PixelSetting> inputQuery = unitOfWork.PixelSettings.GetAll().AsQueryable();
            IQueryable<PixelSettingDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<PixelSettingDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.PixelSettings.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.PixelSettings.Remove(entity);
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
