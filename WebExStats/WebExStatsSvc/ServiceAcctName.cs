namespace WebExStatsSvc
{
    public static class ServiceAcctName
    {
        public static string GetServiceAccountName()
        {
            var returnName = string.Empty;
            const string wmiQuery = "select startname from Win32_Service where name = 'DataSvc'";

            var sQuery = new System.Management.SelectQuery(wmiQuery);
            using (var mgmtSearcher = new System.Management.ManagementObjectSearcher(sQuery))
            {
                foreach (var service in mgmtSearcher.Get())
                {
                    returnName = service["startname"].ToString().Contains("\\") ? service["startname"].ToString().Split(char.Parse("\\"))[1] : service["startname"].ToString();

                    break;
                }
            }

            return returnName;
        }

    }
}
