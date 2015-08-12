using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.OData;
using System.Web.OData.Query;

using NuClear.AdvancedSearch.Web.OData.DataAccess;

namespace NuClear.AdvancedSearch.Web.OData.Controllers
{

    public abstract class GenericODataController<TEntity> : ODataController where TEntity : class
    {
        private readonly IFinder _finder;

        protected GenericODataController(IFinder finder)
        {
            _finder = finder;
        }

        [DynamicEnableQuery]
        public IHttpActionResult Get(ODataQueryOptions<TEntity> queryOptions)
        {
            var entities = _finder.FindAll<TEntity>();

            return Ok(entities);
        }

        [DynamicEnableQuery]
        public IHttpActionResult Get([FromODataUri] long key)
        {
            var entities = _finder.FindAll<TEntity>().GetById(key);
            return Ok(SingleResult.Create(entities));
        }

        protected IHttpActionResult GetContainedEntity<TContainedEntity>(long key, string propertyName) where TContainedEntity : class
        {
            var entities = _finder.FindAll<TEntity>().GetById(key).SelectManyProperties<TEntity, TContainedEntity>(propertyName);
            return Ok(entities);
        }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var response = base.ExecuteAsync(controllerContext, cancellationToken);
            response.Wait(cancellationToken);

            this.MakeCompatibleResponse(response.Result);

            return response;
        }
    }
}