// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageHelper.cs" company="">
//   
// </copyright>
// <summary>
//   Because math is hard. Holds the state for paging.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Grid
{
    /// <summary>
    ///     Because math is hard. Holds the state for paging.
    /// </summary>
    public class PageHelper : IPageHelper
    {
        /// <summary>
        ///     Current page, 0-based.
        /// </summary>
        public int DbPage => this.Page - 1;

        /// <summary>
        ///     <c>true</c> when next page exists.
        /// </summary>
        public bool HasNext => this.Page < this.PageCount;

        /// <summary>
        ///     <c>true</c> when previous page exists.
        /// </summary>
        public bool HasPrev => this.Page > 1;

        /// <summary>
        ///     Next page number.
        /// </summary>
        public int NextPage => this.Page < this.PageCount ? this.Page + 1 : this.Page;

        /// <summary>
        ///     Current page, 1-based.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        ///     Total number of pages.
        /// </summary>
        public int PageCount => (this.TotalItemCount + this.PageSize - 1) / this.PageSize;

        /// <summary>
        ///     Items on the current page (should be less than or equal to
        ///     <see cref="PageSize" />).
        /// </summary>
        public int PageItems { get; set; }

        /// <summary>
        ///     Items on a page.
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        ///     Previous page number.
        /// </summary>
        public int PrevPage => this.Page <= 1 ? this.Page : this.Page - 1;

        /// <summary>
        ///     How many records to skip to start current page.
        /// </summary>
        public int Skip => this.PageSize * this.DbPage;

        /// <summary>
        ///     Total items across all pages.
        /// </summary>
        public int TotalItemCount { get; set; }
    }
}