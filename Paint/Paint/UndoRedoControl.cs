using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Paint
{
    class UndoRedoControl
    {
        public UndoRedoControl()
        {

        }

        int currentLogPosition = -1;
        private List<UIElement> Logs = new List<UIElement>();

        public List<UIElement> Undo()
        {
            if (currentLogPosition >= 0)
            {
                currentLogPosition--;

                return GetElements();
            }
            else
            {
                return new List<UIElement>();
            }
        }

        public List<UIElement> Redo()
        {
            if (currentLogPosition < Logs.Count-1)
            {
                currentLogPosition++;

                return GetElements();
            }
            else
            {
                return GetElements();
            }
        }

        private List<UIElement> GetElements()
        {
            List<UIElement> elements = new List<UIElement>();

            for (int i = 0; i < currentLogPosition + 1; i++)
            {
            elements.Add(Logs[i]);
            }

            return elements;
        }

        public void AddNormalLog(UIElement element, string type)
        {
            overrideLogs();

            Logs.Add(element);
            currentLogPosition++;
        }

        private void overrideLogs()
        {
            for (int i = Logs.Count - 1; i >= 0; i--)
            {
                if (i > currentLogPosition)
                {
                    Logs.RemoveAt(i);
                }
            }
        }

    }
}
