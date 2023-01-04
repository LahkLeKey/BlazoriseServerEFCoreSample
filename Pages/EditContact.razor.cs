// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditContact.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The edit contact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using BlazorServerEFCoreSample.Data;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

#endregion

namespace BlazorServerEFCoreSample.Pages
{
    /// <summary>
    ///     The edit contact.
    /// </summary>
    public partial class EditContact
    {
        /// <summary>
        ///     Avoid concurrent requests
        /// </summary>
        private bool _busy;

        /// <summary>
        ///     A concurrency error needs resolution
        /// </summary>
        private bool _concurrencyError;

        /// <summary>
        ///     An error occurred in the update
        /// </summary>
        private bool _error;

        /// <summary>
        ///     _error message
        /// </summary>
        private string _errorMessage = string.Empty;

        /// <summary>
        ///     Id of contact to edit
        /// </summary>
        [Parameter]
        public int ContactId { get; set; }

        /// <summary>
        ///     Contact being edited
        /// </summary>
        private Contact Contact { get; set; } = new();

        /// <summary>
        ///     The <see cref="ContactContext" /> for database access.
        /// </summary>
        private ContactContext? Context { get; set; }

        /// <summary>
        ///     Database version when concurrency issues exist
        /// </summary>
        private Contact DbContact { get; set; } = new();

        /// <summary>
        ///     Implement <see cref="IDisposable" />.
        /// </summary>


        public void Dispose()
        {
            this.Context?.Dispose();
        }

        /// <summary>
        ///     Start it up
        /// </summary>
        /// <returns>Task</returns>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
        /// <exception cref="InvalidOperationException">More than one element satisfies the condition in <paramref name="predicate" />.</exception>
        protected override async Task OnInitializedAsync()
        {
            this._busy = true;

            try
            {
                this.Context = await this.DbFactory.CreateDbContextAsync();

                if (this.Context?.Contacts is not null)
                {
                    var contact = await this.Context.Contacts.SingleOrDefaultAsync(c => c.Id == this.ContactId);

                    if (contact is not null) this.Contact = contact;
                }
            }
            finally
            {
                this._busy = false;
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        ///     Bail out!
        /// </summary>
        private void Cancel()
        {
            this._busy = true;
            this.Navigation.NavigateTo($"/{this.PageHelper.Page}");
        }

        /// <summary>
        /// Result of form validation
        /// </summary>
        /// <param name="success">
        /// Success when model is valid
        /// </param>
        /// <returns>
        /// Task
        /// </returns>
        private async Task ValidationResultAsync(bool success)
        {
            if (this._busy) return;

            if (!success)
            {
                // still need to edit model
                this._error = false;
                this._concurrencyError = false;
                return;
            }

            this._busy = true; // async
            try
            {
                if (this.Context is not null) await this.Context.SaveChangesAsync();

                this.EditSuccessState.Success = true;

                // go to view to see the record
                this.Navigation.NavigateTo($"/view/{this.Contact.Id}");
            }
            catch (DbUpdateConcurrencyException updateConcurrencyException)
            {
                this.EditSuccessState.Success = false;

                // concurrency issues!
                this._concurrencyError = true;

                // get values from database
                var dbValues = await updateConcurrencyException.Entries[0].GetDatabaseValuesAsync();

                if (dbValues is null)
                {
                    // deleted - show contact not found
                    this.Navigation.NavigateTo($"/view/{this.Contact.Id}");
                    return;
                }

                // bind to show labels on form
                this.DbContact = (Contact) dbValues.ToObject();

                // move to original so second submit works (unless there is another concurrent edit)
                updateConcurrencyException.Entries[0].OriginalValues.SetValues(dbValues);
                this._error = false;
                this._busy = false;
            }
            catch (Exception ex)
            {
                this.EditSuccessState.Success = false;

                // unknown exception
                this._error = true;
                this._errorMessage = ex.Message;
                this._busy = false;
            }
        }
    }
}