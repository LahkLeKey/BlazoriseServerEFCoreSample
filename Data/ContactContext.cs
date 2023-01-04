// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactContext.cs" company="">
//   
// </copyright>
// <summary>
//   Context for the contacts database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Data
{
    #region

    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics;

    #endregion
    /// <summary>
    ///     Context for the contacts database.
    /// </summary>
    public class ContactContext : DbContext
    {
        /// <summary>
        ///     Magic strings.
        /// </summary>
        public static readonly string ContactsDb = nameof(ContactsDb).ToLower();

        /// <summary>
        ///     Magic string.
        /// </summary>
        public static readonly string RowVersion = nameof(RowVersion);

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactContext"/> class. 
        /// Inject options.
        /// </summary>
        /// <param name="options">
        /// The <see cref="DbContextOptions{ContactContext}"/>
        ///     for the context
        /// </param>
        public ContactContext(DbContextOptions<ContactContext> options)
            : base(options)
        {
            Debug.WriteLine($"{this.ContextId} context created.");
        }

        /// <summary>
        ///     List of <see cref="Contact" />.
        /// </summary>
        public DbSet<Contact>? Contacts { get; set; }

        /// <summary>
        ///     Dispose pattern.
        /// </summary>
        public override void Dispose()
        {
            Debug.WriteLine($"{this.ContextId} context disposed.");
            base.Dispose();
        }

        /// <summary>
        ///     Dispose pattern.
        /// </summary>
        /// <returns>A <see cref="ValueTask" /></returns>
        public override ValueTask DisposeAsync()
        {
            Debug.WriteLine($"{this.ContextId} context disposed async.");
            return base.DisposeAsync();
        }

        /// <summary>
        /// Define the model.
        /// </summary>
        /// <param name="modelBuilder">
        /// The <see cref="ModelBuilder"/>.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // this property isn't on the C# class
            // so we set it up as a "shadow" property and use it for concurrency
            modelBuilder.Entity<Contact>().Property<byte[]>(RowVersion).IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }
}