using Infrastructure.Models;

namespace Business.Managers
{
    public interface IProductsBusiness
    {
        int AddUpdate(ProductDTO modelDTO);
        ProductDTO GetById(int Id);
        IQueryable<ProductDTO> GetAll();
        bool Delete(int Id, bool inRelation);
        bool hasRelation(string searchWord, string ColNameToSearch);
    }
    public class ProductsBusiness : IProductsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _CategoryRepository;

        public ProductsBusiness(IUnitOfWork _unitOfWork, IMapper mapper, ICategoryRepository CategoryRepository)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            _CategoryRepository = CategoryRepository;
        }

        public int AddUpdate(ProductDTO objectDTO)
        {
            int result = 0;


            var data = _mapper.Map<Product>(objectDTO);

            if (objectDTO.Id > 0)
            {
                RemoveProductCategories(data.Id);
                RemoveProductSubCategories(data.Id);
                RemoveProductVideos(data.Id);

                foreach (var video in data?.ProductVideos)
                {
                    video.Id = 0;
                }
                RemoveProductImages(data.Id);

                if (data.ProductImages != null && data.ProductImages.Any())
                {
                    var isCorrectMainImage = data.ProductImages.Where(c => c.ImageUrl == data.MainImageUrl).FirstOrDefault();
                    var isCorrectIconImage = data.ProductImages.Where(c => c.ImageUrl == data.IconImageUrl).FirstOrDefault();
                    if (isCorrectMainImage == null)
                        data.MainImageUrl = data.ProductImages.FirstOrDefault()?.ImageUrl;
                    if (isCorrectIconImage == null)
                        data.IconImageUrl = data.ProductImages.FirstOrDefault()?.ImageUrl;
                }

                if (data.ProductImages == null || !data.ProductImages.Any())
                {
                    data.MainImageUrl = "";
                    data.IconImageUrl = "";
                }

                foreach (var image in data?.ProductImages)
                {
                    image.Id = 0;
                }

                //RemoveProductRelatedProducts(data.Id);
                //foreach (var relatedProduct in data?.RelatedProducts)
                //{
                //    relatedProduct.Id = 0;
                //}
                //RemoveProductCountsOffers(data.Id);
                data.ProductCountsOffers = UpdateProductOffers(data.Id, data?.ProductCountsOffers);
                foreach (var offer in data?.ProductCountsOffers)
                {
                    offer.Id = 0;
                }

                data.ProductVariables = UpdateProductVariables(data.Id, data?.ProductVariables);

                //data.ProductVariables = UpdateProductVariables(data.Id, data?.ProductVariables);

                //foreach (var variable in data?.ProductVariables)
                //{
                //    UpdateProductVariableValues(variable.Id, variable?.VariableValues);
                //}
                data.ProductVariables = null;
                data.VariableCombinations = UpdateProductVariableCombinations(data.Id, data?.VariableCombinations);

                unitOfWork.Products.Update(data);
                result = 2;

            }
            else
            {
                unitOfWork.Products.Insert(data);
                result = 1;
            }
            unitOfWork.Complete();
            return result;
        }

        public ProductDTO GetById(int Id)
        {
            var data = unitOfWork.Products.GetAll(c => c.Id == Id).Include(c => c.Categories).Include(c => c.SubCategories).Include(c => c.ProductImages)
                .Include(c => c.ProductVariables).ThenInclude(c => c.VariableValues).Include(c => c.VariableCombinations).Include(c => c.ProductCountsOffers)
                .Include(c => c.ProductVideos).Include(c => c.RelatedProduct).Include(c => c.Brand).Include(c => c.ProductType).Include(c => c.Material).FirstOrDefault();
            return _mapper.Map<ProductDTO>(data);
        }

        public IQueryable<ProductDTO> GetAll()
        {
            IQueryable<Product> inputQuery = unitOfWork.Products.GetAll(c => c.isDeleted == false).AsQueryable();
            //IQueryable<Product> inputQuery = unitOfWork.Products.GetAll().Include(c=>c.ProductColors).AsQueryable();
            IQueryable<ProductDTO> outputQuery;
            outputQuery = inputQuery.ProjectTo<ProductDTO>(_mapper.ConfigurationProvider);
            return outputQuery;
        }

        public bool Delete(int Id, bool inRelation)
        {
            var entity = unitOfWork.Products.Get(a => a.Id == Id);
            if (entity == null)
                throw new Exception("ErrorNotFoundCodeMsg");

            if (inRelation)
            {
                entity.isDeleted = true;
                unitOfWork.Products.Update(entity);
            }
            else
            {
                removeRelatedDate(Id);

                unitOfWork.Products.Remove(entity);
                //int? isDeleted = unitOfWork.Complete();
                //if (isDeleted.HasValue && isDeleted.Value >= 0)
                //    return true;
                //else
                //    return false;
            }
            ;
            int? isDeleted = unitOfWork.Complete();
            if (isDeleted.HasValue && isDeleted.Value >= 0)
                return true;
            else
                return false;
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

        private void removeRelatedDate(int Id)
        {
            RemoveProductCategories(Id);
            RemoveProductSubCategories(Id);
            RemoveProductVideos(Id);
            RemoveProductImages(Id);
            //RemoveProductRelatedProducts(Id);
            RemoveProductCountsOffers(Id);
            var ProductVariableCombinations = unitOfWork.VariableCombinations.GetAll(c => c.ProductId == Id);
            var productVariables = unitOfWork.ProductVariables.GetAll(c => c.ProductId == Id);
            var productVariableIds = productVariables.Select(pv => pv.Id).ToList();
            var variablesValues = unitOfWork.VariableValues.GetAll(c => productVariableIds.Contains(c.ProductVariableId));

            if (ProductVariableCombinations != null && ProductVariableCombinations.Any())
            {
                unitOfWork.VariableCombinations.RemoveRange(ProductVariableCombinations);
                unitOfWork.Complete();
            }
            if (variablesValues != null && variablesValues.Any())
            {
                unitOfWork.VariableValues.RemoveRange(variablesValues);
                unitOfWork.Complete();
            }
            if (productVariables != null && productVariables.Any())
            {
                unitOfWork.ProductVariables.RemoveRange(productVariables);
                unitOfWork.Complete();
            }

        }
        private void RemoveProductCategories(int productId)
        {
            var existingCategories = unitOfWork.ProductCategories.GetAll(c => c.ProductId == productId); // Retrieve all categories related to this product

            if (existingCategories != null && existingCategories.Any())
            {
                unitOfWork.ProductCategories.RemoveRange(existingCategories); // Delete all categories associated with the product
                unitOfWork.Complete(); // Commit the deletion to the database
            }


        }
        private void RemoveProductSubCategories(int productId)
        {
            var existingCategories = unitOfWork.ProductSubCategories.GetAll(c => c.ProductId == productId); // Retrieve all categories related to this product

            if (existingCategories != null && existingCategories.Any())
            {
                unitOfWork.ProductSubCategories.RemoveRange(existingCategories); // Delete all categories associated with the product
                unitOfWork.Complete(); // Commit the deletion to the database
            }


        }
        private void RemoveProductVideos(int productId)
        {
            var existingVideos = unitOfWork.ProductVideos.GetAll(c => c.ProductId == productId); // Retrieve all ProductVideos related to this product

            if (existingVideos != null && existingVideos.Any())
            {
                unitOfWork.ProductVideos.RemoveRange(existingVideos); // Delete all ProductVideos associated with the product
                unitOfWork.Complete(); // Commit the deletion to the database
            }


        }
        private void RemoveProductImages(int productId)
        {
            var existingImages = unitOfWork.ProductImages.GetAll(c => c.ProductId == productId); // Retrieve all ProductVideos related to this product

            if (existingImages != null && existingImages.Any())
            {
                unitOfWork.ProductImages.RemoveRange(existingImages); // Delete all ProductVideos associated with the product
                unitOfWork.Complete(); // Commit the deletion to the database
            }


        }
        private void RemoveProductRelatedProducts(int productId)
        {
            var existingRelatedProducts = unitOfWork.RelatedProducts.GetAll(c => c.ProductId == productId); // Retrieve all RelatedProducts related to this product

            if (existingRelatedProducts != null && existingRelatedProducts.Any())
            {
                unitOfWork.RelatedProducts.RemoveRange(existingRelatedProducts); // Delete all RelatedProducts associated with the product
                unitOfWork.Complete(); // Commit the deletion to the database
            }


        }
        private void RemoveProductCountsOffers(int productId)
        {
            var existingProductCountsOffers = unitOfWork.ProductCountsOffers.GetAll(c => c.ProductId == productId); // Retrieve all ProductCountsOffers related to this product

            if (existingProductCountsOffers != null && existingProductCountsOffers.Any())
            {
                unitOfWork.ProductCountsOffers.RemoveRange(existingProductCountsOffers); // Delete all ProductCountsOffers associated with the product
                unitOfWork.Complete(); // Commit the deletion to the database
            }


        }
        //private void UpdateProductVariableCombinations(int productId, IEnumerable<VariableCombination> variableCombinations)
        //{
        //    var existingCombinations = unitOfWork.VariableCombinations.GetAll(c=>c.ProductId == productId); // Retrieve all ProductCountsOffers related to this product

        //    var newCombinations = null;
        //    var updateCombinations = null;
        //    var deleteCombinations = null;

        //    if (existingCombinations != null && existingCombinations.Any())
        //    {
        //        unitOfWork.VariableCombinations.RemoveRange(existingCombinations); // Delete all ProductCountsOffers associated with the product
        //        unitOfWork.Complete(); // Commit the deletion to the database
        //    }


        //}

        private IEnumerable<VariableCombination> UpdateProductVariableCombinations(int productId, IEnumerable<VariableCombination> variableCombinations)
        {
            // Retrieve all existing combinations for the product
            var existingCombinations = unitOfWork.VariableCombinations.GetAll(c => c.ProductId == productId);

            // Initialize lists for new, updated, and deleted combinations
            var newCombinations = new List<VariableCombination>();
            var updateCombinations = new List<VariableCombination>();
            var deleteCombinations = new List<VariableCombination>();

            // Step 1: Classify combinations into new, update, or delete
            foreach (var combination in variableCombinations)
            {
                if (combination.Id == 0) // New combination (doesn't exist in DB)
                {
                    newCombinations.Add(combination);
                }
                else
                {
                    var existing = existingCombinations.FirstOrDefault(ec => ec.Id == combination.Id);
                    if (existing != null) // Existing combination that needs updating
                    {
                        updateCombinations.Add(combination);
                    }
                }
            }

            // Step 2: Find combinations to delete (existing in DB but not in the input list)
            foreach (var existing in existingCombinations)
            {
                var matchingCombination = variableCombinations.FirstOrDefault(vc => vc.Id == existing.Id);
                if (matchingCombination == null) // Combination doesn't exist in the input list, so it should be deleted
                {
                    deleteCombinations.Add(existing);
                }
            }


            // Update existing combinations in the database
            foreach (var updateCombination in updateCombinations)
            {
                var existingCombination = existingCombinations.First(ec => ec.Id == updateCombination.Id);
                existingCombination.Price = updateCombination.Price;
                existingCombination.StockCount = updateCombination.StockCount;
            }

            // Delete removed combinations from the database
            if (deleteCombinations.Any())
            {
                foreach (var combination in deleteCombinations)
                {
                    var Relation = hasRelation(combination.Id.ToString(), "CombinationId");
                    if (!Relation)
                        unitOfWork.VariableCombinations.Remove(combination);
                }
            }

            // Commit the changes to the database
            unitOfWork.Complete();

            return newCombinations;
        }

        private IEnumerable<ProductCountsOffer> UpdateProductOffers(int productId, IEnumerable<ProductCountsOffer> productCountsOfferDTOs)
        {
            // Retrieve all existing combinations for the product
            var existingOffers = unitOfWork.ProductCountsOffers.GetAll(c => c.ProductId == productId).ToList();

            // Initialize lists for new, updated, and deleted combinations
            var newOffers = new List<ProductCountsOffer>();
            var updateOffers = new List<ProductCountsOffer>();
            var deleteOffers = new List<ProductCountsOffer>();

            // Step 1: Classify combinations into new, update, or delete
            foreach (var offer in productCountsOfferDTOs)
            {
                if (offer.Id == 0) // New combination (doesn't exist in DB)
                {
                    newOffers.Add(offer);
                }
                else
                {
                    var existing = existingOffers.FirstOrDefault(ec => ec.Id == offer.Id);
                    if (existing != null) // Existing combination that needs updating
                    {
                        updateOffers.Add(offer);
                    }
                }
            }

            // Step 2: Find combinations to delete (existing in DB but not in the input list)
            foreach (var existing in existingOffers)
            {
                var matchingOffer = productCountsOfferDTOs.FirstOrDefault(vc => vc.Id == existing.Id);
                if (matchingOffer == null) // Combination doesn't exist in the input list, so it should be deleted
                {
                    deleteOffers.Add(existing);
                }
            }


            // Update existing combinations in the database
            foreach (var updateOffer in updateOffers)
            {
                var existingOffer = existingOffers.First(ec => ec.Id == updateOffer.Id);
                existingOffer.Count = updateOffer.Count;
                existingOffer.Price = updateOffer.Price;
            }

            // Delete removed combinations from the database
            if (deleteOffers.Any())
            {
                foreach (var offer in deleteOffers)
                {
                    var Relation = hasRelation(offer.Id.ToString(), "OfferId");
                    if (!Relation)
                        unitOfWork.ProductCountsOffers.Remove(offer);
                    else
                    {
                        offer.isDeleted = true;
                        unitOfWork.ProductCountsOffers.Update(offer);
                    }
                }
            }

            // Commit the changes to the database
            unitOfWork.Complete();

            return newOffers;
        }

        private IEnumerable<VariableValue> UpdateProductVariableValues(int variableId, IEnumerable<VariableValue> variableValues)
        {
            // Retrieve all existing combinations for the product
            var existingVariableValues = unitOfWork.VariableValues.GetAll(c => c.ProductVariableId == variableId);

            // Initialize lists for new, updated, and deleted combinations
            var newVariableValues = new List<VariableValue>();
            var updateVariableValues = new List<VariableValue>();
            var deleteVariableValues = new List<VariableValue>();

            // Step 1: Classify combinations into new, update, or delete
            foreach (var value in variableValues)
            {
                if (value.Id == 0) // New combination (doesn't exist in DB)
                {
                    value.ProductVariableId = variableId;
                    newVariableValues.Add(value);
                }
                else
                {
                    var existing = existingVariableValues.FirstOrDefault(ec => ec.Id == value.Id);
                    if (existing != null) // Existing combination that needs updating
                    {
                        updateVariableValues.Add(value);
                    }
                }
            }

            // Step 2: Find combinations to delete (existing in DB but not in the input list)
            foreach (var existing in existingVariableValues)
            {
                var matchingValues = variableValues.FirstOrDefault(vc => vc.Id == existing.Id);
                if (matchingValues == null) // Combination doesn't exist in the input list, so it should be deleted
                {
                    deleteVariableValues.Add(existing);
                }
            }
            if (newVariableValues.Any())
            {
                unitOfWork.VariableValues.InsertRange(newVariableValues);
            }

            // Update existing combinations in the database
            foreach (var updateCombination in updateVariableValues)
            {
                var existingValue = existingVariableValues.First(ec => ec.Id == updateCombination.Id);
                existingValue.Value = updateCombination.Value;
                existingValue.ImageUrl = updateCombination.ImageUrl;
            }

            // Delete removed combinations from the database
            if (deleteVariableValues.Any())
            {
                foreach (var value in deleteVariableValues)
                {
                    var Relation = hasRelation(value.Id.ToString(), "VariableValueId");
                    if (!Relation)
                        unitOfWork.VariableValues.Remove(value);
                }
            }

            // Commit the changes to the database
            unitOfWork.Complete();

            return newVariableValues;
        }

        private IEnumerable<ProductVariable> UpdateProductVariables(int productId, IEnumerable<ProductVariable> productVariables)
        {
            // Retrieve all existing combinations for the product
            var existingproductVariables = unitOfWork.ProductVariables.GetAll(c => c.ProductId == productId).Include(c => c.VariableValues);

            // Initialize lists for new, updated, and deleted combinations
            var newProductVariables = new List<ProductVariable>();
            var updateProductVariables = new List<ProductVariable>();
            var deleteProductVariables = new List<ProductVariable>();

            // Step 1: Classify combinations into new, update, or delete
            foreach (var Variable in productVariables)
            {

                if (Variable.Id == 0) // New combination (doesn't exist in DB)
                {
                    Variable.ProductId = productId;
                    newProductVariables.Add(Variable);
                }
                else
                {
                    var existing = existingproductVariables.FirstOrDefault(ec => ec.Id == Variable.Id);
                    if (existing != null) // Existing combination that needs updating
                    {
                        updateProductVariables.Add(Variable);
                    }
                }

            }

            // Step 2: Find combinations to delete (existing in DB but not in the input list)
            foreach (var existing in existingproductVariables)
            {
                var matchingValues = productVariables.FirstOrDefault(vc => vc.Id == existing.Id);
                if (matchingValues == null) // Combination doesn't exist in the input list, so it should be deleted
                {
                    deleteProductVariables.Add(existing);
                }
            }

            if (newProductVariables.Any())
            {

                unitOfWork.ProductVariables.InsertRange(newProductVariables);
            }

            // Update existing combinations in the database
            foreach (var updateVariable in updateProductVariables)
            {
                var existingValue = existingproductVariables.First(ec => ec.Id == updateVariable.Id);
                existingValue.Name = updateVariable.Name;
                foreach (var variableValue in existingValue?.VariableValues)
                {
                    var currentVariableValue = updateVariable.VariableValues.FirstOrDefault(c => c.Id == variableValue.Id);
                    if (currentVariableValue != null)
                    {
                        variableValue.ImageUrl = currentVariableValue.ImageUrl;
                        variableValue.Value = variableValue.Value;
                        variableValue.ProductVariableId = variableValue.ProductVariableId;

                    }
                }
            }


            // Delete removed combinations from the database
            if (deleteProductVariables.Any())
            {
                foreach (var variable in deleteProductVariables)
                {
                    var existingProductVariableValues = unitOfWork.VariableValues.GetAll(c => c.ProductVariableId == variable.Id);
                    if (existingProductVariableValues.Any())
                    {
                        unitOfWork.VariableValues.RemoveRange(existingProductVariableValues);
                        unitOfWork.Complete();
                    }
                    var Relation = hasRelation(variable.Id.ToString(), "ProductVariableId");
                    if (!Relation)
                        unitOfWork.ProductVariables.Remove(variable);
                }
            }

            // Commit the changes to the database
            unitOfWork.Complete();

            return newProductVariables;
        }

    }
}
