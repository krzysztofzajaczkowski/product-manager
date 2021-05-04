using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Core.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        protected Entity(Guid id)
        {
            Id = id;
        }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
