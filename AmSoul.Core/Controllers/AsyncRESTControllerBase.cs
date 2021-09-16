using AmSoul.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmSoul.Core.Controllers
{
    public abstract class AsyncRESTControllerBase<T> : BaseController where T : class, IDataModel
    {
        protected readonly IAsyncRESTService<T> _restService;

        public AsyncRESTControllerBase(IUserService userService, IAsyncRESTService<T> restService)
            : base(userService)
        {
            _restService = restService;
        }
        [HttpPost]
        public virtual async Task<ActionResult<T>> Create(T obj, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(obj.Id)) return await Update(obj.Id, obj, cancellationToken);
            await _restService.CreateAsync(obj, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = obj.Id.ToString() }, obj);
        }
        [HttpDelete("{id:length(24)}")]
        public virtual async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var obj = await _restService.GetAsync(id, cancellationToken);
            return obj == null
                ? NotFound()
                : Ok(await _restService.DeleteAsync(obj, cancellationToken));
        }

        [HttpGet]
        public virtual async Task<ActionResult<List<T>>> Get(CancellationToken cancellationToken) => await _restService.GetAsync(cancellationToken);

        [HttpGet("{id:length(24)}")]
        public virtual async Task<ActionResult<T>> Get(string id, CancellationToken cancellationToken)
        {
            var obj = await _restService.GetAsync(id, cancellationToken);
            return obj == null
                ? NotFound()
                : obj;
        }
        [HttpPut("{id:length(24)}")]
        public virtual async Task<ActionResult<T>> Update(string id, T newObj, CancellationToken cancellationToken)
        {
            var obj = await _restService.GetAsync(id, cancellationToken);
            return obj == null
                ? NotFound()
                : Ok(await _restService.PutAsync(id, newObj, cancellationToken));
        }
    }
}