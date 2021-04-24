using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.ManageContents
{
	public class TestList
	{
		#region Columns  
		public long Tlid { get; set; }
		public long Uid { get; set; }
		public int QuestionGroup { get; set; }
		public DateTime? TestDate { get; set; }
		public int? TimeTaken { get; set; }
		public int? TotalQuestion { get; set; }
		public int? Attempt { get; set; }
		public int? Currect { get; set; }
		public DateTime CreatedDate { get; set; }
		public long? CreatedBy { get; set; }
		#endregion

		public TestList(DataRow DR)
		{
			this.Tlid = Convert.ToInt64(DR["Tlid"]);
			this.Uid = Convert.ToInt64(DR["Uid"]);
			this.QuestionGroup = Convert.ToInt32(DR["QuestionGroup"]);
			this.TestDate = Convert.ToDateTime(DR["TestDate"]);
			this.TimeTaken = Convert.ToInt32(DR["TimeTaken"]);
			this.TotalQuestion = Convert.ToInt32(DR["TotalQuestion"]);
			this.Attempt = Convert.ToInt32(DR["Attempt"]);
			this.Currect = Convert.ToInt32(DR["Currect"]);
			this.CreatedDate = Convert.ToDateTime(DR["CreatedDate"]);
			this.CreatedBy = Convert.ToInt64(DR["CreatedBy"]);
		}

		public TestList() { }

		public TestList(long Tlid, long Uid, int QuestionGroup, DateTime TestDate, int TimeTaken, int TotalQuestion, int Attempt, int Currect, DateTime CreatedDate, long CreatedBy)
		{
			this.Tlid = Tlid;
			this.Uid = Uid;
			this.QuestionGroup = QuestionGroup;
			this.TestDate = TestDate;
			this.TimeTaken = TimeTaken;
			this.TotalQuestion = TotalQuestion;
			this.Attempt = Attempt;
			this.Currect = Currect;
			this.CreatedDate = CreatedDate;
			this.CreatedBy = CreatedBy;
		}

		public TestList Create(TestList tl, Connection con)
		{
			if (con == null)
				con = new Connection();
			Command cmd = new Command();
			string query1 = "";
			string query2 = "";

			if (tl.Uid == 0)
			{
				throw new Exception("Uid is required");
			}
			else
			{
				query1 += "Uid,";
				query2 += "@Uid,";
				cmd.Parameters.Add("@Uid", DBType.Int, tl.Uid);
			}
			if (tl.QuestionGroup == 0)
			{
				throw new Exception("QuestionGroup is required");
			}
			else
			{
				query1 += "QuestionGroup,";
				query2 += "@QuestionGroup,";
				cmd.Parameters.Add("@QuestionGroup", DBType.Int, tl.QuestionGroup);
			}

			query1 += "TestDate,";
			query2 += "@TestDate,";
			cmd.Parameters.Add("@TestDate", DBType.DateTime, tl.TestDate);


			query1 += "TimeTaken,";
			query2 += "@TimeTaken,";
			cmd.Parameters.Add("@TimeTaken", DBType.Int, tl.TimeTaken);

			query1 += "TotalQuestion,";
			query2 += "@TotalQuestion,";
			cmd.Parameters.Add("@TotalQuestion", DBType.Int, tl.TotalQuestion);

			query1 += "Attempt,";
			query2 += "@Attempt,";
			cmd.Parameters.Add("@Attempt", DBType.Int, tl.Attempt);

			query1 += "Currect,";
			query2 += "@Currect,";
			cmd.Parameters.Add("@Currect", DBType.Int, tl.Currect);

			if (tl.CreatedDate == Convert.ToDateTime("1900-01-01"))
			{
				throw new Exception("CreatedDate is required");
			}
			else
			{
				query1 += "CreatedDate,";
				query2 += "@CreatedDate,";
				cmd.Parameters.Add("@CreatedDate", DBType.DateTime, tl.CreatedDate);
			}
			query1 += "CreatedBy,";
			query2 += "@CreatedBy,";
			cmd.Parameters.Add("@CreatedBy", DBType.Int, tl.CreatedBy);


			string Query = "INSERT INTO TestList(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") ;" + (con.ServerType == DBServerType.MYSQL ? " SELECT last_insert_id(); " : "SELECT @@IDENTITY");
			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			DataTable DT = con.getDataTable();
			tl.Tlid = Convert.ToInt32(DT.Rows[0][0]);
			return tl;
		}


		public TestList GetById(long Tlid, Connection con)
		{
			if (con == null)
				con = new Connection();
			Command cmd = new Command();
			string Query = @"SELECT * FROM TestList WHERE Tlid=@Tlid;";
			cmd.Parameters.Add("@Tlid", DBType.Int, Tlid);
			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			DataTable DT = con.getDataTable();
			if (DT.Rows.Count > 0)
				return new TestList(DT.Rows[0]);
			else
				return null;
		}

		public List<TestList> GetByUser(long uid, Connection con)
		{
			if (con == null)
				con = new Connection();
			Command cmd = new Command();
			string Query = @"SELECT * FROM TestList WHERE Uid=@Uid;";
			cmd.Parameters.Add("@Uid", DBType.Int, Uid);
			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			DataSet DS = con.getDataSet();

			List<TestList> obj = new List<TestList>();


			foreach (DataRow DR in DS.Tables[0].Rows)
			{
				obj.Add(new TestList(DR));
			}

			return obj;
		}

		public DataSet GetUserTestList(long uid, Connection con)
		{

			if (con == null)
				con = new Connection();

			string div = (con.ServerType == DBServerType.MSSQL ? @"/" : " div ");
Command cmd = new Command();
			string Query = @"SELECT 
	tl.Tlid,Qg.QuestionGroupName,tl.QuestionGroup,tl.TestDate,CONCAT((TimeTaken"+ div + @"60),'M ',(TimeTaken%60),' S')  TimeTaken,TotalQuestion,Currect, CAST(((CAST(Currect AS decimal(18,2))/CAST(TotalQuestion  AS decimal(18,2)))*100.00)  AS decimal(18,2)) Percentage
FROM 
	TestList tl
	LEFT OUTER JOIN Question_Group Qg
		ON tl.QuestionGroup=Qg.QGid
WHERE
	tl.Uid=@Uid;";
			cmd.Parameters.Add("@Uid", DBType.Int, uid);
			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			return con.getDataSet();
		}


	}

}
