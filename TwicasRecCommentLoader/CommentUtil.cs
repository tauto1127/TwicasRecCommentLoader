using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TwicasRecCommentLoader
{
    public class TwicasComment
    {
        public TwicasComment(string comment, string id, int number, int time)
        {
            this.Comment = comment;
            this.Id = id;
            this.Number = number;
            this.Time = time;
        }
        public int Number { get; set; }
        public string Id { get; set; }
        public int Time { get; set; }
        public string Comment { get; set; }

        public override string ToString()
        {
            return $"{Number}   時間：{Time}　コメント：{Comment}　ID；{Id}";
        }
    }
    public class CommentUtil
    {

        /// <summary>
        /// コメントそのまま
        /// </summary>
        public static List<string> OldCommentList = new List<string>();
        /// <summary>
        /// 分割したコメント
        /// </summary>
        public List<TwicasComment> TwicasComments { get; set; } = new List<TwicasComment>();

        public CommentUtil(string path)
        {
            CommentLoader(path);
            CommentSplit();
        }

        private void CommentSplit()
        {
            Regex regex = new Regex(@"\[....\/..\/(?<date>..).(?<oclock>..)\:(?<minute>..)\:(?<secound>..)\].(?<comment>.+)（(?<id>.+)）");
            int i = 0;
            Match match;
            
            


            match = regex.Match(OldCommentList[0]);
            int Day = Int32.Parse(match.Result("${date}"));

            int time;
            foreach (string variable in OldCommentList)
            {
                
                if (regex.IsMatch(variable))
                {
                    match = regex.Match(variable);
                    time = Int32.Parse(match.Result("${oclock}")) * 360 + Int32.Parse(match.Result("${minute}")) * 60 +
                           Int32.Parse(match.Result("${secound}"));
                    if (Day != Int32.Parse(match.Result("${date}")))
                    {
                        time += 24 * 60 * 60;
                    }
                    TwicasComments.Add(new TwicasComment(match.Groups["comment"].ToString(), match.Groups["id"].ToString(), i, time));
                    i++;
                }
            }
        }

        public static void CommentLoader(string commentFilePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(commentFilePath))
                {
                    Regex regex = new Regex(@"\[....\/..\/.+\:..\:..\]");
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (regex.IsMatch(line))
                        {
                            OldCommentList.Add(line);
                            line = null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        
    }
}