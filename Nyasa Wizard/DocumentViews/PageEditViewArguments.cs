using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nyasa_Wizard
{
    public class PageEditViewArguments: EventArgs
    {
        public MNDocument Document { get; set; }
        public MNPage Page { get; set; }
        public MNPageObject Object { get; set; }
        public PageEditView PageView { get; set; }
    }
}
