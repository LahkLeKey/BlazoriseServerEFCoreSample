// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewContact.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The view contact.
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
    /// The view contact.
    /// </summary>
    public partial class ViewContact
    {
        /// <summary>
        ///     Set the visual status of the successful edit alert.
        /// </summary>
        private bool _alertVisible;

        /// <summary>
        ///     Set to true when delete is successful
        /// </summary>
        private bool _deleted;

        /// <summary>
        ///     Navigation check.
        /// </summary>
        private int _lastContactId;

        /// <summary>
        ///     Tracking of asynchronous calls.
        /// </summary>
        private bool _loading;

        /// <summary>
        ///     Set to true when not found.
        /// </summary>
        private bool _notFound;

        /// <summary>
        ///     Set to true when delete is requested.
        /// </summary>
        private bool _showConfirmation;

        /// <summary>
        ///     Id from route of contact.
        /// </summary>
        [Parameter]
        public int ContactId { get; set; }

        /// <summary>
        ///     True with redirect from successful edit.
        /// </summary>
        [Parameter]
        public bool EditSuccess { get; set; }

        /// <summary>
        ///     Contact entity.
        /// </summary>
        private Contact? Contact { get; set; }

        /// <summary>
        ///     Navigated
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task OnParametersSetAsync()
        {
            if (this._lastContactId != this.ContactId)
            {
                this.EditSuccess = this.EditSuccessState.Success;
                this.EditSuccessState.Success = false;
                this._lastContactId = this.ContactId;
                await this.LoadContactAsync();
            }

            await base.OnParametersSetAsync();
        }

        /// <summary>
        /// Confirm deletion
        /// </summary>
        /// <param name="result">
        /// True when user confirmed
        /// </param>
        /// <returns>
        /// Task
        /// </returns>
        private async Task ConfirmAsync(bool result)
        {
            if (result)
            {
                await this.DeleteAsync();
            }
            else
            {
                this._showConfirmation = false;
                this.EditSuccess = false;
            }
        }

        /// <summary>
        ///     Deletes the contact.
        /// </summary>
        /// <returns>Task</returns>
        private async Task DeleteAsync()
        {
            if (this._loading) return; // avoid concurrent requests

            this._loading = true;
            await using var context = await this.DbFactory?.CreateDbContextAsync();

            if (context?.Contacts is not null)
            {
                var contact = await context.Contacts.SingleOrDefaultAsync(c => c.Id == this.ContactId);

                if (contact is not null)
                {
                    context.Contacts?.Remove(contact);
                    await context.SaveChangesAsync();
                    this._loading = false;
                    this._deleted = true;
                }
                else
                {
                    this._loading = false;

                    // show not found
                    await this.LoadContactAsync();
                }
            }
            else
            {
                this._loading = false;

                // show not found
                await this.LoadContactAsync();
            }
        }

        /// <summary>
        ///     Loads the contact
        /// </summary>
        /// <returns>Task</returns>
        private async Task LoadContactAsync()
        {
            if (this._loading) return; // avoid concurrent requests

            this._notFound = false;
            this.Contact = null;

            this._loading = true;

            await using var context = await this.DbFactory.CreateDbContextAsync();

            if (context.Contacts is not null)
            {
                this.Contact = await context.Contacts.AsNoTracking().SingleOrDefaultAsync(c => c.Id == this.ContactId);

                if (this.Contact is null) this._notFound = true;
            }

            this._loading = false;
        }
    }
}