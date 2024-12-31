export interface Tasks {
    projectId?: number;
    title: string;
    description: string;
    startDate: string;
    endDate: string;
}

export type TasksPartial = Partial<Tasks>;

export interface Task {
    task: Tasks[];
}