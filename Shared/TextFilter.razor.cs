// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextFilter.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The text filter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace BlazorServerEFCoreSample.Shared
{
    #region

    using BlazorServerEFCoreSample.Grid;
    using Microsoft.AspNetCore.Components;
    using System.Timers;

    #endregion

    /// <summary>
    /// The text filter.
    /// </summary>
    public partial class TextFilter
    {
        /// <summary>
        ///     Wait period in (ms) after the user stops typing.
        /// </summary>
        private const int DebounceMs = 300;

        /// <summary>
        /// The filter text.
        /// </summary>
        private string? filterText;

        /// <summary>
        /// The selected column.
        /// </summary>
        private int selectedColumn;

        /// <summary>
        ///     Timer for debounce.
        /// </summary>
        private Timer? timer;

        /// <summary>
        ///     Get a reference to the global <see cref="GridWrapper" />.
        /// </summary>
        [CascadingParameter]
        public GridWrapper? Wrapper { get; set; }

        /// <summary>
        ///     Text to filter on.
        /// </summary>
        private string? FilterText
        {
            get => this.filterText;
            set
            {
                if (value != this.filterText)
                {
                    this.filterText = value;

                    // more text means restart the debounce timer
                    this.timer?.Dispose();
                    this.timer = new(DebounceMs);
                    this.timer.Elapsed += this.NotifyTimerElapsed;
                    this.timer.Enabled = true;
                }
            }
        }

        /// <summary>
        ///     Column to filter on.
        /// </summary>
        private int SelectedColumn
        {
            get => this.selectedColumn;
            set
            {
                if (value != this.selectedColumn)
                {
                    this.selectedColumn = value;
                    this.Filters.FilterColumn = (ContactFilterColumns) this.selectedColumn;
                    this.FilterText = string.Empty;
                }
            }
        }

        /// <summary>
        ///     Disposable pattern - override to dispose any ticking timers.
        /// </summary>
        /// <param name="disposing"><c>True</c> when disposing.</param>
        public void Dispose()
        {
            this.timer?.Dispose();
            this.timer = null;
        }

        /// <summary>
        ///     When ready.
        /// </summary>
        protected override void OnInitialized()
        {
            // grab filter
            this.filterText = this.Filters.FilterText;

            // grab column to filter on
            this.selectedColumn = (int) this.Filters.FilterColumn;

            base.OnInitialized();
        }

        /// <summary>
        /// Fired after debounce time.
        /// </summary>
        /// <param name="sender">
        /// Timer.
        /// </param>
        /// <param name="e">
        /// Event args.
        /// </param>
        private async void NotifyTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            this.timer?.Dispose();
            this.timer = null;
            if (this.Filters.FilterText != this.filterText)
            {
                // notify the controls
                this.Filters.FilterText = this.filterText?.Trim();

                if (this.Wrapper is not null)
                    await this.InvokeAsync(() => this.Wrapper.FilterChanged.InvokeAsync(this));
            }
        }

        /// <summary>
        /// Sets selected attribute.
        /// </summary>
        /// <param name="column">
        /// The <see cref="ContactFilterColumns"/> being evaluated.
        /// </param>
        /// <returns>
        /// The proper attribute to select the option.
        /// </returns>
        private IEnumerable<KeyValuePair<string, object>> Selected(ContactFilterColumns column)
        {
            if ((int) column == this.selectedColumn)
                return new[] { new KeyValuePair<string, object>("selected", "selected") };
            return Enumerable.Empty<KeyValuePair<string, object>>();
        }
    }
}