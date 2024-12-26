export interface TaskAssignment {
    assignementid?: number;
    taskId: number;
    userId: number;
    assignedDate: string;
    description: string;
    percentComplete: string;
    taskTitle: string;
}

export type TaskAssignmentPartial = Partial<TaskAssignment>;

export interface TaskAssignment {
    taskassignmentid: TaskAssignment[];
}

