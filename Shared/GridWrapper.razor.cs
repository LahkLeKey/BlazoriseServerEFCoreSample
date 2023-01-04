﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridWrapper.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The grid wrapper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Shared
{
    #region

    using Microsoft.AspNetCore.Components;

    #endregion

    /// <summary>
    /// The grid wrapper.
    /// </summary>
    public partial class GridWrapper
    {
        /// <summary>
        ///     Renders whatever is inside the tags and cascades a copy of itself.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        ///     Call when a contact is requested to be deleted.
        /// </summary>
        [Parameter]
        public EventCallback<int> DeleteRequested { get; set; }

        /// <summary>
        ///     The id of the <see cref="Contact" /> to be deleted.
        /// </summary>
        public int DeleteRequestId { get; set; }

        /// <summary>
        ///     Call when the filter has changed to refresh the <see cref="Contact" />
        ///     list.
        /// </summary>
        [Parameter]
        public EventCallback FilterChanged { get; set; }
    }
}