/*WITH Students AS (
SELECT ROW_NUMBER() OVER (ORDER BY rollNo) AS RowNumber, Name, BranchCode
FROM Students)
SELECT *
FROM Students
WHERE RowNumber = 2;*/


--INSERT INTO Teachers_Enrollment(empId, subjCode) VALUES('bd8a1085-bef9-40d7-9a61-b55b76190402', 'd85df616-7008-4422-9c82-d86785d54ce2');
--INSERT INTO Teachers_Enrollment(empId, subjCode) VALUES('0035d5e8-8f17-4f11-9bc6-c81470bded4b', '90830aa8-2dbd-45bc-9c73-b0ce3e15351e');

--SELECT * FROM Teachers_Enrollment;

/*SELECT Students_Enrollment.rollNo AS rollNo, Students_Enrollment.subjCode AS subjCode,
Subjects.name AS subjectName, Teachers.name AS teacherName FROM Subjects JOIN Students_Enrollment
ON Subjects.code = Students_Enrollment.subjCode JOIN Teachers_Enrollment ON Students_Enrollment.subjCode
= Teachers_Enrollment.subjCode JOIN Teachers ON Teachers_Enrollment.empId = Teachers.id WHERE 
rollNo = '9A220924-4BC1-4960-96F7-F134BD7FB7C6' AND Students_Enrollment.active_bit = 1 AND 
Teachers.active_bit = 1 AND Subjects.active_bit = 1 AND Teachers_Enrollment.active_bit = 1;*/

--SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY CAST(Students.name AS varchar)) AS regNo, Students.name, Students.branchCode FROM Students JOIN Branches ON branchCode = code WHERE Students.active_bit = 1 AND Branches.active_bit = 1) AS TEMP WHERE regNO = 2;

--(SELECT ROW_NUMBER() OVER (ORDER BY CAST(Students.name AS varchar)) AS regNo FROM Students JOIN Branches ON branchCode = code WHERE Students.active_bit = 1 AND Branches.active_bit = 1 AS TEMP);

