// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SortIndicator.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The sort indicator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace BlazorServerEFCoreSample.Shared
{
    #region

    using BlazorServerEFCoreSample.Grid;

    using Microsoft.AspNetCore.Components;

    #endregion

    /// <summary>
    /// The sort indicator.
    /// </summary>
    public partial class SortIndicator
    {
        /// <summary>
        ///     Gets or Sets which <see cref="ContactFilterColumns" /> this indicator is for.
        /// </summary>
        [Parameter]
        public ContactFilterColumns Column { get; set; }

        /// <summary>
        ///     The symbol to render.
        /// </summary>
        private string SortSymbol => this.Filters.SortAscending ? "🔼" : "🔽";
    }
}