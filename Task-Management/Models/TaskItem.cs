﻿using System.ComponentModel.DataAnnotations;

namespace Task_Management.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        [Required]
        public string Priority { get; set; }
        public int? AssigneeId { get; set; }
        public User? User { get; set; }
        //public object Assignee { get; internal set; }
    }
}
