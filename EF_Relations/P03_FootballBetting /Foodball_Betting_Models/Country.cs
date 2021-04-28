﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting
{
    public class Country
    {
        public Country()
        {
            this.Towns = new HashSet<Town>();
        }

        public int CountryId { get; set; }

        [Required]
        public string Name { get; set; }

        [InverseProperty("Country")]
        public ICollection<Town> Towns { get; set; }

    }
}
