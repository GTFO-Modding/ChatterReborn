using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Extra
{
    public class PlayerDialogNode
    {

        public string Label { get; set; }
        public uint DialogID { get; set; }

        public string Description { get; set; }

        public PlayerDialogNode NextNode { get; set; }

        public PlayerDialogNode PreviousNode { get; set; }


        public List<DialogOption> OptionsList
        {
            get
            {
                List<DialogOption> options = new List<DialogOption>();
                options.Add(new DialogOption(this.Label, true));
                var currentNode = this;
                int count = 0;
                while (count < 5)
                {
                    count++;
                    if (currentNode.PreviousNode != null)
                    {
                        options.Insert(0, new DialogOption(currentNode.PreviousNode.Label));
                        currentNode = currentNode.PreviousNode;
                    }
                }
                count = 0;
                currentNode = this;
                while (count < 5)
                {
                    count++;
                    if (currentNode.NextNode != null)
                    {
                        options.Add(new DialogOption(currentNode.NextNode.Label));
                        currentNode = currentNode.NextNode;
                    }
                }

                return options;
            }
        }
    }
}
