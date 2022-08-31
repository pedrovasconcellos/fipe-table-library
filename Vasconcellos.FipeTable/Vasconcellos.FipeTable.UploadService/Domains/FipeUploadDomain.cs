using System.Collections.Generic;
using Vasconcellos.FipeTable.UploadService.Repositories.Interfaces;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.UploadService.Domains.Interfaces;
using Vasconcellos.FipeTable.Types.Entities.Denormalized;

namespace Vasconcellos.FipeTable.UploadService.Domains
{
    public class FipeUploadDomain : IFipeUploadDomain
    {
        private readonly IRepository _repository;

        public FipeUploadDomain(IRepository repository)
        {
            this._repository = repository;
        }

        public bool HaveReferenceIdGreaterOrEquals(long referenceId)
        {
            return this._repository.HaveReferenceIdGreaterOrEquals(referenceId);
        }

        public async Task<bool> SaveFipeReference(FipeReference fipeReference)
        {
            var fipeReferencesSelected = await this._repository.GetAllAsync<FipeReference>();
            var fipeReferencesInserted = new List<FipeReference> { fipeReference }
                .Where(x => !fipeReferencesSelected.Any(y => x.Id == y.Id))
                .ToList();

            if (!fipeReferencesInserted.Any())
                return true;

            return await this._repository.InsertOneAsync(fipeReferencesInserted.SingleOrDefault());
        }

        public async Task<bool> SaveVehicleBrands(IList<FipeVehicleBrand> brands)
        {
            var brandsSelected = await this._repository.GetAllAsync<FipeVehicleBrand>();
            var brandsInserted = brands
                .Where(x => !brandsSelected.Any(y => x.Id == y.Id))
                .ToList();

            if (!brandsInserted.Any())
                return true;

            return await this._repository.InsertManyAsync(brandsInserted);
        }

        public async Task<bool> SaveVehicleModels(IList<FipeVehicleModel> models)
        {
            var modelsSelected = await this._repository.GetAllAsync<FipeVehicleModel>();
            var modelsInserted = models
                .Where(x => !modelsSelected.Any(y => x.Id == y.Id))
                .ToList();

            if (!modelsInserted.Any())
                return true;

            return await this._repository.InsertManyAsync(modelsInserted);
        }

        public async Task<bool> SaveVehicles(IList<FipeVehicleInformation> vehicles)
        {
            var vehiclesSelected = await this._repository.GetAllAsync<FipeVehicleInformation>();
            var vehiclesInserted = vehicles
                .Where(x => !vehiclesSelected.Any(y => x.Id == y.Id))
                .ToList();

            if (!vehiclesInserted.Any())
                return true;

            return await this._repository.InsertManyAsync(vehiclesInserted);
        }

        public async Task<bool> SavePrices(IList<FipeVehiclePrice> prices)
        {
            var pricesSelected = await this._repository.GetAllAsync<FipeVehiclePrice>();
            var pricesInserted = prices
                .Where(x => !pricesSelected.Any(y => x.Id == y.Id))
                .ToList();

            if (!pricesInserted.Any())
                return true;

            return await this._repository.InsertManyAsync(pricesInserted);
        }

        public async Task<bool> SaveVehiclesDenormalized(IList<FipeVehicleInformationDenormalized> vehicles)
        {
            var vehiclesSelected = await this._repository.GetAllAsync<FipeVehicleInformationDenormalized>();
            var vehiclesInserted = vehicles
                .Where(x => !vehiclesSelected.Any(y => x.Id == y.Id))
                .ToList();

            if (!vehiclesInserted.Any())
                return true;

            return await this._repository.InsertManyAsync(vehiclesInserted);
        }
    }
}
