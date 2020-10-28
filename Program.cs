using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DJucedToYoutube
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 2) DisplayUsage();

            bool includeBPM = false;
            bool includeKey = false;
            bool includeEndTime = false;

            if(File.Exists("dj2tube.conf"))
            {
                string[] lines = File.ReadAllLines("dj2tube.conf");
                try
                {
                    string bpm = lines[0].Split(':')[1];
                    string key = lines[1].Split(':')[1];
                    string endtime = lines[2].Split(':')[1];

                    includeBPM = bool.Parse(bpm);
                    includeKey = bool.Parse(key);
                    includeEndTime = bool.Parse(endtime);
                }
                catch
                {
                    Console.WriteLine("Unable to parse config file!");
                }
            }

            XmlDocument document = new XmlDocument();

            document.Load(args[0]);

            XmlSerializer serializer = new XmlSerializer(typeof(RecordEvents));
            StringReader sr = new StringReader(File.ReadAllText(args[0]));
            RecordEvents recEvents = (RecordEvents)serializer.Deserialize(sr);

            List<SongInfo> songs = new List<SongInfo>();
            foreach(Track track in recEvents.Track)
            {
                SongInfo songInfo = new SongInfo();
                Console.WriteLine("Adding song {0}...", track.Song);
                try
                {
                    songInfo.Title = track.Song;
                    songInfo.Artist = track.Artist;
                    songInfo.BPM = track.Bpm;
                    songInfo.Key = track.Tonality;
                    List<Interval> intervals = track.Interval;
                    songInfo.StartTime = intervals[0].Start;
                    songInfo.EndTime = intervals[0].End;

                    songs.Add(songInfo);
                }
                catch
                {
                    Console.WriteLine("Error adding song...");
                }
            }

            string fileToWrite = "";
            foreach(SongInfo song in songs)
            {
                if(includeBPM || includeEndTime || includeKey)
                {
                    fileToWrite += song.ToString(includeBPM, includeKey, includeEndTime) + "\n";
                }
                else
                {
                    fileToWrite += song.ToString() + "\n";
                }
            }
            File.WriteAllText(args[1], fileToWrite);
        }
    
        static void DisplayUsage()
        {
            Console.WriteLine("Usage: DJucedToYoutube [PathToXMLFile].xml [outputfile].txt");
            Console.WriteLine("  See dj2tube.conf for more options");
        }
    }

    class SongInfo
    {
        //Normal attributes

        public string Artist = "";
        public string Title = "";
        public string StartTime = "";

        //extra attributes
        public string Key = "";
        public string BPM = "";
        public string EndTime = "";

        public override string ToString()
        {
            string result = GetTimeStamp(StartTime);
            result += ": ";

            result += Artist + " - " + Title;

            return result;
        }

        public string ToString(bool includeBpm, bool includeKey, bool includeEndTime)
        {
            string result = "";

            if(includeEndTime)
            {
                result += GetTimeStamp(StartTime) + " - " + GetTimeStamp(EndTime) + ": ";
            }
            else
            {
                result += StartTime += ": ";
            }

            result += Artist + " - " + Title;

            if(includeBpm)
            {
                result += " | BPM: " + BPM;
            }

            if(includeKey)
            {
                result += " | Key: " + Key.ToUpper();
            }

            return result;
        }

        static string GetTimeStamp(string time)
        {
            float seconds = ParseSeconds(time);

            double minutes = Math.Round(seconds / 60);
            seconds = seconds % 60;

            string result = minutes + ":" + Math.Round(seconds);

            return result;
        }

        static float ParseSeconds(string msStr)
        {
            float seconds = 0;
            if(float.TryParse(msStr, out seconds))
            {
                return seconds;
            }
            else
            {
                return 0;
            }
        }
    }
}
