using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Models
{
	public class DesireListItems
	{
		[ForeignKey("DesireList")]
		public string DesireListId {  get; set; }
		public DesireList DesireList { get; set; }
		[ForeignKey("Product")]
		public string ProductId { get; set; }
		public Product Product { get; set; }

	}
}
