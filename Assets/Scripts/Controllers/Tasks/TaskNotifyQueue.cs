using System;
using System.Collections.Generic;


namespace Tasks
{
    public static class TaskNotifyQueue
    {
        static Queue<TaskNotify> taskNotifications = new Queue<TaskNotify>();
        public static Action newEvent;

        public static void AddTaskNotification(TaskNotify taskNotify)
        {
            taskNotifications.Enqueue(taskNotify);
            newEvent?.Invoke();
        }

        public static void AcknowledgeNotification() {
            taskNotifications.Dequeue();
        }

        public static TaskNotify ViewTaskNotify() {
            return taskNotifications.Peek();
        }
    }
}
