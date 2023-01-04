// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contact.cs" company="">
//   
// </copyright>
// <summary>
//   Contact entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Data
{
    #region

    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>
    ///     Contact entity.
    /// </summary>
    public class Contact
    {
        /// <summary>
        ///     City.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string? City { get; set; }

        /// <summary>
        ///     First name.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string? FirstName { get; set; }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Optional last name.
        /// </summary>
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string? LastName { get; set; }

        /// <summary>
        ///     Phone.
        /// </summary>
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 digits.")]
        public string? Phone { get; set; }

        /// <summary>
        ///     State code.
        /// </summary>
        [Required]
        [StringLength(3, ErrorMessage = "State abbreviation cannot exceed 3 characters.")]
        public string? State { get; set; }

        /// <summary>
        ///     Street.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Street cannot exceed 100 characters.")]
        public string? Street { get; set; }

        /// <summary>
        ///     Zip code.
        /// </summary>
        [Required]
        [RegularExpression(
            @"^\d{5}(?:[-\s]\d{4})?$",
            ErrorMessage = "Enter a valid zipcode in 55555 or 55555-5555 format")]
        public string? ZipCode { get; set; }
    }
}