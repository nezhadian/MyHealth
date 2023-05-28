using GongSolutions.Wpf.DragDrop;
using System.Linq;
using System.Windows;
namespace MyHealth
{
    public class TaskListDropHandler : IDropTarget
    {
        public void DragOver(IDropInfo dropInfo)
        {
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragOver(dropInfo);
            if (dropInfo.TargetGroup == null)
            {
                dropInfo.Effects = DragDropEffects.None;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.Drop(dropInfo);

            var data = DefaultDropHandler.ExtractData(dropInfo.Data).OfType<TaskView>().ToList();
            foreach (var groupedItem in data)
            {
                groupedItem.Grouping = (TaskView.TaskGroups)dropInfo.TargetGroup.Name;
            }
        }
    }


}
