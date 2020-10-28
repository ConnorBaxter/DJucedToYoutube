using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DJucedToYoutube
{
	[XmlRoot(ElementName="interval")]
	public class Interval 
    {
		[XmlAttribute(AttributeName="start")]
		public string Start { get; set; }
		[XmlAttribute(AttributeName="end")]
		public string End { get; set; }
	}

	[XmlRoot(ElementName="track")]
	public class Track 
    {
		[XmlElement(ElementName="interval")]
		public List<Interval> Interval { get; set; }
		[XmlAttribute(AttributeName="tonality")]
		public string Tonality { get; set; }
		[XmlAttribute(AttributeName="artist")]
		public string Artist { get; set; }
		[XmlAttribute(AttributeName="bpm")]
		public string Bpm { get; set; }
		[XmlAttribute(AttributeName="song")]
		public string Song { get; set; }
	}

	[XmlRoot(ElementName="recordEvents")]
	public class RecordEvents 
    {
		[XmlElement(ElementName="track")]
		public List<Track> Track { get; set; }
		[XmlAttribute(AttributeName="length")]
		public string Length { get; set; }
	}

}
