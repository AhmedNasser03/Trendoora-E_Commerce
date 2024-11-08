using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Models
{
	public class DesireList
	{
		[Key, MaxLength(200)]
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; }

		[ForeignKey("Customer")]
		public string CustomerId { get; set; }
		public ApplicationUser Customer { get; set; }

		public virtual ICollection<DesireListItems> DesireListItems { get; set; } = new List<DesireListItems>();

	}
}
