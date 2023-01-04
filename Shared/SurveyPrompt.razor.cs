// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SurveyPrompt.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The survey prompt.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Shared
{
    #region

    using Microsoft.AspNetCore.Components;

    #endregion

    /// <summary>
    /// The survey prompt.
    /// </summary>
    public partial class SurveyPrompt
    {
        // Demonstrates how a parent component can supply parameters
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Parameter]
        public string? Title { get; set; }
    }
}