using System.Collections.Generic;

namespace Tasks
{
    public enum ValidationOperations
    {
        Match,
        Contains,
        GreaterThan,
        GreaterEqualThan,
        LessThan,
        LessEqualThan
    }

    public abstract class ValidationRule
    {
        public abstract bool Validate(Task task);
    }

    public class TypeMatchValidation : ValidationRule
    {
        TaskType taskType;

        public TypeMatchValidation(TaskType taskType)
        {
            this.taskType = taskType;
        }

        public override bool Validate(Task task)
        {
            return taskType == task.TaskType;
        }
    }

    public class TypeContainsValidation : ValidationRule
    {
        List<TaskType> taskTypes;

        public TypeContainsValidation(List<TaskType> taskType)
        {
            this.taskTypes = taskType;
        }

        public override bool Validate(Task task)
        {
            return taskTypes.Contains(task.TaskType);
        }
    }


    public class TaskValidator
    {
        private List<ValidationRule> validations;

        public TaskValidator()
        {
            validations = new List<ValidationRule>();
        }

        public bool Validate(Task task)
        {
            bool valid = true;
            foreach(ValidationRule v in validations)
            {
                valid = v.Validate(task);
                if (!valid) { 
                    return valid;
                }
            }
            return valid;
        }

        public TaskValidator AddValidationRule(ValidationOperations op, List<TaskType> validTaskTypes)
        {
            validations.Add(new TypeContainsValidation(validTaskTypes));
            return this;
        }

        public TaskValidator AddValidationRule(ValidationOperations op, TaskType validTaskType)
        {
            validations.Add(new TypeMatchValidation(validTaskType));
            return this;
        }
    }

}