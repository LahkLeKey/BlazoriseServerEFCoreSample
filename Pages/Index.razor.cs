// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Index.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The index page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using BlazorServerEFCoreSample.Data;
using BlazorServerEFCoreSample.Grid;
using BlazorServerEFCoreSample.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

#endregion

namespace BlazorServerEFCoreSample.Pages
{
    /// <summary>
    /// The index.
    /// </summary>
    public partial class Index
    {
        /// <summary>
        ///     Keeps track of the last page loaded.
        /// </summary>
        private int _lastPage = -1;

        /// <summary>
        ///     The current page.
        /// </summary>
        [Parameter]
        public int Page { get; set; }

        /// <summary>
        ///     Current page of <see cref="Contact" />.
        /// </summary>
        private ICollection<Contact>? Contacts { get; set; }

        /// <summary>
        ///     A wrapper for grid-related activity (like delete).
        /// </summary>
        private GridWrapper? Wrapper { get; set; } = new();

        /// <summary>
        /// Main logic when getting started.
        /// </summary>
        /// <param name="firstRender">
        /// <c>true</c> for first-time render.
        /// </param>
        protected override void OnAfterRender(bool firstRender)
        {
            // Ensure we're on the same, er, right page.
            if (this._lastPage < 1)
            {
                this.Navigation.NavigateTo("/1");
                return;
            }

            // Normalize the page values.
            if (this.Filters.PageHelper.PageCount > 0)
            {
                if (this.Page < 1)
                {
                    this.Navigation.NavigateTo("/1");
                    return;
                }

                if (this.Page > this.Filters.PageHelper.PageCount)
                {
                    this.Navigation.NavigateTo($"/{this.Filters.PageHelper.PageCount}");
                    return;
                }
            }

            base.OnAfterRender(firstRender);
        }

        /// <summary>
        ///     Triggered for any paging update.
        /// </summary>
        /// <returns>A <see cref="Task" />.</returns>
        protected override async Task OnParametersSetAsync()
        {
            // Make sure the page really changed.
            if (this.Page != this._lastPage)
            {
                this._lastPage = this.Page;
                await this.ReloadAsync();
            }

            await base.OnParametersSetAsync();
        }

        /// <summary>
        ///     Deletes a contact.
        /// </summary>
        /// <returns>A <see cref="Task" />.</returns>
        private async Task DeleteContactAsync()
        {
            var context = await this.DbFactory.CreateDbContextAsync();
            this.Filters.Loading = true;

            if (this.Wrapper is not null && context.Contacts is not null)
            {
                var contact = await context.Contacts.FirstAsync(c => c.Id == this.Wrapper.DeleteRequestId);

                if (contact is not null)
                {
                    context.Contacts?.Remove(contact);
                    await context.SaveChangesAsync();
                }
            }

            this.Filters.Loading = false;

            await this.ReloadAsync();
        }


        /// <summary>
        /// Helper method to set disabled on class for paging.
        /// </summary>
        /// <param name="condition">
        /// <c>true</c> when the element is active (and therefore should not be disabled).
        /// </param>
        /// <returns>
        /// The string literal "disabled" or an empty string.
        /// </returns>
        private string IsDisabled(bool condition)
        {
            return !this.Filters.Loading && condition ? string.Empty : "disabled";
        }

        /// <summary>
        ///     Reload page based on filters and paging controls.
        /// </summary>
        /// <returns>A <see cref="Task" />.</returns>
        private async Task ReloadAsync()
        {
            if (this.Filters.Loading || this.Page < 1) return;

            this.Filters.Loading = true;

            if (this.Wrapper is not null) this.Wrapper.DeleteRequestId = 0;

            this.Contacts = new List<Contact>();

            await using var context = await this.DbFactory.CreateDbContextAsync();
            var query = context.Contacts?.AsQueryable();

            if (query is not null)

                // run the query to load the current page
                this.Contacts = await this.QueryAdapter.FetchAsync(query);

            // now we're done
            this.Filters.Loading = false;
        }

        /// <summary>
        /// Used to toggle the grid sort. Will either switch to "ascending" on a new
        ///     column, or toggle between "ascending" and "descending" on a column with the
        ///     sort already set.
        /// </summary>
        /// <param name="contactFilterColumns">
        /// The <see cref="ContactFilterColumns"/> to toggle.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        private Task ToggleAsync(ContactFilterColumns contactFilterColumns)
        {
            if (this.Filters.SortColumn == contactFilterColumns) this.Filters.SortAscending = !this.Filters.SortAscending;
            else this.Filters.SortColumn = contactFilterColumns;
            return this.ReloadAsync();
        }
    }
}