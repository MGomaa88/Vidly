﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;
using System.ComponentModel.DataAnnotations;

namespace Vidly.ViewModels
{
    public class MovieFormViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }


        [Display(Name = "Genre")]
        [Required]
        public byte? GenreId { get; set; }

        [Display(Name = "Release Date")]
        [Required]
        public DateTime? ReleaseDate { get; set; }


        [Display(Name = "Number in Stock")]
        [Range(1, 20)]
        [Required]
        public byte ? NumberInStock { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

        public string Title 
        { 
        
            get
            {
                return Id != 0 ?
                    "Edit Movie" : "New Movie";
                
            }
        }
        public MovieFormViewModel()
        {
            Id = 0;
        }

        public MovieFormViewModel(Movie Movie)
        {

                Id = Movie.Id;
                Name = Movie.Name;
                ReleaseDate = Movie.ReleaseDate;
                NumberInStock = Movie.NumberInStock;
                GenreId = Movie.GenreId;
        }
    }
}