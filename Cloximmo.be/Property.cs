using System.Xml.Serialization;
using System.Collections.Generic;
using System;

namespace Cloximmo.be
{
    [XmlRoot(ElementName = "kyero")]
    public class Kyero
    {
        [XmlElement(ElementName = "feed_version")]
        public string Feed_version { get; set; }
    }

    [XmlRoot(ElementName = "url")]
    public class Url
    {
        [XmlElement(ElementName = "en")]
        public string En { get; set; }
    }

    [XmlRoot(ElementName = "desc")]
    public class Desc
    {
        [XmlElement(ElementName = "en")]
        public string En { get; set; }
    }

    [XmlRoot(ElementName = "image")]
    public class Image
    {
        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlIgnore]
        public string PropertyId { get; set; }
    }

    [XmlRoot(ElementName = "images")]
    public class Images
    {
        [XmlElement(ElementName = "image")]
        public List<Image> Image { get; set; }

        [XmlIgnore]
        public string PropertyId { get; set; }
    }

    [XmlRoot(ElementName = "property")]
    public class Property
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "date")]
        public string Date { get; set; }

        [XmlElement(ElementName = "ref")]
        public string Ref { get; set; }

        [XmlElement(ElementName = "price")]
        public string Price { get; set; }

        [XmlElement(ElementName = "price_freq")]
        public string Price_freq { get; set; }

        [XmlElement(ElementName = "part_ownership")]
        public bool Part_ownership { get; set; }

        [XmlElement(ElementName = "leasehold")]
        public bool Leasehold { get; set; }

        [XmlElement(ElementName = "buildsize")]
        public string Buildsize { get; set; }

        [XmlElement(ElementName = "plotsize")]
        public string Plotsize { get; set; }

        [XmlElement(ElementName = "type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "town")]
        public string Town { get; set; }

        [XmlElement(ElementName = "province")]
        public string Province { get; set; }

        [XmlElement(ElementName = "beds")]
        public string Beds { get; set; }

        [XmlElement(ElementName = "baths")]
        public string Baths { get; set; }

        [XmlElement(ElementName = "url")]
        public Url Url { get; set; }

        [XmlElement(ElementName = "desc")]
        public Desc Desc { get; set; }

        [XmlElement(ElementName = "images")]
        public Images Images { get; set; }
    }

    [XmlRoot(ElementName = "root")]
    public class Root
    {
        [XmlElement(ElementName = "kyero")]
        public Kyero Kyero { get; set; }

        [XmlElement(ElementName = "property")]
        public List<Property> Properties { get; set; }
    }
}