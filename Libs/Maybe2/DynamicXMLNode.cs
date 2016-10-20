using System;
using System.Dynamic;
using System.Xml.Linq;

namespace Maybe2
{

    /// <summary>
    /// How Use
    /// var xe = XElement.Parse(xmlText);
    /// var dxn = new DynamicXmlNode(xe);
    /// var name = dxn.people.dmitri.name;
    /// 
    /// --------------------------------
    /// 
    /// dynamic contact = new DynamicXMLNode(“Contacts”);
    /// contact.Name = “Patrick Hines”;
    /// contact.Phone = “206-555-0144”;
    /// contact.Address = new DynamicXMLNode();
    /// contact.Address.Street = “123 Main St”;
    /// contact.Address.City = “Mercer Island”;
    /// contact.Address.State = “WA”;
    /// contact.Address.Postal = “68402”;
    /// </summary>
    public class DynamicXMLNode : DynamicObject
    {

        //ExpandoObject
        XElement node;
        public DynamicXMLNode(XElement node)
        {
            this.node = node;
        }
        public DynamicXMLNode()
        {
        }
        public DynamicXMLNode(String name)
        {
            node = new XElement(name);
        }
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            XElement setNode = node.Element(binder.Name);
            if (setNode != null)
                setNode.SetValue(value);
            else
            {
                if (value.GetType() == typeof(DynamicXMLNode))
                    node.Add(new XElement(binder.Name));
                else
                    node.Add(new XElement(binder.Name, value));
            }
            return true;
        }
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            XElement getNode = node.Element(binder.Name);
            if (getNode != null)
            {
                result = new DynamicXMLNode(getNode);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
