using System;

namespace DbExec
{
    public enum OperatorTypeEnum
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanEqual,
        LessThanEqual,
        Belong,
        NotBelong,
        Contain,
        NotContain
    }
    public class Operator
    {
        private static String[] m_operatorName = { "等于", "不等于", "大于", "小于", "大于等于", "小于等于", "属于", "不属于", "包含", "不包含" };
        private static String[] m_operatorSym = { "=", "<>", ">", "<", ">=", "<=", "IN", "NOT IN", "LIKE", "NOT LIKE" };
        public OperatorTypeEnum OperatorType;
        public Operator(OperatorTypeEnum opt)
        {
            OperatorType = opt;
        }
        public static implicit operator String (Operator op) {
            return op.getOperator();
        }
        String getOperatorName() { return m_operatorName[(int)OperatorType]; }
        String getOperator() { return m_operatorSym[(int)OperatorType]; }
    }
}
