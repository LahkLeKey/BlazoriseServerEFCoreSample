// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddContact.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The add contact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using BlazorServerEFCoreSample.Data;

#endregion

namespace BlazorServerEFCoreSample.Pages
{
    /// <summary>
    /// The add contact.
    /// </summary>
    public partial class AddContact
    {
        /// <summary>
        ///     <c>True</c> when an asynchronous operation is running.
        /// </summary>
        private bool _busy;

        /// <summary>
        ///     <c>True</c> when an error occurred.
        /// </summary>
        private bool _error;

        /// <summary>
        ///     The error message
        /// </summary>
        private string _errorMessage = string.Empty;

        /// <summary>
        ///     <c>True</c> after successful add.
        /// </summary>
        private bool _success;

        /// <summary>
        ///     New <see cref="Contact" />.
        /// </summary>
        private Contact? Contact { get; set; }

        /// <summary>
        ///     Start with fresh <see cref="Contact" />.
        /// </summary>
        /// <returns>A <see cref="Task" />.</returns>
        protected override Task OnInitializedAsync()
        {
            this.Contact = new Contact();
            return base.OnInitializedAsync();
        }

        /// <summary>
        ///     Back to list.
        /// </summary>
        private void Cancel()
        {
            this.Navigation.NavigateTo($"/{this.PageHelper.Page}");
        }

        /// <summary>
        /// Respond to a forms submission.
        /// </summary>
        /// <param name="success">
        /// <c>True</c> when valid.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        private async Task ValidationResultAsync(bool success)
        {
            if (this._busy) return;

            if (!success)
            {
                this._success = false;
                this._error = false;
                return;
            }

            this._busy = true;

            await using var context = await this.DbFactory.CreateDbContextAsync();

            // this just attaches
            if (this.Contact is not null) context.Contacts?.Add(this.Contact);

            try
            {
                await context.SaveChangesAsync();
                this._success = true;
                this._error = false;

                // ready for the next
                this.Contact = new Contact();
                this._busy = false;
            }
            catch (Exception ex)
            {
                this._success = false;
                this._error = true;
                this._errorMessage = ex.Message;
                this._busy = false;
            }
        }
    }
}