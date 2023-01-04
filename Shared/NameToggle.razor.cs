namespace BlazorServerEFCoreSample.Shared
{
    using Microsoft.AspNetCore.Components;

    public partial class NameToggle
    {
        /// <summary>
        /// Button text based on current state.
        /// </summary>
        private string Label => Filters.ShowFirstNameFirst ?
                                    "Show Last Name, First Name" :
                                    "Show First Name, Last Name";

        /// <summary>
        /// Reference to the <see cref="GridWrapper"/>.
        /// </summary>
        [CascadingParameter]
        public GridWrapper? GridWrapper { get; set; }

        /// <summary>
        /// Toggle name preference.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        private Task ToggleAsync()
        {
            Filters.ShowFirstNameFirst = !Filters.ShowFirstNameFirst;

            return GridWrapper is not null ? GridWrapper.FilterChanged.InvokeAsync(this) : Task.CompletedTask;
        }
    }
}
