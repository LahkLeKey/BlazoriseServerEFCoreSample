// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactForm.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The contact form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Shared
{
    #region

    using BlazorServerEFCoreSample.Data;

    using Microsoft.AspNetCore.Components;

    #endregion

    /// <summary>
    /// The contact form.
    /// </summary>
    public partial class ContactForm
    {
        /// <summary>
        ///     Prevent multiple asynchronous operations at the same time.
        /// </summary>
        [Parameter]
        public bool Busy { get; set; }

        /// <summary>
        ///     Let parent handle what to do on cancel.
        /// </summary>
        [Parameter]
        public EventCallback CancelRequest { get; set; }

        /// <summary>
        ///     The <see cref="Contact" /> to upsert.
        /// </summary>
        [Parameter]
        public Contact? Contact { get; set; }

        /// <summary>
        ///     The database version of <see cref="Contact" /> if a concurrency issue
        ///     exists.
        /// </summary>
        [Parameter]
        public Contact? DbContact { get; set; }

        /// <summary>
        ///     <c>True</c> if add mode.
        /// </summary>
        [Parameter]
        public bool IsAdd { get; set; }

        /// <summary>
        ///     Let parent handle result of validation.
        /// </summary>
        [Parameter]
        public EventCallback<bool> ValidationResult { get; set; }

        /// <summary>
        ///     Mode.
        /// </summary>
        private string Mode => this.IsAdd ? "Add" : "Edit";

        /// <summary>
        ///     Ask to cancel.
        /// </summary>
        /// <returns>A <see cref="Task" />.</returns>
        private Task CancelAsync()
        {
            return this.CancelRequest.InvokeAsync(null);
        }

        /// <summary>
        /// Handle form submission request.
        /// </summary>
        /// <param name="isValid">
        /// <c>True</c> when field validation passed.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        private Task HandleSubmitAsync(bool isValid)
        {
            return this.ValidationResult.InvokeAsync(isValid);
        }
    }
}