using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FloppyDJ
{
    [XmlInclude(typeof(TimeInfo))]
    [XmlInclude(typeof(NoteInfo))]
    public struct TrackInfo
    {
        public List<List<MeasureElementInfo>> Parts;
    }

    [XmlInclude(typeof(TimeInfo))]
    [XmlInclude(typeof(NoteInfo))]
    public struct MeasureElementInfo
    {
        public MeasureElementInfoType Type;
        public object Element;
    }

    public enum MeasureElementInfoType
    {
        Note,
        Time
    }
    
    public struct TimeInfo
    {
        public int Beats;
        public int BeatType;
        public int Divisions;
    }
    
    public struct NoteInfo
    {
        public string Name;
        public double Duration;
    }
}
