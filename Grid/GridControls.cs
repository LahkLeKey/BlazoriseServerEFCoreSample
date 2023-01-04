// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridControls.cs" company="">
//   
// </copyright>
// <summary>
//   State of grid filters.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Grid
{
    /// <summary>
    ///     State of grid filters.
    /// </summary>
    public class GridControls : IContactFilters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridControls"/> class.
        /// </summary>
        /// <param name="pageHelper">
        /// The page helper.
        /// </param>
        public GridControls(IPageHelper pageHelper)
        {
            this.PageHelper = pageHelper;
        }

        /// <summary>
        ///     Column filtered text is against.
        /// </summary>
        public ContactFilterColumns FilterColumn { get; set; } = ContactFilterColumns.Name;

        /// <summary>
        ///     Text to filter on.
        /// </summary>
        public string? FilterText { get; set; }

        /// <summary>
        ///     Avoid multiple concurrent requests.
        /// </summary>
        public bool Loading { get; set; }

        /// <summary>
        ///     Keep state of paging.
        /// </summary>
        public IPageHelper PageHelper { get; set; }

        /// <summary>
        ///     Firstname Lastname, or Lastname, Firstname.
        /// </summary>
        public bool ShowFirstNameFirst { get; set; }

        /// <summary>
        ///     True when sorting ascending, otherwise sort descending.
        /// </summary>
        public bool SortAscending { get; set; } = true;

        /// <summary>
        ///     Column to sort by.
        /// </summary>
        public ContactFilterColumns SortColumn { get; set; } = ContactFilterColumns.Name;
    }
}