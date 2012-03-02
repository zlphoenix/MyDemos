using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.TRF.Persistant.CoreLib.Entity
{
    public class ConcreteEntity:EntityBase
    {

        protected override void SetDefaultValue()
        {
            throw new NotImplementedException();
        }

        protected override void OnValidate()
        {
            throw new NotImplementedException();
        }

        public override string EntityComponent
        {
            get { throw new NotImplementedException(); }
        }
    }
}
