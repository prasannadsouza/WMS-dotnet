namespace WMS.BusinessService
{
    public class BaseBusinessService
    {
        protected BaseBusinessService(Configuration configuration)
        {
            Configuration = configuration;
            _entitySortFieldMapper = GetEntitySortFieldMapper();
        }

        private Dictionary<string, string> _entitySortFieldMapper;
        private string _className;
        private string ClassName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_className) == false) return _className;
                _className = this.GetType().FullName;
                return _className;
            }
        }
        internal Configuration Configuration { get; set; }

        private Dictionary<string, string> GetEntitySortFieldMapper()
        {
            var sortFieldMapper = new Dictionary<string, string>();
            return sortFieldMapper;
        }
    }
}