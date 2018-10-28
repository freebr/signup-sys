using System;

namespace Entity
{
    public class SystemInfo
    {
        public enum SystemStatus
        {
            Closed,
            Open,
            Debugging   // 调试模式
        }
        private String m_page_notice;       // 通知页内容
        private SystemStatus m_status;      // 系统状态
        private bool m_valid = true;

        public String PageNotice
        {
            get { return m_page_notice; }
            set { m_page_notice = value; }
        }
        public SystemStatus Status
        {
            get { return m_status; }
            set { m_status = value; }
        }
        public bool Valid
        {
            get { return m_valid; }
            set { m_valid = value; }
        }
    }
}
