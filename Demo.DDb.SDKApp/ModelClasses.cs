using Newtonsoft.Json;

namespace Demo.DDb.SDKApp
{
    public class College
    {
        [JsonProperty(PropertyName = "id")]
        public string collegeId { get; set; }
        public string collegeName { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public Branch[] branches { get; set; }
    }

    public class Branch
    {
        public string branchId { get; set; }
        public string branchName { get; set; }
        public int capacity { get; set; }
        public Course[] courses { get; set; }
    }

    public class Course
    {
        public string courseId { get; set; }
        public string courseName { get; set; }
        public bool isOtional { get; set; }
    }
}
