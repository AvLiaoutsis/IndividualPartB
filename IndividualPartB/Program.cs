using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualPartB
{
    class Program
    {
        //Last 3 methods to look at
        static string conS = @"Server = DESKTOP-M87V4V7\SQLEXPRESS01; Database = Institution; Trusted_Connection = True";
        static SqlConnection connection = new SqlConnection(conS); //Initializing connection object
        static void Main(string[] args)
        {

            try
            {
                using (connection)
                {   // Establishing connection
                    connection.Open();
                    Console.WriteLine($"Connection State: {connection.State}");//Reporting state of connection

                    //STUDENTS
                    SqlCommand cmdSelectStudents = new SqlCommand("SELECT * FROM STUDENT", connection); //Command object creation containing query info for db
                    //cmdSelectStudents.ExecuteNonQuery() would directly execute this query on db
                    SqlDataReader reader = cmdSelectStudents.ExecuteReader(); //reader allows us to handle the data
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}"); //Reporting state of reader
                    var students = new List<object>();  //Creating a list tha contains objects
                    while (reader.Read())
                    {
                        var newStudent = new //Converting records from db to anonymous type objects
                        {
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Birthday = reader.GetDateTime(3),
                            TuitionFees = reader.GetInt32(4)
                        };
                        students.Add(newStudent); //Populating list 
                    }
                    reader.Close();
                    foreach (var student in students)
                    {
                        Console.WriteLine(student);//Printing
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}"); //Reporting state of reader

                    //TRAINERS
                    SqlCommand cmdSelectTrainers = new SqlCommand("SELECT * FROM TRAINER", connection);
                    reader = cmdSelectTrainers.ExecuteReader();
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");
                    var trainers = new List<object>();
                    while (reader.Read())
                    {
                        var newTrainer = new
                        {
                            FirstName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            Subject = reader.GetString(2)
                        };
                        trainers.Add(newTrainer);
                    }
                    reader.Close();
                    foreach (var trainer in trainers)
                    {
                        Console.WriteLine(trainer);
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");

                    //COURSES
                    SqlCommand cmdSelectCourses = new SqlCommand("SELECT * FROM COURSE", connection);
                    reader = cmdSelectCourses.ExecuteReader();
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");
                    var courses = new List<object>();
                    while (reader.Read())
                    {
                        var newCourse = new
                        {
                            Title = reader.GetString(1),
                            Stream = reader.GetString(2),
                            Type = reader.GetString(3),
                            StartDate = reader.GetDateTime(4),
                            EndDate = reader.GetDateTime(5)
                        };
                        courses.Add(newCourse);
                    }
                    reader.Close();
                    foreach (var course in courses)
                    {
                        Console.WriteLine(course);
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");

                    //ASSIGNMENTS
                    SqlCommand cmdSelectAssignments = new SqlCommand("SELECT * FROM ASSIGNMENT", connection);
                    reader = cmdSelectAssignments.ExecuteReader();
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");
                    var assisgnments = new List<object>();
                    while (reader.Read())
                    {
                        var newAssignment = new
                        {
                            Title = reader.GetString(0),
                            Description = reader.GetString(1),
                            SubmissionDate = reader.GetDateTime(2)
                        };
                        assisgnments.Add(newAssignment);
                    }
                    reader.Close();
                    foreach (var assignment in assisgnments)
                    {
                        Console.WriteLine(assignment);
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");


                    //STUDENTS PER COURSE
                    //Building the query with string builder
                    var sb = new StringBuilder();
                    sb
                        .AppendLine("SELECT student.FirstName,Student.LastName,Student.Birthday,Student.TuitionFees,Course.Title as CourseTitle,Course.Stream")
                        .AppendLine("FROM Course,Student,StudentEnrolled")
                        .AppendLine("where Course.ID = StudentEnrolled.CourseID and StudentEnrolled.StudentID = Student.ID");

                    SqlCommand cmdSelectStudentsPerCourse = new SqlCommand(sb.ToString(), connection);
                    reader = cmdSelectStudentsPerCourse.ExecuteReader();
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");
                    var studentsPerCourse = new List<object>();
                    while (reader.Read())
                    {
                        var newData = new
                        {
                            FirstName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            Birthday = reader.GetDateTime(2),
                            TuitionFees = reader.GetInt32(3),
                            CourseTitle = reader.GetString(4),
                            Stream = reader.GetString(5)
                        };
                        studentsPerCourse.Add(newData);
                    }
                    reader.Close();
                    foreach (var data in studentsPerCourse)
                    {
                        Console.WriteLine(data);
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");

                    //TRAINERS PER COURSE
                    //Building the query with string builder
                    sb = new StringBuilder();
                    sb
                        .AppendLine("SELECT Trainer.FirstName, Trainer.LastName, Trainer.Subject, Course.Title as CourseTitle,Course.Stream ")
                        .AppendLine("FROM Trainer,TrainerEnrolled,Course")
                        .AppendLine("WHERE Trainer.ID = TrainerEnrolled.TrainerID and TrainerEnrolled.CourseID = Course.ID ");

                    SqlCommand cmdSelectTrainersPerCourse = new SqlCommand(sb.ToString(), connection);
                    reader = cmdSelectTrainersPerCourse.ExecuteReader();
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");
                    var trainersPerCourse = new List<object>();
                    while (reader.Read())
                    {
                        var newData = new
                        {
                            FirstName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            Subject = reader.GetString(2),
                            CourseTitle = reader.GetString(3),
                            Stream = reader.GetString(4)
                        };
                        trainersPerCourse.Add(newData);
                    }
                    reader.Close();
                    foreach (var data in trainersPerCourse)
                    {
                        Console.WriteLine(data);
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");

                    //ASSIGNMENTS PER COURSE
                    //Building the query with string builder
                    sb = new StringBuilder();
                    sb
                        .AppendLine("SELECT Course.Title AS CourseTitle, Course.Stream,Assignment.Title AS AssignmentTitle,Assignment.Description,AssignmentRelation.OralGrade,AssignmentRelation.TotalGrade,Assignment.SubmissionDate")
                        .AppendLine("FROM Course,Assignment,AssignmentRelation")
                        .AppendLine("WHERE Course.ID = AssignmentRelation.CourseID")
                        .AppendLine("AND Assignment.ID = AssignmentRelation.AssignmentID");

                    SqlCommand cmdSelectAssignmentsPerCourse = new SqlCommand(sb.ToString(), connection);
                    reader = cmdSelectAssignmentsPerCourse.ExecuteReader();
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");
                    var assignmentsPerCourse = new List<object>();
                    while (reader.Read())
                    {
                        var newData = new
                        {
                            CourseTitle = reader.GetString(0),
                            Stream = reader.GetString(1),
                            AssignmentTitle = reader.GetString(2),
                            Description = reader.GetString(3),
                            OralGrade = reader.GetInt32(4),
                            TotalGrade = reader.GetInt32(5),
                            SubmissionDate = reader.GetDateTime(6)
                        };
                        assignmentsPerCourse.Add(newData);
                    }
                    reader.Close();
                    foreach (var data in assignmentsPerCourse)
                    {
                        Console.WriteLine(data);
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");

                    //ASSIGNMENTS PER COURSE PER STUDENT
                    //Building the query with string builder
                    sb = new StringBuilder();
                    sb
                        .AppendLine("SELECT Course.Title,Course.Stream,Student.FirstName,Student.LastName,Assignment.Title,Assignment.Description,AssignmentRelation.OralGrade,AssignmentRelation.TotalGrade,Assignment.SubmissionDate")
                        .AppendLine("FROM Assignment,AssignmentRelation,Course,Student")
                        .AppendLine("where Assignment.ID = AssignmentRelation.AssignmentID")
                        .AppendLine("AND AssignmentRelation.CourseID = Course.ID")
                        .AppendLine("AND AssignmentRelation.StudentID = Student.ID");

                    SqlCommand cmdSelectASSIGNMENTSperCOURSEperSTUDENT = new SqlCommand(sb.ToString(), connection);
                    reader = cmdSelectASSIGNMENTSperCOURSEperSTUDENT.ExecuteReader();
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");
                    var assignmentsPerCperS = new List<object>();
                    while (reader.Read())
                    {
                        var newData = new
                        {
                            CourseTitle = reader.GetString(0),
                            Stream = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            AssignmentTitle = reader.GetString(4),
                            Description = reader.GetString(5),
                            OralGrade = reader.GetInt32(6),
                            TotalGrade = reader.GetInt32(7),
                            SubmissionDate = reader.GetDateTime(8)
                        };
                        assignmentsPerCperS.Add(newData);
                    }
                    reader.Close();
                    foreach (var data in assignmentsPerCperS)
                    {
                        Console.WriteLine(data);
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");

                    //STUDENTS WITH MORE THAN ONE ASSIGNMENT
                    //Building the query with string builder
                    sb = new StringBuilder();
                    sb
                        .AppendLine("SELECT Student.FirstName,")
                        .AppendLine("Student.LastName,")
                        .AppendLine("COUNT(course.id) As Number_Of_Courses")
                        .AppendLine("FROM Student")
                        .AppendLine("INNER JOIN StudentEnrolled")
                        .AppendLine("ON Student.ID = StudentEnrolled.StudentID")
                        .AppendLine("INNER JOIN Course")
                        .AppendLine("ON StudentEnrolled.CourseID = Course.ID")
                        .AppendLine("GROUP BY Student.FirstName,Student.LastName")
                        .AppendLine("HAVING COUNT(Course.ID)>1");

                    SqlCommand cmdSelectStudentsWithMoreThanTwoCourses = new SqlCommand(sb.ToString(), connection);
                    reader = cmdSelectStudentsWithMoreThanTwoCourses.ExecuteReader();
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");
                    var studentsWithCourses = new List<object>();
                    while (reader.Read())
                    {
                        var newData = new
                        {
                            FirstName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            CoursesNumber = reader.GetInt32(2)
                        };
                        studentsWithCourses.Add(newData);
                    }
                    reader.Close();
                    foreach (var data in studentsWithCourses)
                    {
                        Console.WriteLine(data);
                    }
                    Console.WriteLine($"Reader Open : {!reader.IsClosed}");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            connection.Close();




            Console.WriteLine($"Connection State: {connection.State}");
            Console.WriteLine("Press any key to continue with data insert");
            Console.ReadKey(true);
            Console.Clear();

            try
            {
                using (connection)
                {
                    connection.Open();
                    Console.WriteLine($"Connection State: {connection.State}");//Reporting state of connection

                    StudentInsesrt();

                    TrainerInsesrt();

                    CourseInsesrt();

                    AssignmentInsesrt();

                    StudentPerCourseInsert();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }


        private static void StudentInsesrt()
        {
            //Requesting input from user
            Console.WriteLine("Please type the first name of the student");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please type the last name of the student");
            string lastName = Console.ReadLine();
            Console.WriteLine("Please type the date of birth of the student");
            DateTime birthday = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Please type the tuition costs of the student");
            int tuitionFees = int.Parse(Console.ReadLine());

            //Building query command and passing values above into parameteres of the query
            SqlCommand insertCommand = new SqlCommand($"INSERT INTO STUDENT VALUES('{firstName}','{lastName}','{birthday}',{tuitionFees})", connection);
            //insertCommand.Parameters.AddWithValue("@firstName", firstName);
            //insertCommand.Parameters.AddWithValue("@lastName", lastName);
            //insertCommand.Parameters.AddWithValue("@birthday", birthday);
            //insertCommand.Parameters.AddWithValue("@tuitionFees", tuitionFees);
            //Executing query and storing rows affected
            int result = insertCommand.ExecuteNonQuery();

            Console.WriteLine("Rows affected : {0}", result);
        }
        private static void TrainerInsesrt()
        {
            //Requesting input from user
            Console.WriteLine("Please type the first name of the Trainer");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please type the last name of the Trainer");
            string lastName = Console.ReadLine();
            Console.WriteLine("Please type the subject of the Trainer");
            string subject = Console.ReadLine();

            //Building query command and passing values above into parameteres of the query
            SqlCommand insertCommand = new SqlCommand($"INSERT INTO TRAINER VALUES('{firstName}','{lastName}','{subject}')", connection);
            int result = insertCommand.ExecuteNonQuery();

            Console.WriteLine("Rows affected : {0}", result);
        }


        private static void CourseInsesrt()
        {
            //Requesting input from user
            Console.WriteLine("Please type the Title of the Course");
            string title = Console.ReadLine();
            Console.WriteLine("Please type the stream of the Course");
            string stream = Console.ReadLine();
            Console.WriteLine("Please type the type of the Course");
            string type = Console.ReadLine();
            Console.WriteLine("Please type the StartDate of the Course");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Please type the EndDate of the Course");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            //Building query command and passing values above into parameteres of the query
            SqlCommand insertCommand = new SqlCommand($"INSERT INTO COURSE VALUES('{title}','{stream}','{type}','{startDate}','{endDate}')", connection);
            int result = insertCommand.ExecuteNonQuery();

            Console.WriteLine("Rows affected : {0}", result);
        }

        private static void AssignmentInsesrt()
        {
            //Requesting input from user
            Console.WriteLine("Please type the Title of the Assignnment");
            string title = Console.ReadLine();
            Console.WriteLine("Please type the description of the Assignment");
            string description = Console.ReadLine();
            Console.WriteLine("Please type the submission date of the Assignment");
            DateTime submissionDate = DateTime.Parse(Console.ReadLine());

            //Building query command and passing values above into parameteres of the query
            SqlCommand insertCommand = new SqlCommand($"INSERT INTO COURSE VALUES('{title}','{description}','{submissionDate}')", connection);
            int result = insertCommand.ExecuteNonQuery();

            Console.WriteLine("Rows affected : {0}", result);
        }

        private static void StudentPerCourseInsert()
        {
            //Requesting input from user
            Console.WriteLine("Please type the firstName of the student you want to assign a lesson to");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please type the lastName of the student you want to assign a lesson to");
            string lastName = Console.ReadLine();
            Console.WriteLine("Please type the Title of the course you want to assign the student at");
            string courseTitle = Console.ReadLine();
            Console.WriteLine("Please type the Stream of the course you want to assign the student at");
            string courseStream = Console.ReadLine();

            //Building query to run
            var sb = new StringBuilder();
            sb
                .AppendLine("Insert into StudentEnrolled(StudentID,CourseID)")
                .AppendLine($"Values((Select Student.ID from Student where FirstName = '{firstName}' and LastName = '{lastName}'),")
                .AppendLine($"(select Course.ID from Course where Course.Title = '{courseTitle}' and Course.Stream = '{courseStream}'))");

            //Building query command and passing values above into parameteres of the query
            SqlCommand insertCommand = new SqlCommand(sb.ToString(), connection);
            int result = insertCommand.ExecuteNonQuery();

            Console.WriteLine("Rows affected : {0}", result);
        }
        private static void TrainerPerCourseInsert()
        {
            //Requesting input from user
            Console.WriteLine("Please type the firstName of the trainer");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please type the lastName of the trainer");
            string lastName = Console.ReadLine();
            Console.WriteLine("Please type the Title of the course you want to assign the trainer at");
            string courseTitle = Console.ReadLine();
            Console.WriteLine("Please type the Stream of the course you want to assign the trainer at");
            string courseStream = Console.ReadLine();

            //Building query to run
            var sb = new StringBuilder();
            sb
                .AppendLine("Insert into TrainerEnrolled(TrainerID,CourseID)")
                .AppendLine($"Values((Select Trainer.ID from Trainer where FirstName = '{firstName}' and LastName = '{lastName}'),")
                .AppendLine($"(select Course.ID from Course where Course.Title = '{courseTitle}' and Course.Stream = '{courseStream}'))");

            //Building query command and passing values above into parameteres of the query
            SqlCommand insertCommand = new SqlCommand(sb.ToString(), connection);
            int result = insertCommand.ExecuteNonQuery();

            Console.WriteLine("Rows affected : {0}", result);
        }

        private static void AssignmentPerStudentPerCourseInsert()
        {
            //Requesting input from user
            Console.WriteLine("Please type the Title of the Assignnment");
            string title = Console.ReadLine();
            Console.WriteLine("Please type the description of the Assignment");
            string description = Console.ReadLine();
            Console.WriteLine("Please type the submission date of the Assignment");
            DateTime submissionDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Please type the first name of the student you want to assign the assignment to");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please type the last name of the student you want to assign the assignment to");
            string lastName = Console.ReadLine();
            Console.WriteLine("Please type the Title of the course to include this assignment");
            string courseTitle = Console.ReadLine();
            Console.WriteLine("Please type the Stream of the course to include this assignment");
            string courseStream = Console.ReadLine();

            //Building query to run
            var sb = new StringBuilder();
            sb
                .AppendLine("Insert into TrainerEnrolled(TrainerID,CourseID)")
                .AppendLine($"Values((Select Trainer.ID from Trainer where FirstName = '{firstName}' and LastName = '{lastName}'),")
                .AppendLine($"(select Course.ID from Course where Course.Title = '{courseTitle}' and Course.Stream = '{courseStream}'))");

            //Building query command and passing values above into parameteres of the query
            SqlCommand insertCommand = new SqlCommand(sb.ToString(), connection);
            int result = insertCommand.ExecuteNonQuery();

            Console.WriteLine("Rows affected : {0}", result);
        }


    }
}
