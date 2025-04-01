namespace Business.Managers
{
    public interface IShoppingCartsBusiness
    {
        int AddUpdate(ShoppingCartDTO modelDTO);
        int AddUpdateRange(List<ShoppingCartDTO> modelDTOs);
        ShoppingCartDTO GetById(int Id);
        IQueryable<ShoppingCartDTO> GetAll();
        IQueryable<ShoppingCartDTO> GetByUserId(string userId);
        IQueryable<ShoppingCartDTO> GetBySessionId(string sessionId);
        bool Delete(int Id, bool inRelation);
        bool DeleteOfferFromCart(int offerId, DateTime regDate);
        bool EmptyUserCart(string userId);
        bool EmptySessionCart(string sessionId);
        bool MergeSessionCartToUser(string sessionId, string userId);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class ShoppingCartsBusiness : IShoppingCartsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public ShoppingCartsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public int AddUpdate(ShoppingCartDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<ShoppingCart>(objectDTO);

            if (objectDTO.Id > 0)
            {

                unitOfWork.ShoppingCarts.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.ShoppingCarts.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }
        
        public int AddUpdateRange(List<ShoppingCartDTO> modelDTOs)
        {
            int result = 0;


            var data = _mapper.Map<List<ShoppingCart>>(modelDTOs);
            var dataToAdd = data.Where(c => c.Id == 0);
            var dataToUpdate = data.Where(c => c.Id > 0);

            if (dataToUpdate.Count() > 0)
            {

                unitOfWork.ShoppingCarts.UpdateRange(dataToAdd);
                result = 2;

            }

            if(dataToAdd.Count() > 0)
            {
                unitOfWork.ShoppingCarts.InsertRange(dataToAdd);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public ShoppingCartDTO GetById(int Id)
        {
            var data = unitOfWork.ShoppingCarts.Get(c => c.Id == Id,null, true );
            return _mapper.Map<ShoppingCartDTO>(data);
        }

        public IQueryable<ShoppingCartDTO> GetAll()
        {
            IQueryable<ShoppingCart> inputQuery = unitOfWork.ShoppingCarts.GetAll()
                .Include(c => c.Product).Include(c => c.Combination).Include(c => c.ProductCountsOffer).AsQueryable();
            IQueryable<ShoppingCartDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<ShoppingCartDTO>(_mapper.ConfigurationProvider);
            return outputQuery;
        }
        
        public IQueryable<ShoppingCartDTO> GetByUserId(string userId)
        {
            //IQueryable<ShoppingCart> inputQuery = unitOfWork.ShoppingCarts.GetAll(c=>c.UserId == userId)
            //    .Include(c=>c.Product).Include(c => c.Combination).Include(c => c.ProductCountsOffer).AsQueryable();

            IQueryable<ShoppingCart> inputQuery = unitOfWork.ShoppingCarts
    .GetAll(c => c.UserId == userId)
    .Include(c => c.Product)
    .Include(c => c.Combination)
    .Include(c => c.ProductCountsOffer)
    .AsQueryable()
    .AsSplitQuery();
            IQueryable<ShoppingCartDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<ShoppingCartDTO>(_mapper.ConfigurationProvider);
            return outputQuery;
        }

        public IQueryable<ShoppingCartDTO> GetBySessionId(string sessionId)
        {
            //IQueryable<ShoppingCart> inputQuery = unitOfWork.ShoppingCarts.GetAll(c=>c.UserId == userId)
            //    .Include(c=>c.Product).Include(c => c.Combination).Include(c => c.ProductCountsOffer).AsQueryable();

            IQueryable<ShoppingCart> inputQuery = unitOfWork.ShoppingCarts
    .GetAll(c => c.GuestSessionId == sessionId)
    .Include(c => c.Product)
    .Include(c => c.Combination)
    .Include(c => c.ProductCountsOffer)
    .AsQueryable()
    .AsSplitQuery();
            IQueryable<ShoppingCartDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<ShoppingCartDTO>(_mapper.ConfigurationProvider);
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
                var entity = unitOfWork.ShoppingCarts.Get(a => a.Id == Id);
                if (entity == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.ShoppingCarts.Remove(entity);
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

        public bool DeleteOfferFromCart(int offerId, DateTime regDate)
        {
            
                var entities = unitOfWork.ShoppingCarts.GetAll(a => a.offerId == offerId && a.RegDate == regDate);
                if (entities == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.ShoppingCarts.RemoveRange(entities);
                int? isDeleted = unitOfWork.Complete();
                if (isDeleted.HasValue && isDeleted.Value >= 0)
                    return true;
                else
                    return false;
        }
        public bool EmptyUserCart(string userId)
        {
            
                var entities = unitOfWork.ShoppingCarts.GetAll(a => a.UserId == userId);
                if (entities == null)
                    throw new Exception("ErrorNotFoundCodeMsg");

                unitOfWork.ShoppingCarts.RemoveRange(entities);
                int? isDeleted = unitOfWork.Complete();
                if (isDeleted.HasValue && isDeleted.Value >= 0)
                    return true;
                else
                    return false;
        }
        public bool EmptySessionCart(string sessionId)
        {
            
                var entities = unitOfWork.ShoppingCarts.GetAll(a => a.GuestSessionId == sessionId);
                if (entities == null)
                    //throw new Exception("ErrorNotFoundCodeMsg");
                    return true;

                unitOfWork.ShoppingCarts.RemoveRange(entities);
                int? isDeleted = unitOfWork.Complete();
                if (isDeleted.HasValue && isDeleted.Value >= 0)
                    return true;
                else
                    return false;
        }


        public bool MergeSessionCartToUser(string sessionId, string userId)
        {

            var entities = unitOfWork.ShoppingCarts.GetAll(a => a.GuestSessionId == sessionId);
            if (!entities.Any())
                return true;

            entities.ForEachAsync(c => c.GuestSessionId = null);
            entities.ForEachAsync(c => c.UserId = userId);

            unitOfWork.ShoppingCarts.UpdateRange(entities);
            int? isUpdated = unitOfWork.Complete();
            if (isUpdated.HasValue && isUpdated.Value >= 0)
                return true;
            else
                return false;
        }
    }
}
