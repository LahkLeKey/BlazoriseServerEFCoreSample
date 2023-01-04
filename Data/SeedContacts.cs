// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeedContacts.cs" company="">
//   
// </copyright>
// <summary>
//   Generates desired number of random contacts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlazorServerEFCoreSample.Data
{
    /// <summary>
    ///     Generates desired number of random contacts.
    /// </summary>
    public class SeedContacts
    {
        /// <summary>
        ///     A sampling of cities.
        /// </summary>
        private readonly string[] _cities =
            {
                "Austin", "Denver", "Fayetteville", "Des Moines", "San Francisco", "Portland", "Monroe", "Redmond",
                "Bothel", "Woodinville", "Kent", "Kennesaw", "Marietta", "Atlanta", "Lead", "Spokane", "Bellevue",
                "Seattle"
            };

        /// <summary>
        ///     Combined with things for last names.
        /// </summary>
        private readonly string[] _colors =
            {
                "Blue", "Aqua", "Red", "Green", "Orange", "Yellow", "Black", "Violet", "Brown", "Crimson", "Gray",
                "Cyan", "Magenta", "White", "Gold", "Pink", "Lavender"
            };

        /// <summary>
        ///     More uniqueness.
        /// </summary>
        private readonly string[] _directions = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };

        /// <summary>
        ///     Use these to make names.
        /// </summary>
        private readonly string[] _gems =
            {
                "Diamond", "Crystal", "Morion", "Azore", "Sapphire", "Cobalt", "Aquamarine", "Montana", "Turquoise",
                "Lime", "Erinite", "Emerald", "Turmaline", "Jonquil", "Olivine", "Topaz", "Citrine", "Sun", "Quartz",
                "Opal", "Alabaster", "Rose", "Burgundy", "Siam", "Ruby", "Amethyst", "Violet", "Lilac"
            };

        /// <summary>
        ///     Get some randominzation in play.
        /// </summary>
        private readonly Random _random = new();

        /// <summary>
        ///     State list.
        /// </summary>
        private readonly string[] _states =
            {
                "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY",
                "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND",
                "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
            };

        /// <summary>
        ///     Street names.
        /// </summary>
        private readonly string[] _streets = { "Broad", "Wide", "Main", "Pine", "Ash", "Poplar", "First", "Third" };

        /// <summary>
        ///     Types of streets.
        /// </summary>
        private readonly string[] _streetTypes = { "Street", "Lane", "Place", "Terrace", "Drive", "Way" };

        /// <summary>
        ///     Also helpful for names.
        /// </summary>
        private readonly string[] _things =
            {
                "beard", "finger", "hand", "toe", "stalk", "hair", "vine", "street", "son", "brook", "river", "lake",
                "stone", "ship"
            };

        /// <summary>
        /// The seed database with contact count of async.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="totalCount">
        /// The total count.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task SeedDatabaseWithContactCountOfAsync(ContactContext context, int totalCount)
        {
            var count = 0;
            var currentCycle = 0;
            while (count < totalCount)
            {
                var list = new List<Contact>();
                while (currentCycle++ < 100 && count++ < totalCount) list.Add(this.MakeContact());
                if (list.Count > 0)
                {
                    context.Contacts?.AddRange(list);
                    await context.SaveChangesAsync();
                }

                currentCycle = 0;
            }
        }

        /// <summary>
        ///     Make a new contact.
        /// </summary>
        /// <returns>A random <see cref="Contact" /> instance.</returns>
        private Contact MakeContact()
        {
            var contact = new Contact
            {
                FirstName = this.RandomOne(this._gems),
                LastName = $"{this.RandomOne(this._colors)}{this.RandomOne(this._things)}",
                Phone = $"({this._random.Next(100, 999)})-555-{this._random.Next(1000, 9999)}",
                Street = $"{this._random.Next(1, 99999)} {this._random.Next(1, 999)}"
                                           + $" {this.RandomOne(this._streets)} {this.RandomOne(this._streetTypes)} {this.RandomOne(this._directions)}",
                City = this.RandomOne(this._cities),
                State = this.RandomOne(this._states),
                ZipCode = $"{this._random.Next(10000, 99999)}"
            };
            return contact;
        }

        /// <summary>
        /// Picks a random item from a list.
        /// </summary>
        /// <param name="list">
        /// A list of <c>string</c> to parse.
        /// </param>
        /// <returns>
        /// A single item from the list.
        /// </returns>
        private string RandomOne(string[] list)
        {
            var idx = this._random.Next(list.Length - 1);
            return list[idx];
        }
    }
}