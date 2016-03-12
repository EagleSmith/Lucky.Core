using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Lucky.Hr.Core.Test
{
    [ProtoBuf.ProtoContract]
    public  class TestNews
    {
        [ProtoMember(1)]
        public Int32 ID { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }
        [ProtoMember(3)]
        public Int32 SortID { get; set; }
        [ProtoMember(4)]
        public bool IsValid { get; set; }
        [ProtoMember(5)]
        public DateTime CreateDateTime { get; set; }

    }
}
