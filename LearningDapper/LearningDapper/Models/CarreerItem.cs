﻿namespace LearningDapper.Models
{
    public class CareerItem
    {
        public Career Career { get; set; }
        public Course Course { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Order { get; set; }
    }
}