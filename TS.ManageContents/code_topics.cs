using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.ManageContents
{
    public class code_topics
    {
        #region Columns  
        public int topic_id { get; set; }
        public int userId { get; set; }
        public string topic_subject { get; set; }
        public string search_tags { get; set; }
        public string code_topicscol { get; set; }
        public int? Active { get; set; }
        public int? Allow_Comment { get; set; }
        public DateTime? created_date { get; set; }
        public int? visible { get; set; }
        public string topic_details { get; set; }
        public string fileName { get; set; }
        public string ArticalBody { get; set; }
        public string Language { get; set; }
        public string URL { get; set; }
        public string Comment { get; set; }
        public List<ArticalComments> Comments { get; set; }


        #endregion

        public code_topics(DataRow DR)
        {
            this.topic_id = Convert.ToInt32(DR["topic_id"]);
            this.userId = Convert.ToInt32(DR["userId"]);
            this.topic_subject = Convert.ToString(DR["topic_subject"]);
            this.search_tags = Convert.ToString(DR["search_tags"]);
            this.code_topicscol = Convert.ToString(DR["code_topicscol"]);
            this.Active = Convert.ToInt32(DR["Active"]);
            this.Allow_Comment = Convert.ToInt32(DR["Allow_Comment"]);
            //this.created_date = Convert.ToDateTime(DR["created_date"]);
            this.visible = Convert.ToInt32(DR["visible"]);
            this.topic_details = Convert.ToString(DR["topic_details"]);
            this.fileName = Convert.ToString(DR["fileName"]);
            this.Language = Convert.ToString(DR["Language"]);
            this.URL = Convert.ToString(DR["URL"]);
        }

        public code_topics() { }
        public code_topics(int topic_id, int userId, string topic_subject, string search_tags, string code_topicscol, int Active, int Allow_Comment, DateTime created_date, int visible, string topic_details, string fileName, string Language, string URL)
        {
            this.topic_id = topic_id;
            this.userId = userId;
            this.topic_subject = topic_subject;
            this.search_tags = search_tags;
            this.code_topicscol = code_topicscol;
            this.Active = Active;
            this.Allow_Comment = Allow_Comment;
            this.created_date = created_date;
            this.visible = visible;
            this.topic_details = topic_details;
            this.fileName = fileName;
            this.Language = Language;
            this.URL = URL;
        }

        public code_topics Create(code_topics ct, Connection con)
        {
            //Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (ct.userId == 0)
            {
                throw new Exception("userId is required");
            }
            else
            {
                query1 += "userId,";
                query2 += "@userId,";
                cmd.Parameters.Add("@userId", DBType.Int, ct.userId);
            }
            if (!string.IsNullOrEmpty(ct.topic_subject))
            {
                query1 += "topic_subject,";
                query2 += "@topic_subject,";
                cmd.Parameters.Add("@topic_subject", DBType.VarChar, ct.topic_subject);
            }

            if (!string.IsNullOrEmpty(ct.search_tags))
            {
                query1 += "search_tags,";
                query2 += "@search_tags,";
                cmd.Parameters.Add("@search_tags", DBType.VarChar, ct.search_tags);
            }

            if (!string.IsNullOrEmpty(ct.code_topicscol))
            {
                query1 += "code_topicscol,";
                query2 += "@code_topicscol,";
                cmd.Parameters.Add("@code_topicscol", DBType.VarChar, ct.code_topicscol);
            }

            query1 += "Active,";
            query2 += "@Active,";
            cmd.Parameters.Add("@Active", DBType.Int, ct.Active);

            query1 += "Allow_Comment,";
            query2 += "@Allow_Comment,";
            cmd.Parameters.Add("@Allow_Comment", DBType.Int, ct.Allow_Comment);

            // Write Here for missing column created_date
            query1 += "created_date,";
            query2 += "@created_date,";
            cmd.Parameters.Add("@created_date", DBType.DateTime, ct.created_date);

            query1 += "visible,";
            query2 += "@visible,";
            cmd.Parameters.Add("@visible", DBType.Int, ct.visible);

            if (!string.IsNullOrEmpty(ct.topic_details))
            {
                query1 += "topic_details,";
                query2 += "@topic_details,";
                cmd.Parameters.Add("@topic_details", DBType.VarChar, ct.topic_details);
            }

            if (!string.IsNullOrEmpty(ct.fileName))
            {
                query1 += "fileName,";
                query2 += "@fileName,";
                cmd.Parameters.Add("@fileName", DBType.VarChar, ct.fileName);
            }

            if (!string.IsNullOrEmpty(ct.Language))
            {
                query1 += "Language,";
                query2 += "@Language,";
                cmd.Parameters.Add("@Language", DBType.VarChar, ct.Language);
            }

            if (!string.IsNullOrEmpty(ct.URL))
            {
                query1 += "URL,";
                query2 += "@URL,";
                cmd.Parameters.Add("@URL", DBType.VarChar, ct.URL);
            }


            string Query = "INSERT INTO code_topics(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") ;" + (con.ServerType == DBServerType.MYSQL ? " SELECT last_insert_id(); " : "SELECT @@IDENTITY");



            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            ct.topic_id = Convert.ToInt32(DT.Rows[0][0]);
            return ct;
        }

        public code_topics Update(code_topics ct, Connection con)
        {
            code_topics old = GetByid(con, ct.topic_id);

            //Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            
            if (!ct.topic_subject.Equals(old.topic_details))
            {
                query1 += "topic_subject=@topic_subject,";
                cmd.Parameters.Add("@topic_subject", DBType.VarChar, ct.topic_subject);
            }

            if (!ct.search_tags.Equals(old.search_tags))
            {
                query1 += "search_tags=@search_tags,";
                cmd.Parameters.Add("@search_tags", DBType.VarChar, ct.search_tags);
            }

            if (!ct.code_topicscol.Equals(old.code_topicscol))
            {
                query1 += "code_topicscol=@code_topicscol,";
                cmd.Parameters.Add("@code_topicscol", DBType.VarChar, ct.code_topicscol);
            }

            if (!ct.topic_details.Equals(old.topic_details))
            {
                query1 += "topic_details=@topic_details,";
                cmd.Parameters.Add("@topic_details", DBType.VarChar, ct.topic_details);
            }

            if (!ct.fileName.Equals(old.fileName))
            {
                query1 += "fileName=@fileName,";
                cmd.Parameters.Add("@fileName", DBType.VarChar, ct.fileName);
            }

            if (!ct.Language.Equals(old.Language))
            {
                query1 += "Language=@Language,";
                cmd.Parameters.Add("@Language", DBType.VarChar, ct.Language);
            }

            if (!ct.URL.Equals(old.URL))
            {
                query1 += "URL=@URL,";
                cmd.Parameters.Add("@URL", DBType.VarChar, ct.URL);
            }

            if (query1.Length > 2)
            {
                string Query = "UPDATE code_topics SET " + query1.TrimEnd(',') + " WHERE topic_id=" + ct.topic_id;



                cmd.CommandText = Query;
                cmd.CommandType = DBConnection.CommandType.Text;
                con.cmd = cmd;

                DataTable DT = con.getDataTable();
                ct.topic_id = Convert.ToInt32(DT.Rows[0][0]);
            }

            return ct;
        }


        public static code_topics GetByid(Connection con, int topic_id)
        {
            //Connection con = new Connection();
            Command cmd = new Command();

            string Query = "SELECT * FROM code_topics WHERE topic_id=@topic_id";
            cmd.Parameters.Add("@topic_id", DBType.Int, topic_id);
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            //List<code_topics> obj = new List<code_topics>();

            //foreach (DataRow DR in DT.Rows)
            //{
            //    obj.Add(new code_topics(DR));
            //}
            if (DT.Rows.Count > 0)
                return new code_topics(DT.Rows[0]);
            else
                return null;

        }

        public static List<code_topics> Search(Connection con, string SearchString, string Language = "")
        {
            //Connection con = new Connection();
            Command cmd = new Command();

            string Query = "SELECT "+(con.ServerType== DBServerType.MSSQL ? " TOP 100 ":"")+" * FROM code_topics WHERE search_tags LIKE CONCAT('%',@search_tags,'%') " + (string.IsNullOrEmpty(Language) ? "" : " and Language=@v_Language") + " and visible=1 and Active=1 ORDER BY topic_id desc " + (con.ServerType == DBServerType.MSSQL ? "" : "LIMIT 100");
            cmd.Parameters.Add("@search_tags", DBType.VarChar, string.IsNullOrEmpty(SearchString) ? "" : SearchString);
            if (!string.IsNullOrEmpty(Language))
                cmd.Parameters.Add("@v_Language", DBType.VarChar, Language);
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            List<code_topics> obj = new List<code_topics>();

            foreach (DataRow DR in DT.Rows)
            {
                obj.Add(new code_topics(DR));
            }

            return obj;

        }


        public static List<code_topics> SearchByUser(Connection con, int UserId, string Language = "")
        {
            //Connection con = new Connection();
            Command cmd = new Command();

            string Query = "SELECT " + (con.ServerType == DBServerType.MSSQL ? " TOP 100 " : "") + " * FROM code_topics WHERE userId=@userId " + (string.IsNullOrEmpty(Language) ? "" : " and Language=@v_Language") + " and visible=1 and Active=1 ORDER BY topic_id desc " + (con.ServerType == DBServerType.MSSQL ? "" : "LIMIT 100");
            cmd.Parameters.Add("@userId", DBType.Int, UserId);
            if (!string.IsNullOrEmpty(Language))
                cmd.Parameters.Add("@v_Language", DBType.VarChar, Language);
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            List<code_topics> obj = new List<code_topics>();

            foreach (DataRow DR in DT.Rows)
            {
                obj.Add(new code_topics(DR));
            }

            return obj;

        }


    }
}
