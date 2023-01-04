// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrencyField.razor.cs" company="">
//   
// </copyright>
// <summary>
//   The concurrency field.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Shared
{
    #region

    using Microsoft.AspNetCore.Components;

    #endregion

    /// <summary>
    /// The concurrency field.
    /// </summary>
    /// <typeparam name="TModel">
    /// </typeparam>
    public partial class ConcurrencyField<TModel>
    {
        /// <summary>
        ///     Highlight properties with different values.
        /// </summary>
        private bool IsDelta;

        /// <summary>
        /// The property.
        /// </summary>
        private IComparable? property;

        /// <summary>
        ///     The <see cref="TModel" /> in the database.
        /// </summary>
        [Parameter]
        public TModel? DbModel { get; set; }

        /// <summary>
        ///     The <see cref="TModel" /> being edited.
        /// </summary>
        [Parameter]
        public TModel? Model { get; set; }

        /// <summary>
        ///     Returns the property to inspect.
        /// </summary>
        [Parameter]
        public Func<TModel, IComparable>? Property { get; set; }

        /// <summary>
        ///     Only show if concurrency conflict exists.
        /// </summary>
        private bool Show => this.Model != null && this.DbModel != null;

        /// <summary>
        /// The on initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            if (this.Property is not null && this.DbModel is not null)
            {
                this.property = this.Property(this.DbModel);

                if (this.Model is not null)
                    this.IsDelta = !this.Property(this.Model).Equals(this.Property(this.DbModel));
            }
        }
    }
}