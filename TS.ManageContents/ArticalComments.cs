using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.ManageContents
{
    public class ArticalComments
    {
        #region Columns  
        public long id { get; set; }
        public long topic_id { get; set; }
        public long? commentId { get; set; }
        public long userId { get; set; }
        public int Active { get; set; }
        public string Comments { get; set; }
        public string display_name { get; set; }
        public DateTime? created_date { get; set; }

        #endregion

        public ArticalComments(DataRow DR)
        {
            this.id = Convert.ToInt64(DR["id"]);
            this.topic_id = Convert.ToInt64(DR["topic_id"]);
            this.commentId = Convert.ToInt64(DR["commentId"]);
            this.userId = Convert.ToInt64(DR["userId"]);
            this.Active = Convert.ToInt32(DR["Active"]);
            this.Comments = Convert.ToString(DR["Comments"]);
            this.created_date = Convert.ToDateTime(DR["created_date"]);
            this.display_name= Convert.ToString(DR["display_name"]);
        }

        public ArticalComments() { }

        public ArticalComments(long id, long topic_id, long commentId, long userId, int Active, string Comments, DateTime created_date)
        {
            this.id = id;
            this.topic_id = topic_id;
            this.commentId = commentId;
            this.userId = userId;
            this.Active = Active;
            this.Comments = Comments;
            this.created_date = created_date;
        }

        public ArticalComments Create(ArticalComments ac, Connection con)
        {
             if(con==null)
                con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (ac.topic_id == 0)
            {
                throw new Exception("topic id is required");
            }
            else
            {
                query1 += "topic_id,";
                query2 += "@topic_id,";
                cmd.Parameters.Add("@topic_id", DBType.Int, ac.topic_id);
            }
            query1 += "commentId,";
            query2 += "@commentId,";
            cmd.Parameters.Add("@commentId", DBType.Int, ac.commentId);

            if (ac.userId == 0)
            {
                throw new Exception("userId is required");
            }
            else
            {
                query1 += "userId,";
                query2 += "@userId,";
                cmd.Parameters.Add("@userId", DBType.Int, ac.userId);
            }
            if (ac.Active == 0)
            {
                throw new Exception("Active is required");
            }
            else
            {
                query1 += "Active,";
                query2 += "@Active,";
                cmd.Parameters.Add("@Active", DBType.Int, ac.Active);
            }
            if (!string.IsNullOrEmpty(ac.Comments))
            {
                query1 += "Comments,";
                query2 += "@Comments,";
                cmd.Parameters.Add("@Comments", DBType.VarChar, ac.Comments);
            }

            // Write Here for missing column created_date
            query1 += "created_date,";
            query2 += "@created_date,";
            cmd.Parameters.Add("@created_date", DBType.DateTime, DateTime.Now);

            string Query = "INSERT INTO ArticalComments(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") ;" + (con.ServerType == DBServerType.MYSQL ? " SELECT last_insert_id(); " : "SELECT @@IDENTITY");
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            ac.id = Convert.ToInt32(DT.Rows[0][0]);
            return ac;
        }

        public List<ArticalComments> GetByTopicId(int topic_id, Connection con)
        {
            Command cmd = new Command();

            string Query = @"SELECT 
	ac.*, um.display_name
FROM 
	ArticalComments  ac
    INNER JOIN UserMaster um
		ON ac.UserId=um.uid
WHERE 
	topic_id=@topic_id 
order by id desc;";
            cmd.Parameters.Add("@topic_id", DBType.Int, topic_id);
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            if (DT.Rows.Count > 0)
            {
                List<ArticalComments> lst = new List<ArticalComments>();
                foreach (DataRow DR in DT.Rows)
                {
                    lst.Add(new ArticalComments(DR));
                }
                return lst;
            }
            else
                return null;
        }
    }

}
