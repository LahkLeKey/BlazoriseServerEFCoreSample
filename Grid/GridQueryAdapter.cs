// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridQueryAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   Creates the right expressions to filter and sort.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using BlazorServerEFCoreSample.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

#endregion

namespace BlazorServerEFCoreSample.Grid
{
    #region

    using System.Text;

    #endregion

    /// <summary>
    ///     Creates the right expressions to filter and sort.
    /// </summary>
    public class GridQueryAdapter
    {
        /// <summary>
        ///     Holds state of the grid.
        /// </summary>
        private readonly IContactFilters _controls;

        /// <summary>
        ///     Expressions for sorting.
        /// </summary>
        private readonly Dictionary<ContactFilterColumns, Expression<Func<Contact, string>>> _expressions =
            new()
                {
                    { ContactFilterColumns.City, c => c != null && c.City != null ? c.City : string.Empty },
                    { ContactFilterColumns.Phone, c => c != null && c.Phone != null ? c.Phone : string.Empty },
                    { ContactFilterColumns.Name, c => c != null && c.LastName != null ? c.LastName : string.Empty },
                    { ContactFilterColumns.State, c => c != null && c.State != null ? c.State : string.Empty },
                    { ContactFilterColumns.Street, c => c != null && c.Street != null ? c.Street : string.Empty },
                    { ContactFilterColumns.ZipCode, c => c != null && c.ZipCode != null ? c.ZipCode : string.Empty }
                };

        /// <summary>
        ///     Queryables for filtering.
        /// </summary>
        private readonly Dictionary<ContactFilterColumns, Func<IQueryable<Contact>, IQueryable<Contact>>> _filterQueries = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="GridQueryAdapter"/> class. 
        /// Creates a new instance of the <see cref="GridQueryAdapter"/> class.
        /// </summary>
        /// <param name="controls">
        /// The <see cref="IContactFilters"/> to use.
        /// </param>
        public GridQueryAdapter(IContactFilters controls)
        {
            this._controls = controls;

            // set up queries
            this._filterQueries = new Dictionary<ContactFilterColumns, Func<IQueryable<Contact>, IQueryable<Contact>>>
                                      {
                                          {
                                              ContactFilterColumns.City,
                                              cs => cs.Where(
                                                  c => c != null && c.City != null && this._controls.FilterText != null
                                                           ? c.City.Contains(this._controls.FilterText)
                                                           : false)
                                          },
                                          {
                                              ContactFilterColumns.Phone,
                                              cs => cs.Where(
                                                  c => c != null && c.Phone != null && this._controls.FilterText != null
                                                           ? c.Phone.Contains(this._controls.FilterText)
                                                           : false)
                                          },
                                          {
                                              ContactFilterColumns.Name,
                                              cs => cs.Where(
                                                  c => c != null && c.FirstName != null
                                                                 && this._controls.FilterText != null
                                                           ? c.FirstName.Contains(this._controls.FilterText)
                                                           : false)
                                          },
                                          {
                                              ContactFilterColumns.State,
                                              cs => cs.Where(
                                                  c => c != null && c.State != null && this._controls.FilterText != null
                                                           ? c.State.Contains(this._controls.FilterText)
                                                           : false)
                                          },
                                          {
                                              ContactFilterColumns.Street,
                                              cs => cs.Where(
                                                  c => c != null && c.Street != null
                                                                 && this._controls.FilterText != null
                                                           ? c.Street.Contains(this._controls.FilterText)
                                                           : false)
                                          },
                                          {
                                              ContactFilterColumns.ZipCode,
                                              cs => cs.Where(
                                                  c => c != null && c.ZipCode != null
                                                                 && this._controls.FilterText != null
                                                           ? c.ZipCode.Contains(this._controls.FilterText)
                                                           : false)
                                          }
                                      };
        }

        /// <summary>
        /// Get total filtered items count.
        /// </summary>
        /// <param name="query">
        /// The <see cref="IQueryable{Contact}"/> to use.
        /// </param>
        /// <returns>
        /// Asynchronous <see cref="Task"/>.
        /// </returns>
        public async Task CountAsync(IQueryable<Contact> query)
        {
            this._controls.PageHelper.TotalItemCount = await query.CountAsync();
        }

        /// <summary>
        /// Uses the query to return a count and a page.
        /// </summary>
        /// <param name="query">
        /// The <see cref="IQueryable{Contact}"/> to work from.
        /// </param>
        /// <returns>
        /// The resulting <see cref="ICollection{Contact}"/>.
        /// </returns>
        public async Task<ICollection<Contact>> FetchAsync(IQueryable<Contact> query)
        {
            query = this.FilterAndQuery(query);
            await this.CountAsync(query);
            var collection = await this.FetchPageQuery(query).ToListAsync();
            this._controls.PageHelper.PageItems = collection.Count;
            return collection;
        }

        /// <summary>
        /// Build the query to bring back a single page.
        /// </summary>
        /// <param name="query">
        /// The <see cref="IQueryable{Contact}"/> to modify.
        /// </param>
        /// <returns>
        /// The new <see cref="IQueryable{Contact}"/> for a page.
        /// </returns>
        public IQueryable<Contact> FetchPageQuery(IQueryable<Contact> query)
        {
            return query.Skip(this._controls.PageHelper.Skip).Take(this._controls.PageHelper.PageSize).AsNoTracking();
        }

        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <param name="root">
        /// The <see cref="IQueryable{Contact}"/> to start with.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IQueryable{Contact}"/> with sorts and
        ///     filters applied.
        /// </returns>
        private IQueryable<Contact> FilterAndQuery(IQueryable<Contact> root)
        {
            var sb = new StringBuilder();

            // apply a filter?
            if (!string.IsNullOrWhiteSpace(this._controls.FilterText))
            {
                var filter = this._filterQueries[this._controls.FilterColumn];
                sb.Append($"Filter: '{this._controls.FilterColumn}' ");
                root = filter(root);
            }

            // apply the expression
            var expression = this._expressions[this._controls.SortColumn];
            sb.Append($"Sort: '{this._controls.SortColumn}' ");

            // fix up name
            if (this._controls.SortColumn == ContactFilterColumns.Name && this._controls.ShowFirstNameFirst)
            {
                sb.Append("(first name first) ");
                expression = c => c.FirstName != null ? c.FirstName : string.Empty;
            }

            var sortDir = this._controls.SortAscending ? "ASC" : "DESC";
            sb.Append(sortDir);

            Debug.WriteLine(sb.ToString());

            // return the unfiltered query for total count, and the filtered for fetching
            return this._controls.SortAscending ? root.OrderBy(expression) : root.OrderByDescending(expression);
        }
    }
}