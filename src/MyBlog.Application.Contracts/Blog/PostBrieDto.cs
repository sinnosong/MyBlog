﻿namespace MyBlog.Application.Contracts.Blog
{
    public class PostBrieDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int Year { get; set; }
        public string CreateTime { get; set; }
    }
}