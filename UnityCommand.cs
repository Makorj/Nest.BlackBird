using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBird
{
    public class UnityCommand : Command
    {
        public UnityCommand(string parameters, string workingDirectory = "") : base("Unity", parameters, workingDirectory)
        { }
    }
}
