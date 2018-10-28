using System;
using System.Collections.Generic;

namespace Entity
{
    public class QuestionItem
    {
        private int m_id;
        private int m_sequence;
        private String m_question;  // 问题文本
        private List<AnswerItem> m_answers;
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
        public String Question
        {
            get { return m_question; }
            set { m_question = value; }
        }
        public List<AnswerItem> Answers
        {
            get { return m_answers; }
            set { m_answers = value; }
        }
        public bool Valid
        {
            get { return m_valid; }
            set { m_valid = value; }
        }
    }
}
