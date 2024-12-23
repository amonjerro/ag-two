using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// This is a UI element that displays two children side-by-side
namespace DialogueEditor
{
    public class DialogueSplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<DialogueSplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}
