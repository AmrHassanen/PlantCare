using Microsoft.EntityFrameworkCore;
using Rootics.Core.InterFaces;
using Rootics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rootics.EF.Repsotories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync(int page, int pageSize, string[] includes = null)
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Invalid page or pageSize parameters.");
            }

            IQueryable<T> queryable = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    queryable = queryable.Include(include);
                }
            }

            // Calculate the number of items to skip based on the page and pageSize
            int skip = (page - 1) * pageSize;

            // Apply pagination to the query
            var result = await queryable.Skip(skip).Take(pageSize).ToListAsync();

            // Serialize the result using JsonSerializerOptions with ReferenceHandler.Preserve
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32, // Set your appropriate value here
                               // Add any other options you need
            };

            var serializedResult = JsonSerializer.Serialize(result, jsonOptions);

            // Deserialize the result back to a List<T>
            var deserializedResult = JsonSerializer.Deserialize<List<T>>(serializedResult, jsonOptions);

            return deserializedResult;
        }



        public async Task<List<T>> GetAllWithIncludeAsync(int page, int pageSize, params Expression<Func<T, object>>[] includes)
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Invalid page or pageSize parameters.");
            }

            // Build the base query
            var query = _dbSet.AsQueryable();

            // Apply includes
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            // Calculate the number of items to skip based on the page and pageSize
            int skip = (page - 1) * pageSize;

            // Apply pagination to the query
            var result = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exceptions
                return false;
            }
            catch (DbUpdateException)
            {
                // Handle other update exceptions
                return false;
            }
            catch (Exception)
            {
                // Handle other exceptions
                throw;
            }
        }
    }
}