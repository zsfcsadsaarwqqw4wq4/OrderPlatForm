using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ExModel
    {
        public class ClassiFicationModel
        {
            public ClassiFication ClassiFication { get; set; }
            public List<ClassiFication> childClassiFication { get; set; }
        }

        public class EvaluateModel
        {
            public EvaluateModel()
            {
                isCollected = false;
                isGood = false;
                isReceiveOrder = true;
            }
            public bool isCollected { get; set; }
            public bool isGood { get; set; }
            public bool isReceiveOrder { get; set; }
        }
    }
}
