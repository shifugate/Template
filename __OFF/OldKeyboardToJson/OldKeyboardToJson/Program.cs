using System.Xml;

void Main()
{
    XmlDocument doc = new XmlDocument();
    doc.Load("config.xml");

    XmlNodeList nodes = doc.ChildNodes[0].ChildNodes;

    for (int i = 2; i < nodes.Count; i++)
    {
        XmlNode node = nodes[i];

        Console.WriteLine(node.ToString());
    }
}

Main();