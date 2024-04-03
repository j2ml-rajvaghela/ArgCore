using Arg.DataModels;
using Dapper;

namespace Arg.DataAccess
{
    public class ContainerEventTypesImpl
    {
        public List<ContainerEventType> GetEventTypes(bool addSelectAllOption = false)
        {
            const string query = @"SELECT DISTINCT EventType,EventDescription FROM ContainerEventTypes
                                   WHERE EventType <> ''
                                   ORDER BY EventDescription;";

            using (var connection = Common.ClientDatabase)
            {
                var eventTypes = connection.Query<ContainerEventType>(query).ToList();
                if (addSelectAllOption)
                {
                    eventTypes.Add(new ContainerEventType { EventDescription = "All Events", EventType = "All Events" });
                }
                return eventTypes;
            }
        }
    }
}
