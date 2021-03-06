﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessContract.Infrastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string message, string prop, IEnumerable<string> errors = null)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
            Errors = errors;
        }

        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }
        public IEnumerable<string> Errors { get; private set; }
    }
}
