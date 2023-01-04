// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactRow.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The contact row.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using BlazorServerEFCoreSample.Data;

using Microsoft.AspNetCore.Components;

#endregion

namespace BlazorServerEFCoreSample.Shared
{
    /// <summary>
    /// The contact row.
    /// </summary>
    public partial class ContactRow
    {
        /// <summary>
        /// The _current contact.
        /// </summary>
        private Contact _currentContact = new();

        /// <summary>
        ///     The <see cref="Contact" /> being rendered.
        /// </summary>
        [Parameter]
        public Contact CurrentContact
        {
            get => this._currentContact;
            set
            {
                if (value is not null && !value.Equals(this._currentContact))
                {
                    this._currentContact = value;
                    this.DeleteConfirmation = false;
                }
            }
        }

        /// <summary>
        ///     Event to raise when a contact delete is requested.
        /// </summary>
        [Parameter]
        public EventCallback DeleteContact { get; set; }

        /// <summary>
        ///     Overall wrapper of functionality.
        /// </summary>
        [CascadingParameter]
        public GridWrapper? Wrapper { get; set; }

        /// <summary>
        ///     Returns <c>true</c> if conditions for delete are met.
        /// </summary>
        private bool CanDelete =>
            !this.DeleteConfirmation && (this.Wrapper?.DeleteRequestId == 0
                                         || this.Wrapper?.DeleteRequestId == this.CurrentContact?.Id);

        /// <summary>
        ///     Confirm the delete.
        /// </summary>
        private bool DeleteConfirmation { get; set; }

        /// <summary>
        ///     Correctly formatted name.
        /// </summary>
        private string Name =>
            this.Filters.ShowFirstNameFirst
                ? $"{this.CurrentContact?.FirstName} {this.CurrentContact?.LastName}"
                : $"{this.CurrentContact?.LastName}, {this.CurrentContact?.FirstName}";

        /// <summary>
        ///     Navigate to view.
        /// </summary>
        private string ViewLink => $"View/{this.CurrentContact?.Id}";

        /// <summary>
        /// Called based on confirmation.
        /// </summary>
        /// <param name="confirmed">
        /// <c>True</c> when confirmed
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        private async Task ConfirmAsync(bool confirmed)
        {
            if (confirmed)
            {
                await this.DeleteAsync();
            }
            else
            {
                this.DeleteConfirmation = false;

                if (this.Wrapper is not null) await this.Wrapper.DeleteRequested.InvokeAsync(0);
            }
        }

        /// <summary>
        ///     Deletes the <see cref="Contact" />.
        /// </summary>
        /// <returns>A <see cref="Task" />.</returns>
        private Task DeleteAsync()
        {
            return this.DeleteContact.InvokeAsync(this);
        }

        /// <summary>
        /// Set delete to true.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task DeleteRequestAsync()
        {
            if (this.Wrapper?.DeleteRequestId == 0)
            {
                this.DeleteConfirmation = true;
                await this.Wrapper.DeleteRequested.InvokeAsync(this.CurrentContact.Id);
            }
        }
    }
}