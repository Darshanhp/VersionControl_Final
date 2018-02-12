using CodeRep.Entities;
using CodeRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRepository
{
    public interface IRepositoryInterface
    {
        IQueryable<User> GetAllUsers();
        IQueryable<Project> GetAllProjects();
        IQueryable<File> GetAllFiles();
        IQueryable<Project> SearchProject(string projectname);
        IQueryable<File> GetFileByProject(string ProjectId);
        IQueryable<File> GetFilesByFileName(string filename,string project);
        File GetFile(string FileName, string Path);
        File GetFile(int id);
        File GetFile(string filename);
        IQueryable<Project> GetProjectbyname(string email);
        IQueryable<Project> GetProject(string email);
        Project GetProject(string projectName, string user);
        bool Insert(File file);
        bool SaveAll();
        bool Update(File origianlFile, File updatedFile);
        bool DeleteFile(int id);
        IQueryable<File> GetFilesInProject(string ProjectId);
        void Compress(System.IO.DirectoryInfo directoryPath);
        void Decompress(string zipPath, string extractPath);
        bool InsertProject(Project project);
        bool InsertCoAuthor(CoAuthor co);
        bool UpdateProject(Project project);
        CoAuthor GetAuthor(string email, string project);
        CoAuthor GetAuthor(string project);
        IEnumerable<T> Add<T>(IEnumerable<T> e, T value);
        /*
        IQueryable<Subject> GetAllSubjects();
        Subject GetSubject(int subjectId);

        IQueryable<Course> GetCoursesBySubject(int subjectId);

        IQueryable<Course> GetAllCourses();
        Course GetCourse(int courseId, bool includeEnrollments = true);
        bool CourseExists(int courseId);

        IQueryable<Student> GetAllStudentsWithEnrollments();
        IQueryable<Student> GetAllStudentsSummary();

        IQueryable<Student> GetEnrolledStudentsInCourse(int courseId);
        Student GetStudentEnrollments(string userName);
        Student GetStudent(string userName);

        Tutor GetTutor(int tutorId);

        bool LoginStudent(string userName, string password);

        bool Insert(Student student);
        bool Update(Student originalStudent, Student updatedStudent);
        bool DeleteStudent(int id);

        int EnrollStudentInCourse(int studentId, int courseId, Enrollment enrollment);

        bool Insert(Course course);
        bool Update(Course originalCourse, Course updatedCourse);
        bool DeleteCourse(int id);

        bool SaveAll();
        */
    }
}
