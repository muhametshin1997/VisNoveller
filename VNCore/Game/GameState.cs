﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace VNCore.Game
{
    public class GameState
    {
        public string Name { get; set; }
        public string NovelPath { get; set; }
        public int SlideID { get; set; }
        public List<Link> Links { get; set; }
        public GameState()
        {
            Links = new List<Link>();
        }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                writer.WriteStartElement("Game");
                writer.WriteAttributeString("Name", Name);
                writer.WriteAttributeString("NovelPath", NovelPath);
                writer.WriteAttributeString("SlideID", SlideID.ToString());
                foreach (var current in Links)
                    writer.WriteRaw(current.ToString());
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        public static GameState Parse(string xml)
        {
            var result = new GameState();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                    if (reader.IsStartElement("Game"))
                    {
                        result.Name = reader.GetAttribute("Name");
                        result.NovelPath = reader.GetAttribute("NovelPath");
                        int slideID;
                        slideID = int.TryParse(reader.GetAttribute("SlideID"), out slideID) ? slideID : 0;
                        reader.Read();
                    }
                    else if (reader.IsStartElement("Link"))
                        result.Links.Add(Link.Parse(reader.ReadOuterXml()));
                    else reader.Read();
            return result;
        }
    }

}
