﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllByTenantIdAsync(string tenantId, int? skip, int? limit);

        Task<T?> GetAsync(string tenantId, string id);

        Task CreateAsync(T newModel);

        Task UpdateAsync(string id, T updatedModel);

        Task RemoveAsync(string tenantId, string id);

        Task<IEnumerable<T>> GetByFilterAsync(string tenantId, FilterDefinition<T> filter);
    }
}
