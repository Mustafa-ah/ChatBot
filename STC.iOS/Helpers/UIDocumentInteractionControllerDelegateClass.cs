﻿using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace STC.iOS.Helpers
{
    public class UIDocumentInteractionControllerDelegateClass : UIDocumentInteractionControllerDelegate
    {
        UIViewController ownerVC;

        public UIDocumentInteractionControllerDelegateClass(UIViewController vc)
        {
            ownerVC = vc;
        }

        public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
        {
            return ownerVC;
        }

        public override UIView ViewForPreview(UIDocumentInteractionController controller)
        {
            return ownerVC.View;
        }
    }
}