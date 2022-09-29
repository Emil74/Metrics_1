namespace MetricsManager.Models
{
    public class AgentPool
    {
        private static AgentPool _instance;

        public static AgentPool Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AgentPool();
                return _instance;
            }
        }


        private Dictionary<int, AgentInfo> _values;

        public AgentPool()
        {
            _values = new Dictionary<int, AgentInfo>();
        }

        public void Add(AgentInfo value)
        {
            lock (_values)
            {
                if (!_values.ContainsKey(value.AgentId))
                    _values.Add(value.AgentId, value);
            }
        }

        public AgentInfo[] Get()
        {

            return _values.Values.ToArray();
        }

        public Dictionary<int, AgentInfo> Values
        {
            get { return _values; }
            set { _values = value; }
        }

    }
}
