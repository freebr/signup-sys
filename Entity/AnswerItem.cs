using System;

namespace Entity
{
    public class AnswerItem
    {
        private int m_id;
        private int m_sequence;
        private QuestionItem m_questions;   // 关联问题
        private String m_answer;            // 答案文本
        private bool m_valid = true;

        public int ID
        {
            get { return m_id; }
            set { m_id = value; }
        }
        public int Sequence
        {
            get { return m_sequence; }
            set { m_sequence = value; }
        }
        public QuestionItem Question
        {
            get { return m_questions; }
            set { m_questions = value; }
        }
        public String Answer
        {
            get { return m_answer; }
            set { m_answer = value; }
        }
        public bool Valid
        {
            get { return m_valid; }
            set { m_valid = value; }
        }
    }
}
