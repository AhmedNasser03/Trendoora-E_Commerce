﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class CategoryResultDto
	{
		public string Name { get; set; }
		public List<ProductResultDto>? products { get; set; }
	}
}