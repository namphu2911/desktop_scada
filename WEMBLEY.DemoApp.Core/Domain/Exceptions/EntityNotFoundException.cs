using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string EntityType { get; private set; }
        public string EntityId { get; private set; }

        public EntityNotFoundException(string entityType, string entityId, string message) : base(message)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}
