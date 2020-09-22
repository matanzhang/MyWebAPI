using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyWebAPI.Entities;
using MyWebAPI.Web.Attributes;

namespace MyWebAPI.Web.Controllers
{
    [RoutePrefix("api/Student"),ApiAuthorize]
    public class StudentController : ApiController
    {
        private static readonly List<Student> StudentList = new List<Student>()
        {
            new Student() {Id = "001", Name = "张三", Sex = "男", Age = 19, Dept = "软件学院"},
            new Student() {Id = "002", Name = "李丽", Sex = "女", Age = 19, Dept = "资环学院"}
        };

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentList;
        }

        [HttpGet, Route("GetByDept/{dept}")]
        public IEnumerable<Student> GetByDept(string dept)
        {
            List<Student> tempList = StudentList.Where(p => p.Dept == dept).ToList();
            return tempList;
        }
        [HttpGet]
        public Student Get(string id)
        {
            List<Student> tempList = StudentList.Where(p => p.Id == id).ToList();
            return tempList.Count == 1 ? tempList.First() : null;
        }

        [HttpPost]
        public bool Post([FromBody] Student student)
        {
            if (student == null) return false;
            if (StudentList.Where(p => p.Id == student.Id).ToList().Count > 0) return false;
            StudentList.Add(student);
            return true;
        }

        [HttpPut]
        public bool Put(string id, [FromBody] Student student)
        {
            if (student == null) return false;
            List<Student> tempList = StudentList.Where(p => p.Id == id).ToList();
            if (tempList.Count == 0) return false;
            Student originStudent = tempList[0];
            originStudent.Name = student.Name;
            originStudent.Sex = student.Sex;
            originStudent.Age = student.Age;
            originStudent.Dept = student.Dept;
            return true;
        }

        [HttpDelete]
        public bool Delete(string id)
        {
            List<Student> tempList = StudentList.Where(p => p.Id == id).ToList();
            if (tempList.Count == 0) return false;
            StudentList.Remove(tempList[0]);
            return true;
        }
    }
}