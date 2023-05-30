using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyHealth
{
    public class TaskListDropHandler : DefaultDropHandler
    {
        public override void DragOver(IDropInfo dropInfo)
        {
            base.DragOver(dropInfo);
            if (dropInfo.TargetGroup == null)
            {
                dropInfo.Effects = DragDropEffects.None;
            }
        }

        public override void Drop(IDropInfo dropInfo)
        {
            var data = ExtractData(dropInfo.Data).OfType<TaskView>().ToList();
            foreach (var groupedItem in data)
            {
                groupedItem.Grouping = (TaskView.TaskGroups)dropInfo.TargetGroup.Name;
            }

            MatchSourceListWithSortedList(dropInfo.TargetCollection);
            base.Drop(dropInfo);
        }

        private void MatchSourceListWithSortedList(IEnumerable targetCollection)
        {
            if(targetCollection is ListCollectionView collectionView){
                var sourceList = collectionView.TryGetList();
                var sortedList = collectionView.Cast<object>().ToList();

                int i = 0;
                foreach (var item in sortedList)
                {
                    if (sortedList[i] != item)
                        Move(sourceList, sourceList.IndexOf(item), i);
                    i++;
                }

            }
        }


    }


}
