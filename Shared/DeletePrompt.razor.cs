// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeletePrompt.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The delete prompt.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Shared
{
    #region

    using Microsoft.AspNetCore.Components;

    #endregion

    /// <summary>
    /// The delete prompt.
    /// </summary>
    public partial class DeletePrompt
    {
        /// <summary>
        ///     Delegate confirmation to parent.
        /// </summary>
        [Parameter]
        public EventCallback<bool> Confirmation { get; set; }

        /// <summary>
        /// Confirmation.
        /// </summary>
        /// <param name="confirmed">
        /// <c>True</c> when confirmed.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        private Task ConfirmAsync(bool confirmed)
        {
            return this.Confirmation.InvokeAsync(confirmed);
        }
    }
}