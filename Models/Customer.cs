﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Customer
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter customer's name!!")]
        [StringLength(255)]
        public string Name { get; set; }

        // This method to show the label title in view
        [Display(Name = "Date of Birth")]
        [Min18YearsIfAMember]
        public DateTime? BirthDay { get; set; }
        public bool IsSubscribedToNewsLetter { get; set; }

        public MembershipType MembershipType { get; set; }

        public byte MembershipTypeId { get; set; }
    }
}