// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Xml
{
    // Represents a processing instruction, which XML defines to keep
    // processor-specific information in the text of the document.
    public class XmlProcessingInstruction : XmlLinkedNode
    {
        private readonly string _target;
        private string _data;

        protected internal XmlProcessingInstruction(string target, string data, XmlDocument doc) : base(doc)
        {
            _target = target;
            _data = data;
        }

        // Gets the name of the node.
        public override String Name
        {
            get
            {
                return _target ?? string.Empty;
            }
        }

        // Gets the name of the current node without the namespace prefix.
        public override string LocalName
        {
            get { return Name; }
        }

        // Gets or sets the value of the node.
        public override String Value
        {
            get { return _data; }
            set { Data = value; } //use Data instead of data so that event will be fired
        }

        // Gets the target of the processing instruction.
        public String Target
        {
            get { return _target; }
        }

        // Gets or sets the content of processing instruction,
        // excluding the target.
        public String Data
        {
            get { return _data; }
            set
            {
                XmlNode parent = ParentNode;
                XmlNodeChangedEventArgs args = GetEventArgs(this, parent, parent, _data, value, XmlNodeChangedAction.Change);
                if (args != null)
                    BeforeEvent(args);
                _data = value;
                if (args != null)
                    AfterEvent(args);
            }
        }

        // Gets or sets the concatenated values of the node and
        // all its children.
        public override string InnerText
        {
            get { return _data; }
            set { Data = value; } //use Data instead of data so that change event will be fired
        }

        // Gets the type of the current node.
        public override XmlNodeType NodeType
        {
            get { return XmlNodeType.ProcessingInstruction; }
        }

        // Creates a duplicate of this node.
        public override XmlNode CloneNode(bool deep)
        {
            Debug.Assert(OwnerDocument != null);
            return OwnerDocument.CreateProcessingInstruction(_target, _data);
        }

        // Saves the node to the specified XmlWriter.
        public override void WriteTo(XmlWriter w)
        {
            w.WriteProcessingInstruction(_target, _data);
        }

        // Saves all the children of the node to the specified XmlWriter.
        public override void WriteContentTo(XmlWriter w)
        {
            // Intentionally do nothing
        }
    }
}
